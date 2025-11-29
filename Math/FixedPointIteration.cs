using System;

namespace mathlib
{
    public class FixedPointIteration
    {
        public static T FindFixedPoint<T>(Func<T, T> f, T initialValue, int iterCount, Action<T> observer = null)
        {
            var x = initialValue;
            for (int i = 0; i < iterCount; i++)
            {
                observer?.Invoke(x);
                x = f(x);
            }
            return x;
        }

        public static T FindFixedPoint<T>(Func<T, T> f, T initialValue, double eps, Func<T, T, double> metric, int maxIter)
        {
            var x = initialValue;
            var xNext = f(x);
            var counter = 0;
            while (counter <= maxIter && metric(x, xNext) > eps)
            {
                x = xNext;
                xNext = f(x);
            }

            return xNext;
        }
    }
}