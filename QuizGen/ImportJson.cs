using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuizGen
{
    public static class ImportJson
    {
        public static Knowledge ReadFile(string path)
        {
            Knowledge k;
            using (var sr = File.OpenText(path))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer();
                var lookupTable = new List<string>();
                
                var knowledgeConverter = new KnowledgeConverter();
                knowledgeConverter.lookupTable = lookupTable;

                var namedRelationConverter = new NamedRelationConverter();
                namedRelationConverter.lookupTable = lookupTable;

                serializer.Converters.Add(knowledgeConverter);
                serializer.Converters.Add(namedRelationConverter);

                k = serializer.Deserialize<Knowledge>(jsonTextReader);
            }

            return k;
        }
    }

    internal class KnowledgeConverter : JsonConverter
    {
        internal List<string> lookupTable;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Knowledge).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject jsonKnowledge = JObject.Load(reader);

                var k = new Knowledge();

                k.QuestionTemplates = jsonKnowledge["questionTemplates"]
                    .AsJEnumerable()
                    .Select(x => (string)x)
                    .ToList();

                if (jsonKnowledge.ContainsKey("lookup"))
                {
                    lookupTable.AddRange(jsonKnowledge["lookup"].Select(x => (string)x));
                }

                k.Relations = serializer.Deserialize<NamedRelation[]>(jsonKnowledge["relations"].CreateReader())
                    .ToList();

                return k;
            }
            return null;
        }
    }

    internal class NamedRelationConverter : JsonConverter
    {
        internal List<string> lookupTable;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(NamedRelation[]).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray relationArray = JArray.Load(reader);
                var relations = new NamedRelation[relationArray.Count];

                for (int i = 0; i < relationArray.Count; i++)
                {
                    JToken rel = relationArray[i];
                    if (rel.Type == JTokenType.Array)
                    {
                        var v = rel.Select(x =>
                        {
                            if (x.Type == JTokenType.Integer)
                            {
                                return lookupTable[(int)x];
                            }
                            else
                            {
                                return (string)x;
                            }
                        }).Take(3).ToArray();

                        var subject = v[0];
                        var name = v[1];
                        var target = v[2];

                        relations[i] = new NamedRelation(subject, name, target);
                    }
                }
                return relations;
            }
            return null;
        }
    }
}
