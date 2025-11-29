using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace mathlib
{
    public static class MixHaar
    {
        public static double Tcheb(int k, double x)
        {
            if (k == 0) return 1;
            if (k == 1) return x;

            return 2 * x * Tcheb(k - 1, x) - Tcheb(k - 2, x);
            //return Cos(k * Acos(x));
        }



        public static double MixTcheb(int k, double x)
        {
            if (k < 3) return 0;

            var v = Tcheb(k + 2, x) / (4 * (k + 1) * (k + 2));
            v -= Tcheb(k, x) / (2 * (k * k - 1));
            v += Tcheb(k - 2, x) / (4 * (k - 1) * (k - 2));
            v -= Pow(-1, k) * ((1 + x) / (Pow(k, 2) - 1) + 3.0 / ((k + 2) * (k + 1) * (k - 2) * (k - 1)));
            return v;
        }

        /*public static double MixTcheb(int k, double x)
        {
            if (k < 3) return 0;

            var v = Tcheb(k + 2, x) / (4 * (k + 1) * (k + 2));
            v -= Tcheb(k, x) / (2 * (k * k - 1));
            v += Tcheb(k - 2, x) / (4 * (k - 1) * (k - 2));
            v -= Pow(-1, k) * ((1 + x) / (Pow(k, 2) - 1) + 3.0 / ((k + 2) * (k + 1) * k * (k - 1)));
            return v;
        }*/

        /*public static double MixTcheb(int k, double x)
        {
            if (k < 3) return 0;
            int sgn;

            if (k % 2 == 0) { sgn = 1; }
            else { sgn = -1; }

            var v = Tcheb(k + 2, x) / (4 * (k + 1) * (k + 2));
            v -= Tcheb(k, x) / (2 * (k * k - 1));
            v += Tcheb(k - 2, x) / (4 * (k - 1) * (k - 2));
            v -= sgn *
                 (
                     (1 + x)/(k*k - 1) + 3.0/((k*k - 4)*(k*k - 1))
                 );
            return v;
        }*/


        public static double[] uniformGrid(double a, double b, int N)
        {
            double[] grid = new double[N + 1];
            double h = (b - a) / N;
            grid[0] = a;
            for (int i = 1; i <= N; i++)
            {
                grid[i] = grid[i - 1] + h;
            }
            return grid;
        }

        public static double[][] Tchebs(int n, double[] x)
        {
            int N = x.Length;
            double[][] T = new double[n + 1][];
            for (int i = 0; i <= n; i++)
            {
                T[i] = new double[N];
            }

            for (int i = 0; i < N; i++) { T[0][i] = 1.0; }
            if (n == 0) return T;
            for (int i = 0; i < N; i++) { T[1][i] = x[i]; }
            if (n == 1) return T;

            for (int k = 2; k <= n; k++)
            {
                for (int i = 0; i < N; i++)
                {
                    T[k][i] = 2 * x[i] * T[k - 1][i] - T[k - 2][i];
                }
            }

            return T;
        }


        public static double[] MixTcheb(int k, int N = 200)
        {
            double[] x = uniformGrid(-1, 1, N);
            double[][] T = Tchebs(k + 2, x);
            double[] v = new double[N + 1];

            if (k < 3)
            {
                for (int i = 0; i <= N; i++) { v[i] = 0.0; }
                return v;
            }

            int sgn;

            if (k % 2 == 0) { sgn = 1; }
            else { sgn = -1; }

            for (int i = 0; i <= N; i++)
            {
                v[i] = T[k + 2][i] / (4.0 * (k + 1) * (k + 2));
                v[i] -= T[k][i] / (2.0 * (k * k - 1));
                v[i] += T[k - 2][i] / (4.0 * (k - 1) * (k - 2));
                v[i] -= sgn *
                     (
                         (1 + x[i]) / (k * k - 1) + 3.0 / ((k * k - 4.0) * (k * k - 1.0))
                     );
            }
            return v;
        }




        // n >= 1
        public static Func<double, double> Haar(int n)
        {
            if (n == 1) return x => 1;
            Func<double, double> log2 = t => Log(t, 2);
            // Find k and i in representation n=2^k + i
            var k = Floor(log2(n - 1));
            var i = n - Pow(2, k);

            return x =>
            {
                if (x < (i - 1) / Pow(2, k)) return 0;
                if (x < (2 * i - 1) / Pow(2, k + 1)) return Pow(2, k / 2);
                if (x < i / Pow(2, k)) return -Pow(2, k / 2);
                return 0;
            };
        }


        /// <summary>
        ///  n >= 1, r >= 1. 
        /// Common definition of sobolev function by integrals is used.  
        /// And to calc integrals rectangular formula is used .
        /// </summary>
        /// <param name="r"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Func<double, double> MixedHaarByDef(int r, int n)
        {
            if (n <= r)
            {
                var fact = Factorial(n - 1);
                return x => Pow(x, n - 1) / fact;
            }

            var c = 1 / Factorial(r - 1);
            var haarFunc = Haar(n - r);
            return x => c * Integrals.Rectangular(t => Pow(x - t, r - 1) * haarFunc(t), 0, x, 10000, Integrals.RectType.Center);
        }

        public static int Factorial(int n)
        {
            var fact = 1;
            for (int i = 2; i <= n; i++)
            {
                fact *= i;
            }
            return fact;
        }

        /// <summary>
        /// Used formulas from paper http://www.mathnet.ru/php/archive.phtml?wshow=paper&jrnid=isu&paperid=34&option_lang=rus
        /// </summary>
        /// <param name="r"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Func<double, double> MixedHaar(int r, int n)
        {
            if (n <= r + 1)
            {
                var fact = Factorial(n - 1);
                return x => Pow(x, n - 1) / fact;
            }

            n = n - r;

            int nCopy = n - 1;
            int k = 0;
            while (nCopy != 1)
            {
                k++;
                nCopy >>= 1;
            }
            int i = n - (1 << k);

            var pow2k = Pow(2, k);
            var pow2k2 = Pow(2, k / 2.0);
            var rFact = Factorial(r);

            return x =>
            {
                var v = 0.0;
                var t = (i - 1) / pow2k;
                if (x >= t) v += Pow(x - t, r);
                t = (2 * i - 1) / (2 * pow2k);
                if (x >= t) v -= 2 * Pow(x - t, r);
                t = i / pow2k;
                if (x >= t) v += Pow(x - t, r);
                return v * pow2k2 / rFact;
            };
        }

        public static Func<double, double> MixedHaar1(int n)
        {
            if (n <= 2)
            {
                return x => Pow(x, n - 1);
            }

            n--;
            
            int nCopy = n - 1;
            int k = 0;
            while (nCopy != 1)
            {
                k++;
                nCopy >>= 1;
            }
            int i = n - (1 << k);

            return MixedHaar1(k, i);
        }

        public static Func<double, double> MixedHaar1(double k, double i)
        {
            var pow2k = Pow(2, k);
            var pow2k2 = Pow(2, k / 2);

            return x =>
            {
                var v = 0.0;
                var t = (i - 1) / pow2k;
                if (x >= t) v += x - t;
                t = (2 * i - 1) / (2 * pow2k);
                if (x >= t) v -= 2 * (x - t);
                t = i / pow2k;
                if (x >= t) v += x - t;
                return v * pow2k2;
            };
        }
    }
}
