using System;
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

        public Question CreateQuestion(Random seed, Knowledge knowledge)
        {
            var correct = knowledge.Features
                .Where(x => x.feature == feature)
                .Select(x => x.subject)
                .Distinct()
                .ToArray();

            var distractions = knowledge.FindSimilar(correct)
                .Where(x => !correct.Contains(x))
                .ToArray();

            if (distractions.Length == 0)
            {
                return null;
            }

            distractions.Shuffle(seed);
            distractions = distractions.Take(5).ToArray();

            var stem = seed.Choose(
                $"Which of the following support {feature.feature}?",
                $"Which of the following can {feature.action}?");

            return new Question
            {
                Stem = stem,
                Correct = correct,
                Distraction = distractions
            };
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
