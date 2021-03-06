﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QuizGen
{
    public class Query
    {
        public string FormatString;

        private string[] patterns;

        public Query(string template)
        {
            var str = new StringBuilder();
            var start = 0;

            var patternsList = new List<string>();

            var matches = Regex.Matches(template, "{[^}]+}");

            foreach (Match match in matches)
            {
                str.Append(template.Substring(start, match.Index - start));

                // "{0}" is a legal substitution, but not graph pattern
                if (Regex.IsMatch(match.Value, "{[0-9]+}"))
                {
                    str.Append(match.Value); // add "{#}" back to the string as it is
                }
                else
                {
                    str.Append('{');
                    str.Append(patternsList.Count);
                    str.Append('}');

                    patternsList.Add(match.Value.Substring(1, match.Length - 2));
                }
                start = match.Index + match.Length;

            }
            str.Append(template.Substring(start, template.Length - start));
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

            var paths = patterns
                .Select(x => knowledge.ParsePattern(x))
                .ToArray();

            if (paths.Any(x => x.name == null))
                return null;

            foreach (var rel in knowledge.Relations)
            {
                foreach (var path in paths)
                {
                    if (rel.name == path.name)
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

        public string[] GetSubstitutions(Knowledge knowledge, Random seed, string answer)
        {
            var str = new string[patterns.Length];
            for (int i = 0; i < patterns.Length; i++)
            {
                var options = knowledge.TracePattern(patterns[i], answer);
                if (options.Length == 0)
                    return null;

                var prior = str.Take(i);
                while (true)
                {
                    str[i] = seed.Choose(options);

                    // If the node is not already picked, break the loop
                    if (!prior.Contains(str[i]))
                        break;

                    // If all our options are exhausted, return null
                    else if (options.Intersect(prior).Count() == options.Length)
                        return null;
                }
            }
            return str;
        }
    }
}
