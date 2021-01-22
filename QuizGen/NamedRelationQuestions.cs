using System;
using System.Linq;

namespace QuizGen
{
    public class NamedRelationQuestions
    {
        Knowledge knowledge;

        public NamedRelationQuestions(Knowledge knowledge)
        {
            this.knowledge = knowledge;
        }

        public Question Question(Random seed)
        {
            var template = seed.Choose(
                "{<id} belong in which category?",
                "Which options belong in category {id>}?",
                "Which options are a feature of {<feature}?",
                "Which options support {feature>}?",
                "Which options can {ability>}?",
                "How would you make sure {<condition} is enabled?",
                "What feature of {<feature} can {ability>}?"
                );

            return ExpandTemplate(seed, template);
        }

        private Question FillDistractors(Random seed, string stem, string[] answers)
        {
            var distractors = knowledge.FindSimilar(answers)
                .Distinct()
                .Where(x => !answers.Contains(x))
                .ToArray();

            if (distractors.Length == 0)
            {
                return null;
            }

            distractors.Shuffle(seed);
            distractors = distractors.Take(5).ToArray();

            return new Question
            {
                Stem = stem,
                Answers = answers,
                Distractors = distractors
            };
        }

        private Question ExpandTemplate(Random seed, string template)
        {
            var query = new Query(template);

            var answer = seed.Choose(query.GetAnswerCandidates(knowledge));

            var substs = query.GetSubstitutions(knowledge, answer);

            var allAnswers = query.GetAnswers(knowledge, substs);

            var stem = string.Format(query.FormatString, substs);

            return FillDistractors(seed, stem, allAnswers);
        }
    }
}
