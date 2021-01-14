using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizGen
{
    public class FeatureRelation
    {
        public string subject;
        public Feature feature;

        public FeatureRelation(string subject, Feature feature)
        {
            if (feature == null) throw new ArgumentNullException("feature");
            this.subject = subject;
            this.feature = feature;
        }

        public bool CreateQuestion(Random seed, IEnumerable<FeatureRelation> graph)
        {
            var correct = graph
                .Where(x => x.feature == feature)
                .Select(x => x.subject)
                .Distinct()
                .ToArray();

            var identityofcorrect = IdentityRelation.IdentitiesOf(ExampleData.idrelations, correct);

            var distractions = IdentityRelation.SubjectsOf(ExampleData.idrelations, identityofcorrect)
                .Where(x => !correct.Contains(x))
                .ToArray();

            if (distractions.Length == 0)
            {
                return false;
            }

            var answerPool = new List<string>();

            answerPool.Add(correct[seed.Next(0, correct.Length)]);

            distractions.Shuffle(seed);

            foreach (var answer in distractions.Take(5))
            {
                answerPool.Add(answer);
            }

            var q = seed.Next(0, 2) > 0
                ? $"Q: Which of the following support {feature.feature}?"
                : $"Q: Which of the following can {feature.action}?";
            Console.WriteLine(q);

            answerPool.Shuffle(seed);

            foreach (var answer in answerPool)
            {
                Console.WriteLine(" - " + answer);
            }

            Console.WriteLine("\nA: " + String.Join(", ", correct.Where(x => answerPool.Contains(x))));

            return true;
        }
    }

    // Azure Front Door supports network firewall traffic rules
    //     through Frontend (Domain) components' association with a WAF Policy
    // Azure Front Door supports blocking network traffic
    // Azure Front Door can block network traffic based on OSI Layer 7 data
    // Azure Front Door has firewall features
    // Azure Front Door's individual Frontends (Domains) can be associated with a WAF Policy
    // Azure Front Door can use multiple WAF Policies
    //     through multiple Frontend (Domain) components
    // Azure Front Door is a firewall (contentious phrasing)
}
