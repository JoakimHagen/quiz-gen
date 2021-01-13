using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizGen
{
    class Program
    {
        static IEnumerable<Relation> allRelations;

        static void Main(string[] args)
        {
            var random = new Random();

            allRelations = ExampleData.relations.Concat(ExampleData.distractions);

            while (true)
            {
                Relation relation = null;
                int cardinality = 0;

                while (cardinality < 2)
                {
                    var i = random.Next(0, ExampleData.relations.Count);

                    relation = ExampleData.relations[i];

                    cardinality = ExampleData.relations.Count(x => x.property == relation.property && x.item2 == relation.item2);
                    cardinality += ExampleData.distractions.Count(x => x.property == relation.property && x.item2 == relation.item2);
                }

                var ok = random.Next(0,2) > 0
                    ? AskWhatParent(random, relation)
                    : AskWhichChildren(random, relation);

                if (ok) Console.ReadLine();
            }

        }

        private static bool AskWhatParent(Random random, Relation relation)
        {
            var correct = ExampleData.relations
                .Where(x => x.property == "is" && x.item1 == relation.item1)
                .Select(x => x.item2)
                .Distinct()
                .ToList();

            var distractions = ExampleData.distractions
                .Where(x => x.property == "is not" && x.item1 == relation.item1)
                .Select(x => x.item2)
                .ToList();

            distractions = distractions
                .Concat(Siblings(relation.item2, relation.property))
                .Where(x => !correct.Contains(x))
                .ToList();

            if (distractions.Count == 0)
            {
                return false;
            }

            var answerPool = new List<string>();

            answerPool.Add(correct[random.Next(0, correct.Count)]);

            distractions.Shuffle(random);

            foreach (var answer in distractions.Take(5))
            {
                answerPool.Add(answer);
            }

            Console.WriteLine("Q: " + relation.item1 + " is which of the following?");

            answerPool.Shuffle(random);

            foreach (var answer in answerPool)
            {
                Console.WriteLine(" - A " + answer);
            }

            Console.WriteLine("\nA: " + String.Join(", ", correct.Where(x => answerPool.Contains(x))));

            return true;
        }

        private static bool AskWhichChildren(Random random, Relation relation)
        {
            var correct = ExampleData.relations
                .Where(x => x.property == "is" && x.item2 == relation.item2)
                .Select(x => x.item1)
                .Distinct()
                .ToList();

            var distractions = ExampleData.distractions
                .Where(x => x.property == "is not" && x.item2 == relation.item2)
                .Select(x => x.item1)
                .ToList();

            foreach (var sib in Siblings(relation.item2, relation.property))
            {
                distractions = distractions.Concat(Children(sib, relation.property)).ToList();
            }

            if (distractions.Count == 0)
            {
                return false;
            }

            distractions = distractions
                .Distinct()
                .Where(x => !correct.Contains(x))
                .ToList();

            var answerPool = new List<string>();

            answerPool.Add(correct[random.Next(0, correct.Count)]);

            distractions.Shuffle(random);

            foreach (var answer in distractions.Take(5))
            {
                answerPool.Add(answer);
            }

            Console.WriteLine("Q: Which of the following is a " + relation.item2 + "?");

            answerPool.Shuffle(random);

            foreach (var answer in answerPool)
            {
                Console.WriteLine(" - " + answer);
            }

            Console.WriteLine("\nA: " + String.Join(", ", correct.Where(x => answerPool.Contains(x))));

            return true;
        }

        static IEnumerable<string> Parents(string item, string property)
        {
            return allRelations.Where(x => x.property == property
                && x.item1 == item)
                .Select(x => x.item2)
                .Distinct();
        }

        static IEnumerable<string> Children(string item, string property)
        {
            return allRelations.Where(x => x.property == property
                && x.item2 == item)
                .Select(x => x.item1)
                .Distinct();
        }

        static IEnumerable<string> Siblings(string item, string property)
        {
            var parents = Parents(item, property);

            return allRelations.Where(x => x.property == property
                && parents.Contains(x.item2))
                .Select(x => x.item1)
                .Distinct();
        }
    }
}
