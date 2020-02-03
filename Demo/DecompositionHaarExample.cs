using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mathlib;

namespace Demo
{
    public static class DecompositionHaarExample
    {
        static int n = 11;
        static int m = 1 << 10;

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

        static double F(double x)
        {
            return Math.Sin(x * Math.PI);
        }
    }
}
