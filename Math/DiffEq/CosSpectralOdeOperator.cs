using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mathlib.Polynomials;
using static System.Linq.Enumerable;

namespace mathlib.DiffEq
{
    public class CosSpectralOdeOperator: ISpectralOdeOperator<double[][]>
    {
        private DynFunc<double>[] _f;
        private double[] _initialValues;
        private int _partialSumOrder; // N
        private int _m;

        private readonly IEnumerable<Func<double, double>> _phi;
        private readonly IEnumerable<Func<double, double>> _phiSobolev;

        private Func<double, double>[] _phiCached;
        private Func<double, double>[] _phiSobolevCached;
        /// <summary>
        /// Nodes that are used in quadrature formula of calculating integral
        /// </summary>
        private readonly double[] _nodes;


        //public Segment OrthogonalitySegment => new Segment(0, 1);
        private Segment _OrthogonalitySegment = new Segment(0, 1);
        public Segment OrthogonalitySegment {
            get {
                return _OrthogonalitySegment;
            }
            set {
                _OrthogonalitySegment = value;
            }
        }

        public CosSpectralOdeOperator(IEnumerable<Func<double, double>> phi, IEnumerable<Func<double, double>> phiSobolev, double[] quadratureNodes)
        {
            _phi = phi;
            _phiSobolev = phiSobolev;
            _nodes = quadratureNodes;
            // TODO: check arguments consistence
        }

        public void SetParams(DynFunc<double>[] odeRightSides, double[] initialValues, int partialSumOrder)
        {
            _f = odeRightSides;
            _initialValues = initialValues;
            _partialSumOrder = partialSumOrder;
            _m = _f.First().ArgsCount - 1;
            _phiCached = _phi.Take(partialSumOrder).ToArray();
            _phiSobolevCached = _phiSobolev.Take(partialSumOrder).ToArray();
        }

        public double[][] GetValue(double[][] c)
        {
            if (c.Length != _m)
                throw new ArgumentOutOfRangeException($"Argument c should have first dimension length equal to {_m}");
            // eta[k][j] = $\eta_k(t_j)$
            var eta = Range(0, _m)
                        .Select(k => CalcEta(k, c[k])).ToArray();

            return CalcCoeffs(eta);
        }

        /// <summary>
        /// Calculates $\eta_j(t)=initialValues[j]+h\sum_{i=0}^N c[i] phiSobolev[i+1](t)$ in nodes.
        /// </summary>
        /// <param name="j"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private double[] CalcEta(int k, double[] c)
        {
            return _nodes
                .Select(t => _initialValues[k] + c.Zip(_phiSobolevCached.Skip(1), (ci, phiiPlus1) => ci * phiiPlus1(t)).Sum())
                .ToArray();
        }

        private double[][] CalcCoeffs(double[][] eta)
        {
            //var fk = _nodes.Select(t => _f[k].Invoke(_h * t, eta[k]))
            // fArgs[j][k] = $\eta_k(t_j)$ if k>0 and $h t_j$ if k=0, so
            // fArgs[j] = $(h t_j, \eta_0(t_j), ..., \eta_{m-1}(t_j))$.
            var fArgs = new double[_nodes.Length][];

            for (int j = 0; j < _nodes.Length; j++)
            {
                fArgs[j] = new double[_m+1];
                fArgs[j][0] = _nodes[j];
                for (int k = 1; k < _m + 1; k++)
                {
                    fArgs[j][k] = eta[k-1][j];
                }
            }

            return Range(0,_m)
                .Select(i => Range(0, _partialSumOrder)
                    .Select(k => CalcCoeff(i, k, fArgs)).ToArray()).ToArray();
        }

        /// <summary>
        /// Calculates $c_k(f_i)$
        /// </summary>
        /// <param name="i"></param>
        /// <param name="k"></param>
        /// <param name="fArgs"></param>
        /// <returns></returns>
        private double CalcCoeff(int i, int k, double[][] fArgs)
        {
            // qk[j] = $f_k(h t_j, \eta_0(t_j), ..., \eta_{m-1}(t_j)) * phi_k(t_j)$,
            // so qk is a vector of values of function $q_k(t)=f_k(ht,\eta_0(t),...) phi(t)$ in _nodes
            var qk = _nodes.Select((t, j) => _f[i].Invoke(fArgs[j])*_phiCached[k](t));
            return Integrals.Trapezoid(qk.ToArray(), _nodes);
        }

    }
}