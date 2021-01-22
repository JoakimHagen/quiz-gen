using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizGen
{
    public class Query
    {
        public string Template;

        public string FormatString;

        private string[] patterns;

        public Query(string template)
        {
            Template = template;
            var str = new StringBuilder();
            var start = 0;

            var patternsList = new List<string>();

            while (start < template.Length)
            {
                var index = template.IndexOf('{', start);
                if (index == -1)
                {
                    str.Append(template.Substring(start, template.Length - start));
                    break;
                }

                str.Append(template.Substring(start, index - start));
                start = index + 1;

                index = template.IndexOf('}', start);
                if (index == -1)
                    index = template.Length;

                var pattern = template.Substring(start, index - start);
                str.Append($"{{{patternsList.Count}}}");
                start = index + 1;

                patternsList.Add(pattern);
            }

            patterns = patternsList.ToArray();
            FormatString = str.ToString();
        }

        public string[] GetAnswers(Knowledge knowledge, string[] substitutions)
        {
            IEnumerable<string> answers = null;
            for (int i = 0; i < patterns.Length; i++)
            {
                if (answers == null)
                {
                    answers = knowledge.TracePatternReverse(patterns[i], substitutions[i]);
                }
                else
                {
                    answers = answers.Intersect(knowledge.TracePatternReverse(patterns[i], substitutions[i]));
                }
            }

            return answers.ToArray();
        }

        public string[] GetAnswerCandidates(Knowledge knowledge)
        {
            var candidateDict = new Dictionary<string, int>();

            var paths = patterns.Select(x => knowledge.ParsePattern(x));

            foreach (var rel in knowledge.Relations)
            {
                foreach (var path in paths)
                {
                    if (rel.name != path.name)
                    {
                        var c = path.isLeft ? rel.target : rel.subject;
                        if (candidateDict.ContainsKey(c))
                        {
                            candidateDict[c] += 1;
                        }
                        else
                        {
                            candidateDict[c] = 1;
                        }
                    }
                }
            }

            // The same candidates must have appeared in all predicates

            return candidateDict
                .Where(x => x.Value >= patterns.Length)
                .Select(x => x.Key)
                .ToArray();
        }

        public string[] GetSubstitutions(Knowledge knowledge, string answer)
        {
            var str = new string[patterns.Length];
            for (int i = 0; i < patterns.Length; i++)
            {
                str[i] = knowledge.TracePattern(patterns[i], answer).FirstOrDefault();
            }
            return str;
        }
    }
}
