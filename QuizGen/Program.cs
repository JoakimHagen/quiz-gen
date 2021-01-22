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
                var qq = new NamedRelationQuestions(knowledge);

                var question = qq.Question(random);

                if (question != null)
                {
                    question.PrintToConsole(random);
                    Console.ReadLine();

                    Console.WriteLine($"A: {String.Join(", ", question.Answers)}\n\n");
                }
            }
        }
    }
}
