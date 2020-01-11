using Endless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathlib.DiffEq
{
    public class WalshSpectralSolverIter : SpectralSolverIterBase
    {
        public WalshSpectralSolverIter(int quadratureNodesCount) :
            base(CreateOperator(quadratureNodesCount), CreateIft())
        { }

        private static ISpectralOdeOperator<double[][]> CreateOperator(int quadratureNodesCount)
        {
            // Used to calculate coeffs that are represented as integrals
            //var quadratureNodes = new Segment(0, 1).GetUniformPartition(quadratureNodesCount);
            var op = new WalshSpectralOdeOperator(quadratureNodesCount);
            return op;
        }

        private static IInvFourierTransformer CreateIft()
        {
            return new InvFourierTransformer((coeffs, nodes) =>
            {
                var sobWalshSystem = new SobolevWalshSystem();
                var sobolevPartSum = new FourierDiscretePartialSum(nodes,
                    Natural.NumbersWithZero.Select(k => sobWalshSystem.Get(k)).Take(coeffs.Length).ToArray());
                return sobolevPartSum.GetValues(coeffs).Y;
            });
        }

    }
}
