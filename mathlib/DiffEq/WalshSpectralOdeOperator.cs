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
               .Select(t => _initialValues[k] + SobolevWalshLinearCombination.Calc(c, t))
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
                .Select(i => Range(0, _partialSumOrder)
                    .Select(k => CalcCoeff(i, k, fArgs)).ToArray()).ToArray();
        }

        /// <summary>
        /// Calculates $c_k(f_j)$
        /// </summary>
        /// <param name="i"></param>
        /// <param name="k"></param>
        /// <param name="fArgs"></param>
        /// <returns></returns>
        private double CalcCoeff(int i, int k, double[][] fArgs)
        {
            // qk[j] = $f_k(h t_j, \eta_0(t_j), ..., \eta_{m-1}(t_j)) * phi_k(t_j)$,
            // so qk is a vector of values of function $q_k(t)=f_k(ht,\eta_0(t),...) phi(t)$ in _nodes
            var qk = _nodes.Select((t, j) => _f[i].Invoke(fArgs[j]) * _phiCached[k](t));
            return Integrals.Trapezoid(qk.ToArray(), _nodes);
        }


    }
}
