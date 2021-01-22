using System;

namespace QuizGen
{
    public static class RandomExtensions
    {
        public static T Choose<T>(this Random self, params T[] items)
        {
            return items[self.Next(0, items.Length)];
        }

        public static Func<TR> Choose<TR>(this Random self, params Func<TR>[] funcs)
        {
            return funcs[self.Next(0, funcs.Length)];
        }

        public static Func<T1, TR> Choose<T1, TR>(this Random self, params Func<T1, TR>[] funcs)
        {
            return funcs[self.Next(0, funcs.Length)];
        }

        public static Func<T1, T2, TR> Choose<T1, T2, TR>(this Random self, params Func<T1, T2, TR>[] funcs)
        {
            return funcs[self.Next(0, funcs.Length)];
        }
    }
}
