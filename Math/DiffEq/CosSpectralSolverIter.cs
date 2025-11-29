using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DiscreteFunctions;
using Endless;
using static System.Linq.Enumerable;

namespace mathlib.DiffEq
{
    public class CosSpectralSolverIter: SpectralSolverIterBase
    {
        public CosSpectralSolverIter(int quadratureNodesCount):
            base(CreateOperator(quadratureNodesCount), CreateIft())
        {
        }

        private static ISpectralOdeOperator<double[][]> CreateOperator(int quadratureNodesCount)
        {
            // Used to calculate coeffs that are represented as integrals
            var quadratureNodes = new Segment(0, 1).GetUniformPartition(quadratureNodesCount);
            var cosSystem = new CosSystem();
            var sobCosSystem = new SobolevCosSystem();
            var op = new CosSpectralOdeOperator(Natural.NumbersWithZero.Select(k => cosSystem.Get(k)),
                Natural.NumbersWithZero.Select(k => sobCosSystem.Get(k)), quadratureNodes);
            return op;
        }

        private static IInvFourierTransformer CreateIft()
        {
            return new InvFourierTransformer((coeffs, nodes) =>
            {
                var sobCosSystem = new SobolevCosSystem();
                var sobolevPartSum = new FourierDiscretePartialSum(nodes, 
                    Natural.NumbersWithZero.Select(k => sobCosSystem.Get(k)).Take(coeffs.Length).ToArray());
                return sobolevPartSum.GetValues(coeffs).Y;
            });
        }

        //public DiscreteFunction2D[] Solve(double[] nodes, double h, int partialSumOrder, int iterCount)
        //{

        //}


        //protected override ISpectralOdeOperator<double[][]> GetCoeffsOperator()
        //{
        //    throw new NotImplementedException();
        //    var phiSobolev = _phiSobolev.Take(coeffsCount + 1).ToArray();
        //    var op = new CosSpectralOdeOperator(f, initialValues, nodes,
        //        _phi.Take(partialSumOrder + 1).ToArray(), phiSobolev, partialSumOrder);
            
            
        //}

        
        
    }
}