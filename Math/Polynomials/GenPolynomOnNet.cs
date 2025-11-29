using System;
using System.Linq;
using System.Threading.Tasks;

namespace mathlib.Polynomials
{
    public class GenPolynomOnNet
    {
        private readonly int _order;
        private readonly double[] _nodes;
        // _values[j][k] - value of k-th term of polynom at j-th node of net
        private readonly double[][] _values;

        public GenPolynomOnNet(IFunctionsSystem functionsSystem, int order, double[] nodes)
            : this(Enumerable.Range(0, order+1).Select(functionsSystem.Get).ToArray(), nodes)
        {
            //var _values = Enumerable.Range(0, order + 1).Select(k => nodes.Select(functionsSystem.Get(k)).ToArray()).ToArray();
        }

        public GenPolynomOnNet(Func<double, double>[] functions, double[] nodes)
        {
            _order = functions.Length - 1;
            _nodes = nodes;
            _values = new double[nodes.Length][];
            for (int j = 0; j < nodes.Length; j++)
            {
                _values[j] = new double[_order + 1];
                for (int k = 0; k <= _order; k++)
                {
                    _values[j][k] = functions[k](nodes[j]);
                }
            }
        }

        public double[] GetValuesOnNet(double[] coeffs)
        {
            if (coeffs.Length != _order+1)
                throw new ArgumentOutOfRangeException("Coeffs length should be equal to polynom order plus 1");

            var result = new double[_nodes.Length];
            Parallel.For(0, result.Length, j =>
            {
                result[j] = coeffs.AsParallel().Zip(_values[j].AsParallel(), (c, v) => c * v).Sum();
            });
            return result;
        }
    }
}