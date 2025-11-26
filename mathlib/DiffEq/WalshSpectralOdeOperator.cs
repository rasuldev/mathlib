using mathlib.Functions;
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
        private readonly int _quadratureNodesCount;

        /// <summary>
        /// Nodes that are used in quadrature formula of calculating integral
        /// </summary>
        //private readonly double[] _nodes;

        public Segment OrthogonalitySegment { get; set; } = new Segment(0, 1);

        public WalshSpectralOdeOperator(int quadratureNodesCount)
        {
            _quadratureNodesCount = quadratureNodesCount;
            //_nodes = quadratureNodes;
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
            return CalcCoeffs(c);
        }

        private Func<double,double>[] Etas(double[][] c)
        {
            var s = new Func<double>(() => 1);
            return Range(0, _m)
               .Select(i => new Func<double, double>(t => 
                                _initialValues[i] + SobolevWalshLinearCombination.Calc(c[i], t)))
               .ToArray();
        }

        private double[][] CalcCoeffs(double[][] c)
        {
            var etas = Etas(c);
            return Range(0, _m)
                .Select(i => Range(0, _partialSumOrder)
                    .Select(k => CalcCoeff(i, k, t => _f[i].Invoke(etas.Select(e => e(t)).Prepend(t).ToArray())))
                    .ToArray())
                .ToArray();
        }

        /// <summary>
        /// Calculates $c_k(f_i)$
        /// </summary>
        /// <param name="i"></param>
        /// <param name="k"></param>
        /// <param name="fArgs"></param>
        /// <returns></returns>
        private double CalcCoeff(int i, int k, Func<double,double> q)
        {
            // qk[j] = $f_i(t_j, \eta_0(t_j), ..., \eta_{m-1}(t_j)) * phi_k(t_j)$,
            // so qk is a vector of values of function $q_k(t)=f_i(ht,\eta_0(t),...) phi_k(t)$ in _nodes
            //var qk = _nodes.Select((t, j) => _f[i].Invoke(fArgs[j]) * _phiCached[k](t));
            //return Integrals.Trapezoid(qk.ToArray(), _nodes);
            var j = Common.GetClosestPowerOf2(k);
            var pow2j = Math.Pow(2, j);
            // w_k is constant on an interval \delta_p = (p/2^j, (p+1)/2^j). 
            // We'll calculate integral on [0,1] as a sum of integrals on \delta_p
            var s = 0d;
            var wk = Walsh.Get(k);
            for (int p = 0; p < pow2j; p++)
            {
                var deltaP = (p / pow2j, (p + 1) / pow2j);
                var middleOfDeltaP = (deltaP.Item1 + deltaP.Item2) * 0.5;
                s += wk(middleOfDeltaP) * Integrals.Trapezoid(q, deltaP.Item1, deltaP.Item2, _quadratureNodesCount);
            }
            return s;
        }


    }
}
