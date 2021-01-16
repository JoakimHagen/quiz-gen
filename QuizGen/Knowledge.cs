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
        public List<IdentityRelation> Identities;
        public List<FeatureRelation> Features;
        public List<ConditionRelation> Conditions;
        public List<ConditionRelation> DistractConditions;

        public string[] FindSimilar(IEnumerable<string> items)
        {
            var identityOfItems = IdentityRelation.IdentitiesOf(this, items);

            var similar = IdentityRelation.SubjectsOf(this, identityOfItems)
                .Where(x => !items.Contains(x))
                .ToArray();

            var features = Features
                .Where(x => items.Contains(x.subject))
                .Select(x => x.feature)
                .ToArray();

            if (features.Length > 0)
            {
                var featuresBySimilarity = Features
                    .GroupBy(x => x.subject)
                    .Select(g => (subject: g.Key, count: g.Count(x => features.Contains(x.feature))))
                    .OrderBy(g => g.count);

                similar = similar
                    .Concat(featuresBySimilarity.Select(x => x.subject))
                    .Distinct()
                    .ToArray();
            }

            return similar;
        }
    }
}
