using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DiscreteFunctions;
using static System.Linq.Enumerable;

namespace mathlib.DiffEq
{
    public abstract class SpectralSolverIterBase
    {
        private readonly ISpectralOdeOperator<double[][]> _spectralOdeOperator;
        private readonly IInvFourierTransformer _ift;

        protected SpectralSolverIterBase(ISpectralOdeOperator<double[][]> spectralOdeOperator,
            IInvFourierTransformer ift)
        {
            _spectralOdeOperator = spectralOdeOperator;
            _ift = ift;
        }

        private static double[][] GenerateInitialCoeffs(int equationsCount, int coeffsCount) =>
            Range(0, equationsCount).Select(j => Range(0, coeffsCount).Select(k => 1.0 / (k + 1)).ToArray()).ToArray();

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="odeRightSide">Array of functions f_i(x,y_1,y_2,...,y_m) that represents ODE system right side, i. e.
        /// y_i'(x)=f_i(x,y1,y2,...,ym), i=1,...,m.
        /// </param>
        /// <param name="initialValues">y_i(a), i=1,...,m</param>
        /// <param name="coeffsCount"></param>
        /// <param name="quadratureNodes">Some implementations may require nodes for calculating integrals</param>
        /// <returns></returns>
        //public void SetParams(DynFunc<double>[] odeRightSide, double[] initialValues,
        //    int coeffsCount, double[] quadratureNodes = null)
        //{
        //}

        //protected abstract ISpectralOdeOperator<double[][]> CreateSpectralOdeOperator();
        //protected abstract Segment GetOrthogonalitySegment();
        //protected abstract double[][] GetInitialCoeffs();
        //protected abstract double[] InvFourierSobolevTransform(double[] coeffs, double[] nodes);
        */

        /// <summary>
        /// Solves Cauchy problem on orthogonality segment, which is determined by orthogonal system used in GetCoeffsOperator().
        /// </summary>
        /// <param name="rightSides"></param>
        /// <param name="initialValues"></param>
        /// <param name="iterCount"></param>
        /// <param name="initialCoeffs">Initial point for iteration method. It size should be equationsCount x partialSumOrder</param>
        /// <param name="nodes">Calculated approximate solution is discretized on nodes. Nodes should be inside orthogonality segment</param>
        /// <returns>Approximate solution on nodes</returns>
        protected (IEnumerable<DiscreteFunction2D> solution, double[][] solutionCoeffs) SolveOnOrthogonalitySegment(DynFunc<double>[] rightSides, double[] initialValues, 
            int iterCount, double[][] initialCoeffs, double[] nodes)
        {
            if (initialCoeffs.Length != rightSides.Length)
                throw new ArgumentException("rightSides length should equal to initialCoeffs length");

            // \eta(a)+\sum_{j=0}^{partialSumOrder-1}d_j\phi_{1,1+j}(t)
            var partialSumOrder = initialCoeffs[0].Length;

            _spectralOdeOperator.SetParams(rightSides, initialValues, partialSumOrder);
            var result = FixedPointIteration.FindFixedPoint(c => _spectralOdeOperator.GetValue(c), initialCoeffs, iterCount);
            return (result.Select((coeffs, j) => 
                            new DiscreteFunction2D(
                                nodes,
                                _ift.Transform(new[] { initialValues[j] }.Concat(coeffs).ToArray(), nodes))),
                    result);
        }

        protected (IEnumerable<DiscreteFunction2D> solution, double[][] solutionCoeffs) SolveOnOrthogonalitySegment(DynFunc<double>[] rightSides, double[] initialValues, 
            int partialSumOrder, int iterCount, double[] nodes)
        {
            var initialCoeffs = GenerateInitialCoeffs(rightSides.Length, partialSumOrder);
            return SolveOnOrthogonalitySegment(rightSides, initialValues, iterCount, initialCoeffs, nodes);
        }
        
