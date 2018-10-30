using System;
using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;

namespace mathlib.DiffEq
{
    public class HaarSpectralOdeOperator : ISpectralOdeOperator<double[][]>
    {
        private DynFunc<double>[] _f;
        private double[] _initialValues;
        private int _partialSumOrder; // N
        private int _m;

        // ***** Not using in this class *****
        //private readonly IEnumerable<Func<double, double>> _phi;
        //private readonly IEnumerable<Func<double, double>> _phiSobolev;

        //private Func<double, double>[] _phiCached;
        //private Func<double, double>[] _phiSobolevCached;
        /// <summary>
        /// Nodes that are used in quadrature formula of calculating integral
        /// </summary>
        private readonly double[] _nodes;

        public Segment OrthogonalitySegment { get; set; } = new Segment(0, 1);

        public HaarSpectralOdeOperator(IEnumerable<Func<double, double>> phi, IEnumerable<Func<double, double>> phiSobolev, double[] quadratureNodes)
        {
            //_phi = phi;
            //_phiSobolev = phiSobolev;
            _nodes = quadratureNodes;
        }

        public void SetParams(DynFunc<double>[] odeRightSides, double[] initialValues, int partialSumOrder)
        {
            _f = odeRightSides;
            _initialValues = initialValues;
            _partialSumOrder = partialSumOrder;
            _m = _f.First().ArgsCount - 1;
            //_phiCached = _phi.Take(partialSumOrder).ToArray();
            //_phiSobolevCached = _phiSobolev.Take(partialSumOrder).ToArray();
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
               //.Select(t => _initialValues[k] + c.Zip(_phiSobolevCached.Skip(1), (ci, phiiPlus1) => ci * phiiPlus1(t)).Sum())
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

            //return Range(0, _m)
            //    .Select(i => Range(0, _partialSumOrder)
            //        .Select(k => CalcCoeff(i, k, fArgs)).ToArray()).ToArray();
            return Range(0, _m)
                .Select(i =>
                {
                    double func(double x)
                    {
                        var qk = _nodes.Select((t, j) => _f[i].Invoke(fArgs[j])).ToArray();
                        return qk[FindNode(x)];
                    }
                    return SobolevHaarLinearCombination.Decomposition(func, _partialSumOrder);
                }).ToArray();
        }

        private int FindNode(double x)
        {
            int result = 0;
            for (int i = 1; i < _nodes.Length; i++)
            {
                if (_nodes[i] > x)
                {
                    return i - 1;
                }
            }
            return result;
        }
    }
}
