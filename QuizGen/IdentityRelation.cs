using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuizGen
{
    public class IdentityRelation
    {
        public string subject;
        public string identity;

        public IdentityRelation(string subject, string identity)
        {
            this.subject = subject;
            this.identity = identity;
        }

        public static IEnumerable<string> IdentitiesOf(Knowledge knowledge, IEnumerable<string> subject)
        {
            return knowledge.Identities
                .Where(x => subject.Contains(x.subject))
                .Select(x => x.identity)
                .Distinct();
        }

        public static IEnumerable<string> SubjectsOf(Knowledge knowledge, IEnumerable<string> identities)
        {
            return knowledge.Identities
                .Where(x => identities.Contains(x.identity))
                .Select(x => x.subject)
                .Distinct();
        }

        public Question CreateQuestion(Random seed, Knowledge knowledge)
        {
            var choice = seed.Next(0, 2) > 0;
            var q = choice
                ? AskForIdentity(seed, knowledge)
                : AskForSubjects(seed, knowledge);
            if (q == null)
            {
                q = !choice
                    ? AskForIdentity(seed, knowledge)
                    : AskForSubjects(seed, knowledge);
            }
            return q;
        }

        private Question AskForIdentity(Random seed, Knowledge knowledge)
        {
            var correct = knowledge.Identities
                .Where(x => x.subject == subject)
                .Select(x => x.identity)
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

            return new Question
            {
                Stem = subject + " is which of the following?",
                Correct = correct,
                Distraction = distractions
            };
        }

        private Question AskForSubjects(Random seed, Knowledge knowledge)
        {
            var correct = knowledge.Identities
                .Where(x => x.identity == identity)
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

            var an = Regex.IsMatch(identity, "^[aeiouyæøåAEIOUYÆØÅ]") ? "an": "a";

            return new Question
            {
                Stem = $"Which of the following is {an} {identity}?",
                Correct = correct,
                Distraction = distractions
            };
        }
    }
}
