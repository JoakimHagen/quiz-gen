using System;
using System.Linq;

namespace QuizGen
{
    public class Question
    {
        public string Stem;
        public string[] Answers;
        public string[] Distractors;

        public void PrintToConsole(Random seed)
        {
            Console.WriteLine("Q: " + Stem);

            var options = Answers.Concat(Distractors).ToArray();

            options.Shuffle(seed);

            foreach (var answer in options)
            {
                Console.WriteLine(" - " + answer);
            }
        }
    }
}
