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

        public string[] FindSimilar(IEnumerable<string> items)
        {
            var identityOfItems = IdentityRelation.IdentitiesOf(this, items);

            var similar = IdentityRelation.SubjectsOf(this, identityOfItems)
                .Where(x => !items.Contains(x))
                .ToArray();

            return similar;
        }
    }
}
