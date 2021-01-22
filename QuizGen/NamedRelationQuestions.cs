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
            var template = seed.Choose(knowledge.QuestionTemplates);

            var query = new Query(template);

            var answer = seed.Choose(query.GetAnswerCandidates(knowledge));

            var substs = query.GetSubstitutions(knowledge, seed, answer);

            if (substs == null)
                return null;

            var allAnswers = query.GetAnswers(knowledge, substs);

            var stem = string.Format(query.FormatString, substs);

            var distractors = knowledge.FindSimilar(allAnswers)
                .Distinct()
                .Where(x => !allAnswers.Contains(x))
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
                Answers = allAnswers,
                Distractors = distractors
            };
        }
    }
}
