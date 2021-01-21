using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizGen
{
    public class Query
    {
        public string Template;

        public string FormatString;

        private Func<Knowledge, NamedRelation, string>[] isCandidate;

        private Func<Knowledge, string, string>[] substitute;

        private string[] patterns;

        public Query(string template)
        {
            Template = template;
            var str = new StringBuilder();
            var start = 0;

            var patternsList = new List<string>();
            var isCandidates = new List<Func<Knowledge, NamedRelation, string>>();
            var substitutes  = new List<Func<Knowledge, string, string>>();

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
                str.Append($"{{{isCandidates.Count}}}");
                start = index + 1;

                patternsList.Add(pattern);
                isCandidates.Add(ParsePatternCandidate(pattern));
                substitutes.Add(ParsePatternSubst(pattern));
            }

            patterns = patternsList.ToArray();
            isCandidate = isCandidates.ToArray();
            substitute = substitutes.ToArray();
            FormatString = str.ToString();
        }

        /// <summary>
        /// Parses the pattern, returning a function that will return a candidate datapoint if the relation matches.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        private Func<Knowledge, NamedRelation, string> ParsePatternCandidate(string pattern)
        {
            return (Knowledge knowledge, NamedRelation relation) =>
            {
                if (pattern.StartsWith("<"))
                {
                    var name = pattern.Substring(1);

                    if (relation.name == name)
                    {
                        return relation.target;
                    }
                }
                else if (pattern.EndsWith(">"))
                {
                    var name = pattern.Substring(0, pattern.Length - 1);

                    if (relation.name == name)
                    {
                        return relation.subject;
                    }
                }
                return null;
            };
        }

        /// <summary>
        /// Parses the pattern, returning a function that will follow the relation from an answer to the datapoint in the pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        private Func<Knowledge, string, string> ParsePatternSubst(string pattern)
        {
            return (Knowledge knowledge, string answer) =>
            {
                if (pattern.StartsWith("<"))
                {
                    var name = pattern.Substring(1);

                    return knowledge.Relations
                        .Where(x => x.name == name && x.target == answer)
                        .Select(x => x.subject)
                        .FirstOrDefault();
                }
                else if (pattern.EndsWith(">"))
                {
                    var name = pattern.Substring(0, pattern.Length - 1);

                    return knowledge.Relations
                        .Where(x => x.name == name && x.subject == answer)
                        .Select(x => x.target)
                        .FirstOrDefault();
                }
                return null;
            };
        }

        public string[] GetAnswers(Knowledge knowledge, string[] substitutions)
        {
            // reverseSubst is basically just following relation from pattern to answer(origin)
            Func<Knowledge, string, string, IEnumerable<string>> reverseSubst = (knowledge, pattern, subst) =>
            {
                if (pattern.StartsWith("<"))
                {
                    var name = pattern.Substring(1);

                    return knowledge.Relations
                        .Where(x => x.name == name && x.subject == subst)
                        .Select(x => x.target);
                }
                else if (pattern.EndsWith(">"))
                {
                    var name = pattern.Substring(0, pattern.Length - 1);

                    return knowledge.Relations
                        .Where(x => x.name == name && x.target == subst)
                        .Select(x => x.subject);
                }
                return null;
            };

            IEnumerable<string> answers = null;
            for (int i = 0; i < patterns.Length; i++)
            {
                if (answers == null)
                {
                    answers = reverseSubst(knowledge, patterns[i], substitutions[i]);
                }
                else
                {
                    answers = answers.Intersect(reverseSubst(knowledge, patterns[i], substitutions[i]));
                }
            }

            return answers.ToArray();
        }
        
        public string[] GetCandidates(Knowledge knowledge)
        {
            var candidateDict = new Dictionary<string, int>();

            foreach (var rel in knowledge.Relations)
            {
                foreach (var isCand in isCandidate)
                {
                    var c = isCand(knowledge, rel);
                    if (c != null)
                    {
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
                .Where(x => x.Value == isCandidate.Length)
                .Select(x => x.Key)
                .ToArray();
        }

        public string[] GetSubstitutions(Knowledge knowledge, string answer)
        {
            var str = new string[substitute.Length];
            for (int i = 0; i < substitute.Length; i++)
            {
                str[i] = substitute[i](knowledge, answer);
            }
            return str;
        }


    }
}
