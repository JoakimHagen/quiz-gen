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

            var itemsAsfeatures = Relations
                .Where(x => items.Contains(x.target) && x.name == "feature")
                .Select(x => x.subject)
                .ToArray();

            if (itemsAsfeatures.Length > 0)
            {
                var similarFeatures = Relations
                    .Where(x => itemsAsfeatures.Contains(x.subject) && x.name == "feature")
                    .Select(x => x.target);

                similar = similar
                    .Concat(similarFeatures)
                    .Distinct()
                    .ToArray();
            }

            var relationsTo = Relations
                .Where(x => items.Contains(x.target))
                .GroupBy(x => x.name);

            foreach (var rel in relationsTo)
            {
                var key = rel.Key;
                if (key.StartsWith('!'))
                {
                    key = key.Substring(1);
                }
                else
                {
                    key = "!" + key;
                }

                similar = similar
                    .Concat(Relations
                        .Where(x => x.name == key && rel.Any(y => y.subject == x.subject))
                        .Select(x => x.target))
                    .Distinct()
                    .ToArray();
            }

            return similar;
        }
    }
}
