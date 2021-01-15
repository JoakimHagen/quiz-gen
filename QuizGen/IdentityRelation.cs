using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool CreateQuestion(Random seed, Knowledge knowledge)
        {
            var choice = seed.Next(0, 2) > 0;
            var ok = choice
                ? AskForIdentity(seed, knowledge)
                : AskForSubjects(seed, knowledge);
            if (!ok)
            {
                ok = !choice
                    ? AskForIdentity(seed, knowledge)
                    : AskForSubjects(seed, knowledge);
            }
            return ok;
        }

        private bool AskForIdentity(Random seed, Knowledge knowledge)
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
                return false;
            }

            var answerPool = new List<string>();

            answerPool.Add(correct[seed.Next(0, correct.Length)]);

            distractions.Shuffle(seed);

            foreach (var answer in distractions.Take(5))
            {
                answerPool.Add(answer);
            }

            Console.WriteLine("Q: " + subject + " is which of the following?");

            answerPool.Shuffle(seed);

            foreach (var answer in answerPool)
            {
                Console.WriteLine(" - A " + answer);
            }

            Console.WriteLine("\nA: " + String.Join(", ", correct.Where(x => answerPool.Contains(x))));
            return true;
        }

        private bool AskForSubjects(Random seed, Knowledge knowledge)
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
                return false;
            }

            var answerPool = new List<string>();

            answerPool.Add(correct[seed.Next(0, correct.Length)]);

            distractions.Shuffle(seed);

            foreach (var answer in distractions.Take(5))
            {
                answerPool.Add(answer);
            }

            Console.WriteLine("Q: Which of the following is a " + identity + "?");

            answerPool.Shuffle(seed);

            foreach (var answer in answerPool)
            {
                Console.WriteLine(" - " + answer);
            }

            Console.WriteLine("\nA: " + String.Join(", ", correct.Where(x => answerPool.Contains(x))));

            return true;
        }
    }
}
