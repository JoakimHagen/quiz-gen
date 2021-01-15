using System;
using System.Linq;

namespace QuizGen
{
    public class Question
    {
        public string Stem;
        public string[] Correct;
        public string[] Distraction;

        public void PrintToConsole(Random seed)
        {
            Console.WriteLine("Q: " + Stem);

            var answerPool = Correct.Concat(Distraction).ToArray();

            answerPool.Shuffle(seed);

            foreach (var answer in answerPool)
            {
                Console.WriteLine(" - " + answer);
            }
        }
    }
}