        /// <summary>
        /// Solves ODE system on segment
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="partialSumOrder"></param>
        /// <param name="iterCount"></param>
        /// <param name="nodes">Nodes should be inside problem segment</param>
        /// <returns></returns>
        public (IEnumerable<DiscreteFunction2D> solution, double[][] solutionCoeffs) SolveWithCoeffs(CauchyProblem problem, 
            int partialSumOrder, int iterCount, double[] nodes)
        {
            // To apply iter method we should linear transform problem segment to orthogonality segment.
            // Let's denote orthogonality segment by [a0,b0] and problem segment by [a,b]. 
            // We want to transform problem y'(x)=f(x,y(x)), x \in [a,b], y(a)=y0 to equivalent 
            // problem on [a0,b0]. For this purpose we use transform x = (t-a0)(b-a)/(b0-a0)+a:
            // z(t)=y(x)=y(t-a0)(b-a)/(b0-a0)+a). Let's rewrite original problem in terms of z(t) and t:
            // z'(t)=y'(x)*(b-a)/(b0-a0)=f((t-a0)(b-a)/(b0-a0)+a, z(t))*(b-a)/(b0-a0), z(a0)=y(a). 
            // Finally, with notation h=(b-a)/(b0-a0) we have:
            // z'(t)=h * f(h(t-a0)+a, z(t)), z(a0)=y(a). 
            // So to solve original problem on [a,b] we can solve new problem on [a0,b0] and then use inverse 
            // transform to get y(x) from z(t).
            var (a, b) = problem.Segment;
            var (a0, b0) = _spectralOdeOperator.OrthogonalitySegment;
            var h = problem.Segment.Length / _spectralOdeOperator.OrthogonalitySegment.Length;

            var transformedRightSides = problem.RightSides.Select(fj =>
                new DynFunc<double>(fj.ArgsCount, doubles =>
                {
                    var newDoubles = new double[doubles.Length];
                    Array.Copy(doubles, newDoubles, doubles.Length);
                    newDoubles[0] = h * (doubles[0] - a0) + a;
                    return h * fj.Invoke(newDoubles);
                })).ToArray();

            
            var result = SolveOnOrthogonalitySegment(transformedRightSides, problem.InitialValues, 
                partialSumOrder, iterCount, nodes.Select(x => (x - a) / h + a0).ToArray());

            // result.solution is defined on [a0,b0] and we should transform it to [a,b]
            return (result.solution.Select(df => new DiscreteFunction2D(df.X.Select(x => h * (x - a0) + a).ToArray(), df.Y)), 
                    result.solutionCoeffs);
        }

        public DiscreteFunction2D[] Solve(CauchyProblem problem, int partialSumOrder, int iterCount, double[] nodes) =>
            SolveWithCoeffs(problem, partialSumOrder, iterCount, nodes).solution.ToArray();


        public List<DiscreteFunction2D[]> Solve(CauchyProblem problem, int chunksCount, 
            int partialSumOrder, int iterCount, int chunkNodesCount)
        {
            var dfs = new List<DiscreteFunction2D[]>();
            var (a, b) = problem.Segment;
            var h = (b - a) / chunksCount;
            var initVals = new double[problem.InitialValues.Length];
            Array.Copy(problem.InitialValues, initVals, initVals.Length);
            for (int j = 0; j < chunksCount; j++)
            {
                var chunk = new Segment(a + j * h, a + (j + 1) * h);
                var chunkNodes = chunk.GetUniformPartition(chunkNodesCount);
                var problemOnChunk = new CauchyProblem(problem.RightSides, initVals, chunk);
                var dfsOnChunk = Solve(problemOnChunk, partialSumOrder, iterCount, chunkNodes);
                dfs.Add(dfsOnChunk);
                initVals = dfsOnChunk.Select(df => df.Y.Last()).ToArray();
            }
            return dfs;
        }
    }
}