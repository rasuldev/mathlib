using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mathlib
{
    static class ChebRobustCalc {

        //the number of combinations calculation
        public static int[][] Combinations(int n)
        {
            int[][] C = new int[n + 1][];
            for (int i = 0; i < n + 1; i++) { C[i] = new int[i + 1]; }
            for (int i = 0; i <= n; i++)
            {
                C[i][0] = 1; C[i][i] = 1;
                for (int j = 1; j < i; j++)
                {
                    C[i][j] = C[i - 1][j - 1] + C[i - 1][j];
                }
            }
            return C;
        }


        //=====================================================
        //================ Weights Calculation ================
        public static double[] ChebWeight(double alpha, double beta, double[] x)
        {
            double[] result = new double[x.Length];
            return result;
        }

        public static double[] ChebWeightFirstKind(int N)
        {
            double[] w = new double[N];
            w[0] = Math.PI;

            for (int i = 1; i < N; i++) { w[0] *= 1.0 - 0.5 / i; }

            for (int i = 0; i < N - 1; i++) { w[i + 1] = w[i] * (1.0 - 0.5 / (i + 1.0)) * (1.0 + 0.5 / (N - i - 1.5)); }

            return w;
        }

        public static double[] ChebWeightSecondKind(int N)
        {
            double[] w = new double[N];
            w[0] = Math.PI;
            for (int i = 1; i < N + 1; i++) { w[0] *= 1.0 - 0.5 / i; }
            w[0] = w[0] / N / (N + 1);
            for (int i = 0; i < N - 1; i++) { w[i + 1] = w[i] * (1.0 + 0.5 / (i + 1.0)) * (1.0 - 0.5 / (N - i - 0.5)); }

            return w;
        }


        public static double[] ChebWeightUltrasph(int N, int alpha)
        {
            double[] w = new double[N];
            double tmp = Math.Pow(4.0, alpha + 0.5) / (double)N;
            for (int x = 0; x < N; x++)
            {
                w[x] = tmp;
                for (int i = 1; i <= alpha; i++)
                {
                    w[x] = w[x] * ((double)x + i) / ((double)N + i) * ((double)N - x + i - 1.0) / ((double)N + alpha + i);
                }
            }


            return w;
        }



        //=====================================================
        //============= Polynomials Calculation ===============






        //========= CHEBYSHEV--LEGENDER (alpha=beta=0)   ======

        //Chebyshev--Legendre alpha=beta=0
        //RECURSIVE
        public static double[][] ChebLegendreRec(int n, int N)
        {
            //initing
            double[][] T = new double[n + 1][];
            for (int i = 0; i <= n; i++) { T[i] = new double[N]; }
            //first 2 polies
            double N1 = N - 1;
            double tmp1 = Math.Sqrt(0.5);
            double tmp2 = Math.Sqrt(1.5 / (N * N - 1.0));
            for (int i = 0; i < N; i++)
            {
                T[0][i] = tmp1;
                T[1][i] = tmp2 * (2 * i - N1);
            }
            //rest n-2 polies
            double Xk, Yk;
            for (int k = 2; k <= n; k++)
            {
                //coeffs
                Xk = Math.Sqrt((2.0 * k - 1.0) / ((double)N - k) * (2.0 * k + 1.0) / ((double)N + k)) / ((double)k);
                Yk = (k - 1.0) * Math.Sqrt((2.0 * k + 1.0) / (2.0 * k - 3.0) * (N - k + 1.0) / ((double)N - k) * (N + k - 1.0) / ((double)N + k)) / ((double)k);
                //values
                for (int j = 0; j < N; j++)
                {
                    T[k][j] = (2 * j - N1) * Xk * T[k - 1][j] - Yk * T[k - 2][j];
                }
            }

            return T;
        }


        //Chebyshev--Legendre alpha=beta=0 with  Fix on Ends
        //(rest part filled with zeros)
        public static double[][] ChebLegendreRecFromCenter(int n, int N, out string output, double eps = 1E-14)
        {
            output = "";
            //initing
            double[][] T = new double[n + 1][];
            for (int i = 0; i <= n; i++) { T[i] = new double[N]; }
            //first 2 polies
            double N1 = N - 1;
            double tmp1 = Math.Sqrt(0.5);
            double tmp2 = Math.Sqrt(1.5 / (N * N - 1.0));
            for (int i = 0; i < N; i++)
            {
                T[0][i] = tmp1;
                T[1][i] = tmp2 * (2 * i - N1);
            }


            //rest n-2 polies
            double Xk, Yk, pval;
            int threshold; int N2 = N / 2;
            double eps1 = 0.25 * N * (1.0 + eps);
            int sgn = -1;

            //if even N
            if (N % 2 == 0)
            {
                for (int k = 2; k <= n; k++)
                {
                    //coeffs
                    Xk = Math.Sqrt((2.0 * k - 1.0) / ((double)N - k) * (2.0 * k + 1.0) / ((double)N + k)) / ((double)k);
                    Yk = (k - 1.0) * Math.Sqrt((2.0 * k + 1.0) / (2.0 * k - 3.0) * (N - k + 1.0) /
                                                       ((double)N - k) * (N + k - 1.0) / ((double)N + k)) / ((double)k);
                    threshold = -1; sgn = sgn * (-1);
                    pval = 0.0;
                    //values
                    for (int j = N2 - 1; j >= 0; j--)
                    {
                        T[k][j] = (2 * j - N1) * Xk * T[k - 1][j] - Yk * T[k - 2][j];
                        T[k][N - 1 - j] = sgn * T[k][j];
                        pval += T[k][j] * T[k][j];
                        if (pval > eps1)
                        {
                            threshold = j - 1;
                            break;
                        }
                    }
                    for (int j = threshold; j >= 0; j--) { T[k][j] = 0.0; }
                    output = "Threshold: " + threshold.ToString() + "||  ";
                    for (int x = 0; x <= threshold; x++)
                    {
                        output += "T[" + n.ToString() + "][" + x.ToString() + "] = " + T[n][x] + ";  ";
                    }
                }
            }
            else
            { //if odd N
                for (int k = 2; k <= n; k++)
                {
                    //coeffs
                    Xk = Math.Sqrt((2.0 * k - 1.0) / ((double)N - k) * (2.0 * k + 1.0) / ((double)N + k)) / ((double)k);
                    Yk = (k - 1.0) * Math.Sqrt((2.0 * k + 1.0) / (2.0 * k - 3.0) * (N - k + 1.0) / ((double)N - k) * (N + k - 1.0) / ((double)N + k)) / ((double)k);
                    threshold = -1; sgn = sgn * (-1);
                    //nod [(N-1)/2]
                    T[k][N2] = Yk * T[k - 2][N2];
                    pval = 0.5 * T[k][N2];
                    //values
                    for (int j = N2 - 1; j >= 0; j--)
                    {
                        T[k][j] = (2 * j - N1) * Xk * T[k - 1][j] - Yk * T[k - 2][j];
                        T[k][N - 1 - j] = sgn * T[k][j];
                        pval += T[k][j] * T[k][j];
                        if (pval > eps1)
                        {
                            threshold = j - 1;
                            break;
                        }
                    }
                    for (int j = threshold; j >= 0; j--) { T[k][j] = 0.0; }
                    output = "Threshold: " + threshold.ToString() + "||  ";
                    for (int x = 0; x <= threshold; x++)
                    {
                        output += "T[" + n.ToString() + "][" + x.ToString() + "] = " + T[n][x] + ";  ";
                    }
                }
            }

            return T;
        }



        //Chebyshev--Legendre alpha=beta=0 with Fix on Ends
        //(rest part calculated by explicit formulae)
        public static double[][] ChebLegendreRecHyperGeo(int n, int N, out string output, double eps = 1E-14)
        {
            output = "";
            //Number of Combinations (for explicit formulaes)
            int[][] C = Combinations(2 * n + 1);

            //initing
            double[][] T = new double[n + 1][];
            for (int i = 0; i <= n; i++) { T[i] = new double[N]; }
            //first 2 polies
            double N1 = N - 1;
            double tmp1 = Math.Sqrt(0.5);
            double tmp2 = Math.Sqrt(1.5 / (N * N - 1.0));
            for (int i = 0; i < N; i++)
            {
                T[0][i] = tmp1;
                T[1][i] = tmp2 * (2 * i - N1);
            }

            //rest n-2 polies
            double Xk, Yk, pval, explC, explX, explX1;
            int threshold = -1;
            int N2 = N / 2;
            double eps1 = 0.25 * N * (1.0 + eps);
            int sgn = -1; int sgn1;

            //if even N
            if (N % 2 == 0)
            {
                for (int k = 2; k <= n; k++)
                {
                    //coeffs
                    Xk = Math.Sqrt((2.0 * k - 1.0) / ((double)N - k) * (2.0 * k + 1.0) / ((double)N + k)) / ((double)k);
                    Yk = (k - 1.0) * Math.Sqrt((2.0 * k + 1.0) / (2.0 * k - 3.0) * ((double)N - k + 1.0) /
                                               ((double)N - k) * ((double)N + k - 1.0) / ((double)N + k)) / ((double)k);
                    threshold = -1; sgn = sgn * (-1);
                    pval = 0.0;
                    if (k == n)
                    {
                        threshold = -1;
                    }
                    //values
                    for (int j = N2 - 1; j >= 0; j--)
                    {
                        T[k][j] = (2 * j - N1) * Xk * T[k - 1][j] - Yk * T[k - 2][j];
                        T[k][N - 1 - j] = sgn * T[k][j];
                        if (j == 235)
                        {
                            pval += T[k][j] * T[k][j];
                        }
                        else { pval += T[k][j] * T[k][j]; }
                        if (pval > eps1)
                        {
                            threshold = j - 1;
                            break;
                        }
                    }
                    //explicit formulaes
                    if (threshold > -1)
                    {
                        //first one is zero
                        T[k][0] = 0.0; T[k][N - 1] = 0.0;

                        if (threshold > 0)
                        {
                            //Outer Coeff
                            explC = k + 0.5;
                            for (int j = 1; j <= k; j++) { explC = explC * (N - k + j - 1.0) / ((double)N + j); }
                            if (k % 2 == 0) { explC = Math.Sqrt(explC); }
                            else { explC = -Math.Sqrt(explC); }
                            //Tk(1)
                            T[k][1] = 1.0 / (double)k - (double)k / (N - 1.0);
                            T[k][1] = T[k][1] * explC;
                            T[k][N - 2] = sgn * T[k][1];
                            if (threshold > 1)
                            {
                                //Tk(2)
                                T[k][2] = ((double)(k * k) - 1.0) / ((double)N - 1.0) * (double)k /
                                    ((double)N - 2.0) * 0.5 - 2.0 * (double)k / ((double)N - 1.0) + 1.0 / (double)k;
                                T[k][2] = T[k][2] * explC;
                                T[k][N - 3] = sgn * T[k][2];

                                if (threshold > 2)
                                {
                                    //Tk(3)
                                    T[k][3] = 1.5 * ((double)(k * k) - 1.0) / ((double)N - 1.0) * (double)k / ((double)N - 2.0) - 3.0 * (double)k / ((double)N - 1.0) + 1.0 / (double)k
                                        - (double)k / 6.0 * ((double)(k * k) - 1.0) / ((double)N - 3.0) * ((double)(k * k) - 4.0) / ((double)N - 2.0) / ((double)N - 1.0);
                                    T[k][3] = T[k][3] * explC;
                                    T[k][N - 4] = sgn * T[k][3];

                                    //Calculating Rest Part
                                    //Inner sum
                                    for (int x = 4; x <= threshold; x++)
                                    {
                                        T[k][x] = 1.0 / (double)k;
                                        sgn1 = -1;

                                        for (int p = 1; p <= x; p++)
                                        {
                                            explX = k;
                                            for (int l = 1; l <= p - 1; l++)
                                            {
                                                explX = explX * ((double)(k * k) - l * l) / (N - l - 1.0);
                                            }
                                            explX1 = x;
                                            for (int l = 1; l <= p - 1; l++)
                                            {
                                                explX1 = explX1 * ((double)x - l) / (double)l / (double)l;
                                            }
                                            explX = explX * explX1 / ((double)N - 1.0);

                                            T[k][x] += sgn1 * explX;
                                            sgn1 = sgn1 * (-1);
                                        }
                                        /*for (int l = 0; l <= k; l++) {
                                            explX = x * (N - l);
                                            for (int i = 1; i <= l - 1; i++) {
                                                explX = explX * ((double)x - i) / ((double)N - i);
                                            }
                                            T[k][x] += sgn1 * (double)C[k][l] * (double)C[k + l][l] * explX / ((double)k + (double)l);
                                            sgn1 = sgn1 * (-1);
                                        }*/
                                        T[k][x] = T[k][x] * explC;
                                        T[k][N - 1 - x] = sgn * T[k][x];
                                    }
                                } //threshold >= 3
                            } //threshold >= 2
                        } //threshold >= 1

                    } //explicit formulas
                } //n-th loop
                output = "Threshold: " + threshold.ToString() + "||  ";
                for (int x = 0; x <= threshold; x++)
                {
                    output += "T[" + n.ToString() + "][" + x.ToString() + "] = " + T[n][x] + ";  ";
                }
            } //even N
            else
            { //if odd N
                for (int k = 2; k <= n; k++)
                {
                    //coeffs
                    Xk = Math.Sqrt((2.0 * k - 1.0) / ((double)N - k) * (2.0 * k + 1.0) / ((double)N + k)) / ((double)k);
                    Yk = (k - 1.0) * Math.Sqrt((2.0 * k + 1.0) / (2.0 * k - 3.0) * (N - k + 1.0) /
                                                       ((double)N - k) * (N + k - 1.0) / ((double)N + k)) / ((double)k);
                    threshold = -1; sgn = sgn * (-1);
                    //nod [(N-1)/2]
                    T[k][N2] = Yk * T[k - 2][N2];
                    pval = 0.5 * T[k][N2];
                    //values
                    for (int j = N2 - 1; j >= 0; j--)
                    {
                        T[k][j] = (2 * j - N1) * Xk * T[k - 1][j] - Yk * T[k - 2][j];
                        T[k][N - 1 - j] = sgn * T[k][j];
                        pval += T[k][j] * T[k][j];
                        if (pval > eps1)
                        {
                            threshold = j - 1;
                            break;
                        }
                    }
                    //explicit formulaes
                    if (threshold > -1)
                    {
                        //first one is zero
                        T[k][0] = 0.0;
                        if (threshold > 0)
                        {
                            //Calculating Rest Part
                            //Outer Coeff
                            explC = k + 0.5;
                            for (int j = 1; j <= k; j++) { explC = explC * (N - j) / (N + j); }
                            if (k % 2 == 0) { explC = Math.Sqrt(explC); }
                            else { explC = -Math.Sqrt(explC); }
                            //Inner sum
                            for (int x = 1; x <= threshold; x++)
                            {
                                T[k][x] = 0.0;
                                sgn1 = 1;
                                for (int l = 0; l <= k; l++)
                                {
                                    explX = x * (N - l);
                                    for (int i = 1; i <= l - 1; i++)
                                    {
                                        explX = explX * ((double)x - i) / ((double)N - i);
                                    }
                                    T[k][x] += sgn1 * C[k][l] * C[k + l][l] * explX;
                                    sgn1 = sgn1 * (-1);
                                }
                                T[k][x] = T[k][x] * explC;
                                T[k][N - 1 - x] = sgn * T[k][x];
                            }
                        }
                    } // explicits
                }// n-th loop
                output = "Threshold: " + threshold.ToString();
                for (int x = 0; x <= threshold; x++)
                {
                    output += "T[" + n.ToString() + "][" + x.ToString() + "] = " + T[n][x] + ";  ";
                }
            } //odd N

            return T;
        }



        //==========================================================
        //======== CHEBYSHEV OF FIRST KIND (alpha=beta=-1/2)   =====




        //Chebyshev First Kind alpha=beta=-1/2
        //RECURSIVE
        public static double[][] ChebFirstKind(int n, int N)
        {
            //initing
            double[][] T = new double[n + 1][];
            for (int i = 0; i <= n; i++) { T[i] = new double[N]; }
            //first 2 polies
            int N1 = N - 1;
            double tmp1 = 1.0 / Math.Sqrt(Math.PI);
            double tmp2 = 1.0 / Math.Sqrt(0.5 * Math.PI * N * N1);
            for (int i = 0; i < N; i++)
            {
                T[0][i] = tmp1;
                T[1][i] = tmp2 * (2 * i - N1);
            }
            //rest n-2 polies
            double Xn, Yn;
            for (int k = 2; k <= n; k++)
            {
                //coeffs
                Xn = 2.0 / Math.Sqrt((N1 + n) * (N - n));
                Yn = Math.Sqrt((N1 + n - 1.0) / (N1 + n) * (N - n + 1.0) / (N - n));
                //values
                for (int j = 0; j < N; j++)
                {
                    T[k][j] = (2 * j - N1) * Xn * T[k - 1][j] - Yn * T[k - 2][j];
                }
            }

            return T;
        }





        //==========================================================
        //======== CHEBYSHEV OF FIRST KIND (alpha=beta=-1/2)   =====



        //Chebyshev Second Kind alpha=beta=-1/2
        //RECURSIVE
        public static double[][] ChebSecondKind(int n, int N)
        {
            //initing
            double[][] T = new double[n + 1][];
            for (int i = 0; i <= n; i++) { T[i] = new double[N]; }
            //first 2 polies
            int N1 = N - 1;
            double tmp1 = 1.0 / Math.Sqrt(0.5 * Math.PI);
            double tmp2 = 2.0 / Math.Sqrt(0.5 * Math.PI * (N + 2.0) * N1);
            for (int i = 0; i < N; i++)
            {
                T[0][i] = tmp1;
                T[1][i] = tmp2 * (2 * i - N1);
            }
            //rest n-2 polies
            double Xn, Yn;
            for (int k = 2; k <= n; k++)
            {
                //coeffs
                Xn = 2.0 / Math.Sqrt((N + n + 1.0) * (N - n));
                Yn = Math.Sqrt((N + n) / (N + n + 1.0) * (N - n + 1.0) / (N - n));
                //values
                for (int j = 0; j < N; j++)
                {
                    T[k][j] = (2 * j - N1) * Xn * T[k - 1][j] - Yn * T[k - 2][j];
                }
            }

            return T;
        }




        //==========================================================
        //======== CHEBYSHEV ULTRASPHERICAL (alpha=beta)   =====

        //returns norm of ultraspherical polynome tau^alpha_nN in case of integer alphas
        public static double hnN(int n, int N, int alpha)
        {
            double hn = Math.Pow(4.0, alpha + 0.5);
            double tmp = (double)n + alpha;
            for (int i = 1; i <= alpha; i++)
            {
                hn = hn * ((double)n + (double)i) / (tmp + (double)i);
            }
            hn = hn / (2.0 * n + 2.0 * alpha + 1.0);

            tmp = (double)N + 2.0 * alpha + 1.0;
            for (int i = 1; i <= n; i++)
            {
                hn = hn * (tmp + n - i) / ((double)N - i);
            }

            return hn;
        }
        //same, but with square root
        public static double hnNsq(int n, int N, int alpha)
        {
            double hn = Math.Pow(2.0, alpha + 0.5);
            double tmp = (double)n + alpha;
            for (int i = 1; i <= alpha; i++)
            {
                hn = hn * Math.Sqrt(((double)n + (double)i) / (tmp + (double)i));
            }
            hn = hn / (2.0 * n + 2.0 * alpha + 1.0);

            tmp = (double)N + 2.0 * alpha + 1.0;
            for (int i = 1; i <= n; i++)
            {
                hn = hn * Math.Sqrt((tmp + n - i) / ((double)N - i));
            }

            return hn;
        }


        //Chebyshev ULTRASPHERICAL alpha=beta>=0 integers
        //RECURSIVE
        public static double[][] ChebUltrasphRec(int n, int N, int alpha)
        {
            if (alpha == 0) { return ChebLegendreRec(n, N); }

            //initing
            double[][] T = new double[n + 1][];
            for (int i = 0; i <= n; i++) { T[i] = new double[N]; }

            //first 2 polies
            double N1 = N - 1.0;
            double tmp1 = 1.0;
            for (int i = 1; i <= alpha; i++)
            {
                tmp1 = tmp1 * Math.Sqrt(alpha + i) / Math.Sqrt(i);
            }
            tmp1 = tmp1 * Math.Sqrt((2.0 * alpha + 1.0)) * Math.Pow(2.0, -alpha - 0.5);

            double tmp2 = Math.Sqrt(2.0 * alpha + 3.0) / Math.Sqrt(N1) / Math.Sqrt(N + 2.0 * alpha + 1.0);

            if (n > 0)
            {
                for (int i = 0; i < N; i++)
                {
                    T[0][i] = tmp1;
                    T[1][i] = tmp2 * (2 * i - N1) * tmp1;
                }
            }
            else
            {
                for (int i = 0; i < N; i++) { T[0][i] = tmp1; }
            }



            //rest n-2 polies
            double Xk, Yk;
            for (int k = 2; k <= n; k++)
            {
                //coeffs
                Xk = Math.Sqrt(
                    (2.0 * k + 2.0 * alpha - 1.0) / ((double)N - k) * (2.0 * k + 2.0 * alpha + 1.0) / ((double)N + 2.0 * alpha + k) / (k * k + 2.0 * alpha * k)
                );

                Yk = Math.Sqrt(
                    (N1 + k + 2.0 * alpha) / (N + k + 2.0 * alpha) * (N - k + 1.0) / ((double)N - k) *
                    (k - 1.0) / (double)k * (k + 2.0 * alpha - 1.0) / (k + 2.0 * alpha) *
                    (2.0 * k + 2.0 * alpha + 1.0) / (2.0 * k + 2.0 * alpha - 3.0)
                );
                //values
                for (int j = 0; j < N; j++)
                {
                    T[k][j] = (2 * j - N1) * Xk * T[k - 1][j] - Yk * T[k - 2][j];
                }
            }

            return T;
        }


        //Chebyshev  ULTRASPHERICAL alpha=beta>=0 integers  Fix on Ends
        //(rest part filled with zeros)
        public static double[][] ChebUltrasphRecFromCenter(int n, int N, int alpha, out string output, double eps = 1E-14)
        {
            if (alpha == 0) { return ChebLegendreRecFromCenter(n, N, out output); }
            output = "";

            //initing
            double[] weight = ChebWeightUltrasph(N, alpha);
            double[][] T = new double[n + 1][];
            for (int i = 0; i <= n; i++) { T[i] = new double[N]; }
            //first 2 polies
            double N1 = N - 1;

            /*double tmp1 = 1.0;
            for (int i = 1; i <= alpha; i++) {
                tmp1 = tmp1 * ((double)alpha + i) / (double)i;
            }
            tmp1 = Math.Sqrt(tmp1 * (2.0 * alpha + 1.0)) * Math.Pow(2.0, -alpha - 0.5);

            double tmp2 = Math.Sqrt((2.0 * alpha + 3.0) / N1 / (N + 2.0 * alpha + 1.0));*/

            //============================
            //EXTRA OPERATIONS FOR SQRTs IS FOR MORE ACCURACY 
            //(well maybe it is more accurate, i dunno)
            double tmp1 = 1.0;
            for (int i = 1; i <= alpha; i++)
            {
                tmp1 = tmp1 * Math.Sqrt(alpha + i) / Math.Sqrt(i);
            }
            tmp1 = tmp1 * Math.Sqrt((2.0 * alpha + 1.0)) * Math.Pow(2.0, -alpha - 0.5);

            double tmp2 = Math.Sqrt(2.0 * alpha + 3.0) / Math.Sqrt(N1) / Math.Sqrt(N + 2.0 * alpha + 1.0);

            if (n > 0)
            {
                for (int i = 0; i < N; i++)
                {
                    T[0][i] = tmp1;
                    T[1][i] = tmp2 * (2 * i - N1) * tmp1;
                }
            }
            else
            {
                for (int i = 0; i < N; i++) { T[0][i] = tmp1; }
            }


            //rest n-2 polies
            double Xk, Yk;
            int threshold; int N2 = N / 2;
            int sgn = -1;
            double eps1;

            //if even N
            if (N % 2 == 0)
            {
                for (int k = 2; k <= n; k++)
                {
                    //coeffs
                    Xk = Math.Sqrt(
                        (2.0 * k + 2.0 * alpha - 1.0) / ((double)N - k) * (2.0 * k + 2.0 * alpha + 1.0) / ((double)N + 2.0 * alpha + k) / (k * k + 2.0 * alpha * k)
                    );
                    Yk = Math.Sqrt(
                        (N1 + k + 2.0 * alpha) / (N + k + 2.0 * alpha) * (N - k + 1.0) / ((double)N - k) *
                        (k - 1.0) / (double)k * (k + 2.0 * alpha - 1.0) / (k + 2.0 * alpha) *
                        (2.0 * k + 2.0 * alpha + 1.0) / (2.0 * k + 2.0 * alpha - 3.0)
                    );
                    eps1 = 1.0;
                    threshold = -1; sgn = sgn * (-1);
                    //values
                    for (int j = N2 - 1; j >= 0; j--)
                    {
                        T[k][j] = (2 * j - N1) * Xk * T[k - 1][j] - Yk * T[k - 2][j];
                        T[k][N - 1 - j] = sgn * T[k][j];
                        eps1 = eps1 - 2.0 * T[k][j] * T[k][j] * weight[j];
                        if (eps1 < eps)
                        {
                            threshold = j - 1;
                            break;
                        }
                    }
                    for (int j = threshold; j >= 0; j--) { T[k][j] = 0.0; }
                    output = "Threshold: " + threshold.ToString() + "||  ";
                    for (int x = 0; x <= threshold; x++)
                    {
                        output += "T[" + n.ToString() + "][" + x.ToString() + "] = " + T[n][x] + ";  ";
                    }
                }
            }
            else
            { //if odd N
                for (int k = 2; k <= n; k++)
                {
                    //coeffs
                    Xk = Math.Sqrt(
                        (2.0 * k + 2.0 * alpha - 1.0) / ((double)N - k) * (2.0 * k + 2.0 * alpha + 1.0) / ((double)N + 2.0 * alpha + k) / (k * k + 2.0 * alpha * k)
                    );
                    Yk = Math.Sqrt(
                        (N1 + k + 2.0 * alpha) / (N + k + 2.0 * alpha) * (N - k + 1.0) / ((double)N - k) *
                        (k - 1.0) / (double)k * (k + 2.0 * alpha - 1.0) / (k + 2.0 * alpha) *
                        (2.0 * k + 2.0 * alpha + 1.0) / (2.0 * k + 2.0 * alpha - 3.0)
                    );
                    //nod [(N-1)/2]
                    T[k][N2] = Yk * T[k - 2][N2];
                    eps1 = 1 - T[k][N2] * T[k][N2] * weight[N2];
                    threshold = -1; sgn = sgn * (-1);
                    //values
                    for (int j = N2 - 1; j >= 0; j--)
                    {
                        T[k][j] = (2 * j - N1) * Xk * T[k - 1][j] - Yk * T[k - 2][j];
                        T[k][N - 1 - j] = sgn * T[k][j];
                        eps1 -= 2.0 * T[k][j] * T[k][j] * weight[j];
                        if (eps1 < eps)
                        {
                            threshold = j - 1;
                            break;
                        }
                    }
                    for (int j = threshold; j >= 0; j--) { T[k][j] = 0.0; }
                    output = "Threshold: " + threshold.ToString() + "||  ";
                    for (int x = 0; x <= threshold; x++)
                    {
                        output += "T[" + n.ToString() + "][" + x.ToString() + "] = " + T[n][x] + ";  ";
                    }
                }
            }

            return T;
        }











        //=================================================
        //============= Scalar Product, Etc ===============

        //scalar product <f,g> with weight rho
        public static double ScalarProduct(double[] f, double[] g, double[] rho = null)
        {
            int N = f.Length;
            if (rho == null) { rho = new double[N]; for (int i = 0; i < N; i++) { rho[i] = 1.0; } }
            double result = 0.0;

            for (int i = 0; i < N; i++) { result += f[i] * g[i] * rho[i]; }

            return result;
        }

        //scalar of the whole bunch of Polies P with their weight rho
        public static double[,] PoliesScalarProduct(double[][] P, double[] rho = null)
        {
            int n = P.Length;
            int N = P[0].Length;
            if (rho == null) { rho = new double[N]; for (int i = 0; i < N; i++) { rho[i] = 1.0; } }
            double[,] result = new double[n, n];

            //assigning zeros
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++) { result[i, j] = 0.0; }

            //summing for top triangle of matrix
            for (int x = 0; x < N; x++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = i; j < n; j++)
                    {
                        result[i, j] += P[i][x] * P[j][x] * rho[x];
                    }
                }
            }
            //just reflect top on the bottom triangle 
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++) { result[j, i] = result[i, j]; }

            return result;
        }


        //Difference between given matrix and matrix of zeros
        public static double DiffFromZeros(double[,] M, bool isSquareRoot = false)
        {
            int N = M.GetLength(0);
            double result = 0.0;
            //assigning zeros
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++) { result += M[i, j] * M[i, j]; }
            if (isSquareRoot) { result = Math.Sqrt(result); }

            return result;
        }


        //Difference between given matrix and E-matrix
        public static double DiffFromEMatrix(double[,] M, bool isSquareRoot = false)
        {
            int N = M.GetLength(0);
            double result = 0.0;
            //assigning zeros
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (i != j) { result += M[i, j] * M[i, j]; }
                    else { result += (1.0 - M[i, j]) * (1.0 - M[i, j]); }
            if (isSquareRoot) { result = Math.Sqrt(result); }

            return result;
        }






    }
}
