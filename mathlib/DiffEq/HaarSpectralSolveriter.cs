using System.Linq;
using Endless;

namespace mathlib.DiffEq
{
    public class HaarSpectralSolverIter : SpectralSolverIterBase
    {
        public HaarSpectralSolverIter(int quadratureNodesCount) :
            base(CreateOperator(quadratureNodesCount), CreateIft())
        { }

        private static ISpectralOdeOperator<double[][]> CreateOperator(int quadratureNodesCount)
        {
            // Used to calculate coeffs that are represented as integrals
            var quadratureNodes = new Segment(0, 1).GetUniformPartition(quadratureNodesCount);
            var op = new HaarSpectralOdeOperator(quadratureNodes);
            return op;
        }

        private static IInvFourierTransformer CreateIft()
        {
            return new InvFourierTransformer((coeffs, nodes) =>
            {
                var sobHaarSystem = new SobolevHaarSystem();
                var sobolevPartSum = new FourierDiscretePartialSum(nodes,
                    Natural.NumbersWithZero.Select(k => sobHaarSystem.Get(k)).Take(coeffs.Length).ToArray());
                return sobolevPartSum.GetValues(coeffs).Y;
            });
        }
    }
}
