using System;

namespace mathlib.Symbolic
{
    public class Integral
    {
        public double LowerBound { get; protected set; }
        public double UpperBound { get; protected set; }
        public Func<double, double> Function { get; protected set; }

        public Integral(Func<double, double> function, double lowerBound, double upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Function = function;
        }
    }
}