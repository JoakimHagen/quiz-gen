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
        public List<NamedRelation> Relations;

        public string[] FindSimilar(IEnumerable<string> items)
        {
            var identityOfItems = Relations
                .Where(x => items.Contains(x.subject) && x.name == "id")
                .Select(x => x.target)
                .ToArray();

            var similar = Relations
                .Where(x => identityOfItems.Contains(x.target) && x.name == "id")
                .Select(x => x.subject)
                .Where(x => !items.Contains(x))
                .ToArray();

            var featuresOfItems = Relations
                .Where(x => items.Contains(x.subject) && x.name == "feature")
                .Select(x => x.target)
                .ToArray();

            if (featuresOfItems.Length > 0)
            {
                var subjectsWithSimilarFeatures = Relations
                    .Where(x => x.name == "feature")
                    .GroupBy(x => x.subject)
                    .Select(g => (subject: g.Key, count: g.Count(x => featuresOfItems.Contains(x.target))))
                    .Where(g => g.count > 0 && !items.Contains(g.subject))
                    .OrderByDescending(g => g.count)
                    .Take(5);

                similar = similar
                    .Concat(subjectsWithSimilarFeatures.Select(g => g.subject))
                    .Distinct()
                    .ToArray();
            }

            return similar;
        }
    }
}
