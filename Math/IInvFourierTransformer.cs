using System;

namespace mathlib
{
    public interface IInvFourierTransformer
    {
        double[] Transform(double[] coeffs, double[] nodes);
    }

    public class InvFourierTransformer: IInvFourierTransformer
    {
        private readonly Func<double[], double[], double[]> _transform;

        public InvFourierTransformer(Func<double[], double[], double[]> transform)
        {
            _transform = transform;
        }

        public double[] Transform(double[] coeffs, double[] nodes) =>
            _transform(coeffs, nodes);

    }
}