using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using mathlib;

namespace Demo
{
    public static class DecompositionHaarExample
    {
        static readonly int n = 11;
        static readonly int m = 1 << 10;

        public static void TestingForward()
        {
            double[] d = new double[m];
            d = SobolevHaarLinearCombination.Decomposition(F, m);

            double[] x = new double[n];
            double[] f = new double[n];
            double[] s = new double[n];
            for (int i = 0; i < n; i++)
            {
                x[i] = i / (n - 1.0);
                f[i] = F(x[i]);
                for (int j = 0; j < m; j++)
                {
                    s[i] += d[j] * MixHaar.Haar(j + 1)(x[i]);
                }
                Console.WriteLine("f({0}) = {1};\ts({0}) = {2};", x[i], f[i], s[i]);
            }
        }

        public static void DSD()
        {
            double[] d = new double[m];
            double[] sd = new double[m];
            d = SobolevHaarLinearCombination.Decomposition(F, m);
            sd = SlowDecomposition(F, m);

            for (int i = 0; i < m; i++)
            {
                //Console.WriteLine("i = {0};\td({0}) = {1};\tsd({0}) = {2};", i + 1, d[i], sd[i]);
                Console.WriteLine("i = {0};\td({0}) - sd({0}) = {1};", i + 1, d[i] - sd[i]);
            }
        }

        public static void TimeComparsion()
        {
            double[] d = new double[m];
            double[] d1 = new double[m];
            Stopwatch stopwatch = new Stopwatch();
            double[] averageTime = new double[2];
            int exp = 1000;
            stopwatch.Start();
            for (int i = 0; i < exp; i++)
            {
                d = SobolevHaarLinearCombination.Decomposition(F, m);
            }
            averageTime[0] = stopwatch.Elapsed.TotalMilliseconds / exp;
            stopwatch.Restart();
            for (int i = 0; i < exp; i++)
            {
                d1 = SlowDecomposition(F, m);
            }
            averageTime[1] = stopwatch.Elapsed.TotalMilliseconds / exp;

            Console.WriteLine("t(d) = {0} ms;\nt(ds) = {1} ms;\nds/d = {2}", averageTime[0], averageTime[1], averageTime[1] / averageTime[0]);
        }

        private static double[] SlowDecomposition(Func<double, double> func, int n)
        {
            double[] result = new double[n];

            result[0] = Integrals.Rectangular(func, 0, 1, n + 1, Integrals.RectType.Center);
            for (int i = 1; i < n; i++)
            {
                var (k, j) = Common.Decompose(i + 1);

                double a = (j - 1.0) / Math.Pow(2, k);
                double b = (2 * j - 1.0) / Math.Pow(2, k + 1);
                double c = j / Math.Pow(2, k);

                double integralPos = Integrals.Rectangular(func, a, b, n / i + 1, Integrals.RectType.Center);
                double integralNeg = Integrals.Rectangular(func, b, c, n / i + 1, Integrals.RectType.Center);

                result[i] = (integralPos - integralNeg) * Math.Pow(2, k / 2.0);
            }
            return result;
        }

        static double F(double x)
        {
            return Math.Sin(x * Math.PI);
        }
    }
}
