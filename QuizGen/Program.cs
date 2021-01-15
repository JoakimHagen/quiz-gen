using System;

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
                var ok = random.Choose<Random, bool>(AskAboutIdentity, AskAboutFeature)(random);

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
