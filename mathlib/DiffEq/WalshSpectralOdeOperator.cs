using System;
using System.Linq;
using static System.Linq.Enumerable;

namespace mathlib.DiffEq
{
    public class WalshSpectralOdeOperator : ISpectralOdeOperator<double[][]>
    {
        private DynFunc<double>[] _f;
        private double[] _initialValues;
        private int _partialSumOrder; // N
        private int _m;

        /// <summary>
        /// Nodes that are used in quadrature formula of calculating integral
        /// </summary>
        private readonly double[] _nodes;

        public Segment OrthogonalitySegment { get; set; } = new Segment(0, 1);

        public WalshSpectralOdeOperator(double[] quadratureNodes)
        {
            _nodes = quadratureNodes;
        }

        public void SetParams(DynFunc<double>[] odeRightSides, double[] initialValues, int partialSumOrder)
        {
            _f = odeRightSides;
            _initialValues = initialValues;
            _partialSumOrder = partialSumOrder;
            _m = _f.First().ArgsCount - 1;
        }

        public double[][] GetValue(double[][] c)
        {
            if (c.Length != _m)
                throw new ArgumentOutOfRangeException($"Argument c should have first dimension length equal to {_m}");
            var eta = Range(0, _m)
                        .Select(k => CalcEta(k, c[k])).ToArray();
            return CalcCoeffs(eta);
        }

        private double[] CalcEta(int k, double[] c)
        {
            return _nodes
               .Select(t => _initialValues[k] + SobolevHaarLinearCombination.FastCalc(c, t))
               .ToArray();
        }

        private double[][] CalcCoeffs(double[][] eta)
        {
            var fArgs = new double[_nodes.Length][];

            for (int j = 0; j < _nodes.Length; j++)
            {
                fArgs[j] = new double[_m + 1];
                fArgs[j][0] = _nodes[j];
                for (int k = 1; k < _m + 1; k++)
                {
                    fArgs[j][k] = eta[k - 1][j];
                }
            }

            return Range(0, _m)
                .Select(i =>
                {
                    var qk = _nodes.Select((t, j) => _f[i].Invoke(fArgs[j])).ToArray();
                    double func(double x)
                    {
                        return FindNode(x, qk);
                    }
                    return SobolevHaarLinearCombination.Decomposition(func, _partialSumOrder);
                }).ToArray();
        }

        private double FindNode(double x, double[] array)
        {
            if (_nodes[0] == x)
            {
                return array[0];
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (_nodes[i] > x)
                {
                    return (array[i - 1] + array[i]) / 2.0;
                }
                if (_nodes[i] == x)
                {
                    return array[i];
                }
            }
            return 0;
        }
    }
}
