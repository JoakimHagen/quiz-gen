using System;
using System.Collections.Generic;

namespace QuizGen
{
    public static class RandomExtensions
    {
        public static T Choose<T>(this Random self, IList<T> items)
        {
            return items[self.Next(0, items.Count)];
        }

        public static T Choose<T>(this Random self, params T[] items) where T : struct
        {
            return items[self.Next(0, items.Length)];
        }
    }
}
