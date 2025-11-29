using System;
using System.Collections.Generic;
using System.Linq;
using DiscreteFunctions;

namespace mathlib
{
    public class FourierDiscretePartialSum
    {
        private readonly double[] _nodes;
        private readonly Func<double, double>[] _basisFunctions;

        public FourierDiscretePartialSum(double[] nodes, Func<double, double>[] basisFunctions)
        {
            _nodes = nodes;
            _basisFunctions = basisFunctions;
        }

        public DiscreteFunction2D GetValues(params double[] coeffs)
        {
            var values = _nodes
                .Select(t => coeffs.Zip(_basisFunctions, (ci, phii) => ci * phii(t)).Sum())
                .ToArray();
            return new DiscreteFunction2D(_nodes, values);
        }

        public DiscreteFunction2D GetValues(IEnumerable<double> coeffs)
        {
            var values = _nodes
                .Select(t => _basisFunctions.Zip(coeffs, (phii, ci) => ci * phii(t)).Sum())
                .ToArray();
            return new DiscreteFunction2D(_nodes, values);
        }
    }
}