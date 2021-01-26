using System;
using System.Collections.Generic;

namespace QuizGen
{
    class Program
    {
        static readonly Knowledge knowledge = new ExampleData();

        static void Main(string[] args)
        {
            var random = new Random();

            var alreadySeen = new List<int>();

            while (true)
            {
                var qq = new NamedRelationQuestions(knowledge);

                var question = qq.Question(random);

                if (question != null)
                {
                    var hash = question.GetHashCode();
                    if (alreadySeen.Contains(hash))
                        continue;
                    else
                        alreadySeen.Add(hash);

                    question.PrintToConsole(random);
                    Console.ReadLine();

                    Console.WriteLine($"A: {String.Join(", ", question.Answers)}\n\n");
                }
            }
        }
    }
}
