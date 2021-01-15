using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizGen
{
    class Program
    {
        static readonly Knowledge knowledge = new ExampleData();

        static void Main(string[] args)
        {
            var random = new Random();

            while (true)
            {
                var ok = random.Next(0, 2) > 0
                    ? AskAboutFeature(random)
                    : AskAboutIdentity(random);

                if (ok) Console.ReadLine();
            }
        }

        private static bool AskAboutFeature(Random seed)
        {
            var i = seed.Next(0, knowledge.Features.Count);

            var relation = knowledge.Features[i];

            return relation.CreateQuestion(seed, knowledge);
        }

        private static bool AskAboutIdentity(Random seed)
        {
            var i = seed.Next(0, knowledge.Identities.Count);

            var relation = knowledge.Identities[i];

            return relation.CreateQuestion(seed, knowledge);
        }
    }
}
