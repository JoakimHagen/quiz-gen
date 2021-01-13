using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizGen
{
    class Program
    {
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
            var i = seed.Next(0, ExampleData.featureRelations.Count);

            var relation = ExampleData.featureRelations[i];

            return relation.CreateQuestion(seed, ExampleData.featureRelations);
        }

        private static bool AskAboutIdentity(Random seed)
        {
            var i = seed.Next(0, ExampleData.idrelations.Count);

            var relation = ExampleData.idrelations[i];

            return relation.CreateQuestion(seed, ExampleData.idrelations);
        }
    }
}
