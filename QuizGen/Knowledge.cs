using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizGen
{
    /// <summary>
    /// A repository of graphs, describing relational knowledge
    /// </summary>
    public class Knowledge
    {
        public List<string> QuestionTemplates;

        public List<NamedRelation> Relations;

        public string[] FindSimilar(string[] items)
        {
            var targetRels = new List<NamedRelation>();
            var subjectRels = new List<NamedRelation>();

            foreach (var rel in Relations)
            {
                if (items.Contains(rel.subject))
                {
                    targetRels.Add(rel);
                }

                if (items.Contains(rel.target))
                {
                    subjectRels.Add(rel);
                }
            }

            var countedTargets = targetRels.GroupBy(x => (x.target, name: TrimExclamation(x.name)))
                .Select(g => (g.Key.target, g.Key.name, count: g.Count()))
                .ToArray();

            var countedSubjects = subjectRels.GroupBy(x => (x.subject, name: TrimExclamation(x.name)))
                .Select(g => (g.Key.subject, g.Key.name, count: g.Count()))
                .ToArray();

            var backRefCounts = new List<(string item, int count)>();

            foreach (var rel in Relations)
            {
                var name = TrimExclamation(rel.name);

                if (!items.Contains(rel.subject))
                {
                    var count = countedTargets.Sum(x => (x.target == rel.target && x.name == name) ? x.count : 0);
                    if (count > 0)
                    {
                        backRefCounts.Add((rel.subject, count));
                    }
                }

                if (!items.Contains(rel.target))
                {
                    var count = countedSubjects.Sum(x => (x.subject == rel.subject && x.name == name) ? x.count : 0);
                    if (count > 0)
                    {
                        backRefCounts.Add((rel.target, count));
                    }
                }
            }

            var similarItems = backRefCounts.GroupBy(x => x.item)
                .Select(g => (item: g.Key, count: g.Sum(x => x.count)));

            return similarItems
                .OrderByDescending(x => x.count)
                .Select(x => x.item)
                .Take(Math.Max(4 - items.Length, 1))
                .ToArray();
        }

        public string[] TracePattern(string pattern, string origin)
        {
            string[] validEndpoints = null;
            var path = ParsePattern(pattern);

            if (path.isLeft)
            {
                validEndpoints = Relations
                    .Where(x => x.name == path.name && x.target == origin)
                    .Select(x => x.subject)
                    .ToArray();
            }
            else
            {
                validEndpoints = Relations
                    .Where(x => x.name == path.name && x.subject == origin)
                    .Select(x => x.target)
                    .ToArray();
            }
            return validEndpoints;
        }

        public string[] TracePatternReverse(string pattern, string endPoint)
        {
            string[] validOrigins = null;
            var path = ParsePattern(pattern);

            if (path.isLeft)
            {
                validOrigins = Relations
                    .Where(x => x.name == path.name && x.subject == endPoint)
                    .Select(x => x.target)
                    .ToArray();
            }
            else
            {
                validOrigins = Relations
                    .Where(x => x.name == path.name && x.target == endPoint)
                    .Select(x => x.subject)
                    .ToArray();
            }
            return validOrigins;
        }

        public (string name, bool isLeft) ParsePattern(string pattern)
        {
            if (pattern.StartsWith("<"))
            {
                return (pattern.Substring(1), true);
            }
            else if (pattern.EndsWith(">"))
            {
                return (pattern.Substring(0, pattern.Length - 1), false);
            }
            return (null, false);
        }

        private static string TrimExclamation(string name)
        {
            return name.StartsWith('!') ? name.Substring(1) : name;
        }
    }
}
