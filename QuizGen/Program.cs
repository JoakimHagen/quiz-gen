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
                var question = random.Choose<Random, Question>(AskAboutIdentity, AskAboutFeature)(random);

                if (question != null)
                {
                    question.PrintToConsole(random);
                    Console.ReadLine();

                    Console.WriteLine($"A: {String.Join(", ", question.Correct)}\n\n");
                }
            }
        }

        private static Question AskAboutFeature(Random seed)
        {
            var i = seed.Next(0, knowledge.Features.Count);

            var relation = knowledge.Features[i];

            return relation.CreateQuestion(seed, knowledge);
        }

        private static Question AskAboutIdentity(Random seed)
        {
            var i = seed.Next(0, knowledge.Identities.Count);

            var relation = knowledge.Identities[i];

            return relation.CreateQuestion(seed, knowledge);
        }
    }
}
