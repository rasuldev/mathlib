using System;
using System.Linq;
using static System.Math;

namespace mathlib.Functions
{
    public class WalshSobolev
    {
        public static Func<double, double> Get(int n)
        {
            double One(double x) => 1;
            if (n == 0)
                return One;

            var nodesCount = (int)Math.Pow(2, (int)Math.Ceiling(Math.Log(n, 2)) + 6);
            var nodes = Enumerable.Range(0, nodesCount).Select(j => j * 1.0 / nodesCount);
            double Sobolev(double x)
            {
                return Integrals.Rectangular(Walsh.Get(n - 1), nodes.Where(node => node <= x).ToArray(),
                    Integrals.RectType.Center);
            }

            return Sobolev;
        }

        public static Func<double, double> Get(int r, int n)
        {
            if (n < r)
                return x => Pow(x, n) / Common.Factorial(n);

            var nodesCount = (int)Math.Pow(2, (int)Math.Ceiling(Math.Log(n, 2)) + 6);
            var nodes = Enumerable.Range(0, nodesCount).Select(j => j * 1.0 / nodesCount);
            var walsh_nMinusr = Walsh.Get(n - r);
            double Sobolev(double x)
            {
                return Integrals.Rectangular(t => Pow(x - t, r - 1) * walsh_nMinusr(t), nodes.Where(node => node <= x).ToArray(),
                    Integrals.RectType.Center);
            }
            return Sobolev;
        }

        public static Func<double, double> Get2(int n)
        {
            double One(double x) => 1;
            if (n == 0)
                return One;

            double X(double x) => x;
            if (n == 1)
                return X;

            n = n - 1;
            var pow2 = (int)Math.Pow(2, Math.Floor(Math.Log(n, 2)));
            double Sobolev(double x)
            {
                return Walsh.Get(n - pow2)(x) * W12(pow2 * x) / pow2;
            }

            return Sobolev;
        }

        static double W12(double x)
        {
            x = x - (int)x;
            return x < 0.5 ? x : 1 - x;
        }
    }
}