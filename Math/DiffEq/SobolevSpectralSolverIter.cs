using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DiscreteFunctions;
using Endless;
using static System.Linq.Enumerable;


namespace mathlib.DiffEq
{
    public class SobolevSpectralSolverIter : SpectralSolverIterBase
    {
       
        public SobolevSpectralSolverIter(int quadratureNodesCount, FunctionsSystem phi, FunctionsSystem phiSobolev, bool weighted = false)
            : base(CreateOperator(quadratureNodesCount, phi, phiSobolev, weighted), CreateIft(phiSobolev))
        {
        }



        private static ISpectralOdeOperator<double[][]> CreateOperator(int quadratureNodesCount, 
                                                    FunctionsSystem phi, FunctionsSystem phiSobolev, bool weighted = false)
        {
            var OrthoSegment = phi.OrthogonalitySegment;
            
            // Used to calculate coeffs that are represented as integrals
            var quadratureNodes = OrthoSegment.GetUniformPartition(quadratureNodesCount);

            var op = new SobolevSpectralOdeOperator(quadratureNodes, phi, phiSobolev, weighted);

            return op;
        }



        private static IInvFourierTransformer CreateIft(FunctionsSystem phiSobolev)
        {
            return new InvFourierTransformer((coeffs, nodes) =>
            {
                var sobolevPartSum = new FourierDiscretePartialSum(nodes, 
                    Natural.NumbersWithZero.Select(k => phiSobolev.Get(k)).Take(coeffs.Length).ToArray());
                return sobolevPartSum.GetValues(coeffs).Y;

            });
        }


        
        
    }
}