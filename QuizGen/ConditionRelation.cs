using System;
using System.Linq;

namespace QuizGen
{
    public class ConditionRelation
    {
        public FeatureCondition condition;
        public FeatureRelation featureRelation;

        public ConditionRelation(FeatureRelation featureRelation, FeatureCondition condition)
        {
            if (featureRelation == null) throw new ArgumentNullException("feature");
            this.featureRelation = featureRelation;
            this.condition = condition;
        }

        public Question CreateQuestion(Random seed, Knowledge knowledge)
        {
            var distractions = knowledge.DistractConditions
                .Where(x => x.featureRelation == featureRelation)
                .Select(x => x.condition.action)
                .ToArray();

            if (distractions.Length == 0)
            {
                return null;
            }

            distractions.Shuffle(seed);
            distractions = distractions.Take(5).ToArray();

            for (var i = 0; i < distractions.Length; i++)
            {
                distractions[i] = "by " + distractions[i];
            }

            return new Question
            {
                Stem = $"What are the required actions for supporting {featureRelation.feature.feature} with {featureRelation.subject}?",
                Correct = new string[] { "by " + condition.action },
                Distraction = distractions
            };
        }
    }
}
