using System;
using System.Collections.Generic;
using System.Linq;

namespace mathlib.Polynomials
{
    public class GenPolynom
    {
        private readonly Func<double, double>[] _functions;
        
        public GenPolynom(Func<double, double>[] functions)
        {
            _functions = functions;
        }

        public double GetValue(double[] coeffs, double x)
        {
            if (coeffs.Length > _functions.Length)
                throw new ArgumentOutOfRangeException("Coeffs count should not be greater than polynom order");
            var functionsInX = Enumerable.Range(0, coeffs.Length).Select(k => _functions[k](x));
            return coeffs.Zip(functionsInX, (c, t) => c * t).Sum();
        }
    }
}