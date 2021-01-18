using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuizGen
{
    public class NamedRelationQuestions
    {
        Knowledge knowledge;

        public NamedRelationQuestions(Knowledge knowledge)
        {
            this.knowledge = knowledge;
        }

        public Question Question(Random seed, NamedRelation relation)
        {
            if (relation.name == "id")
            {
                return AskForIdentity(seed, relation);
            }
            else if (relation.name == "feature")
            {
                return AskForFeatureSubjects(seed, relation);
            }
            else if (relation.name == "ability")
            {
                return AskForAbilitySubjects(seed, relation);
            }
            else return null;
        }

        private Question AskForIdentity(Random seed, NamedRelation relation)
        {
            var direction = seed.Next(0, 2) > 0;

            var answers = direction
                ? knowledge.Relations
                    .Where(x => x.subject == relation.subject && x.name == relation.name)
                    .Select(x => x.target)
                    .ToArray()
                : knowledge.Relations
                    .Where(x => x.target == relation.target && x.name == relation.name)
                    .Select(x => x.subject)
                    .ToArray();

            var distractors = knowledge.FindSimilar(answers)
                .Where(x => !answers.Contains(x))
                .ToArray();

            if (distractors.Length == 0)
            {
                return null;
            }

            distractors.Shuffle(seed);
            distractors = distractors.Take(5).ToArray();

            var stem = "";
            if (direction)
            {
                stem = relation.subject + " is which of the following?";
            }
            else
            {
                var an = Regex.IsMatch(relation.target, "^[aeiouyæøåAEIOUYÆØÅ]") ? "an" : "a";
                stem = $"Which of the following is {an} {relation.target}?";
            }

            return new Question
            {
                Stem = stem,
                Answers = answers,
                Distractors = distractors
            };
        }

        private Question AskForFeatureSubjects(Random seed, NamedRelation relation)
        {
            var answers = knowledge.Relations
                .Where(x => x.target == relation.target && x.name == relation.name)
                .Select(x => x.subject)
                .ToArray();

            var distractors = knowledge.FindSimilar(answers);

            if (distractors.Length == 0)
            {
                return null;
            }

            distractors.Shuffle(seed);
            distractors = distractors.Take(5).ToArray();

            var stem = $"Which of the following support {relation.target}?";

            return new Question
            {
                Stem = stem,
                Answers = answers,
                Distractors = distractors
            };
        }

        private Question AskForAbilitySubjects(Random seed, NamedRelation relation)
        {
            var features = knowledge.Relations
                .Where(x => x.target == relation.target && x.name == relation.name)
                .Select(x => x.subject)
                .ToArray();

            var answers = knowledge.Relations
                .Where(x => x.name == "feature" && features.Contains(x.target))
                .Select(x => x.subject)
                .ToArray();

            var distractors = knowledge.FindSimilar(answers);

            if (distractors.Length == 0)
            {
                return null;
            }

            distractors.Shuffle(seed);
            distractors = distractors.Take(5).ToArray();

            var stem = $"Which of the following can {relation.target}?";

            return new Question
            {
                Stem = stem,
                Answers = answers,
                Distractors = distractors
            };
        }
    }
}
