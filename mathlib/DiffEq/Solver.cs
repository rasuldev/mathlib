using System;
using DiscreteFunctions;

namespace mathlib.DiffEq
{
    public static class Solver
    {
        /// <summary>
        /// Solves Cauchy problem y'(x)=f(x,y), y(x0)=y0 on segment [x0,b] using Euler method. 
        /// </summary>
        /// <param name="f">Function f(x)</param>
        /// <param name="x0">Initial param x0</param>
        /// <param name="y0">Initial param y0</param>
        /// <param name="b">Right side of segment to find solution on -- [x0, b]</param>
        /// <param name="n">Grid points count</param>
        /// <returns>Solution as DiscreteFunction2D</returns>
        public static DiscreteFunction2D Euler(Func<double, double, double> f, double x0, double y0, double b, int n)
        {
            var y = new double[n];
            var x = new double[n];
            y[0] = y0;
            x[0] = x0;
            var h = (b - x0) / (n - 1);
            var xk = x0;
            for (int k = 0; k < n-1; k++)
            {
                y[k + 1] = y[k] + h * f(x[k], y[k]);
                x[k + 1] = x[k] + h;
            }
            return new DiscreteFunction2D(x, y);
        }

        /// <summary>
        /// Solves Cauchy problem y'(x)=f(x,y), y(x0)=y0 on segment [x0,b] using Euler-Cauchy method. 
        /// </summary>
        /// <param name="f">Function f(x)</param>
        /// <param name="x0">Initial param x0</param>
        /// <param name="y0">Initial param y0</param>
        /// <param name="b">Right side of segment to find solution on -- [x0, b]</param>
        /// <param name="n">Grid points count</param>
        /// <returns>Solution as DiscreteFunction2D</returns>
        public static DiscreteFunction2D EulerCauchy(Func<double, double, double> f, double x0, double y0, double b, int n)
        {
            var y = new double[n];
            var x = new double[n];
            y[0] = y0;
            x[0] = x0;
            var h = (b - x0) / (n - 1);
            var xk = x0;
            for (int k = 0; k < n - 1; k++)
            {
                y[k + 1] = y[k] + h * f(x[k], y[k]);
                x[k + 1] = x[k] + h;
                y[k + 1] = y[k] + 0.5 * h * (f(x[k], y[k]) + f(x[k + 1], y[k + 1]));
            }
            return new DiscreteFunction2D(x, y);
        }


        /// <summary>
        /// Solves Cauchy problem y'(x)=f(x,y), y(x0)=y0 on segment [x0,b] using enhanced Euler-Cauchy method. 
        /// </summary>
        /// <param name="f">Function f(x)</param>
        /// <param name="x0">Initial param x0</param>
        /// <param name="y0">Initial param y0</param>
        /// <param name="b">Right side of segment to find solution on -- [x0, b]</param>
        /// <param name="n">Grid points count</param>
        /// <returns>Solution as DiscreteFunction2D</returns>
        public static DiscreteFunction2D EulerImproved(Func<double, double, double> f, double x0, double y0, double b,
            int n)
        {
            var y = new double[n];
            var x = new double[n];
            y[0] = y0;
            x[0] = x0;
            
            var h = (b - x0) / (n - 1);
            var xk = x0;
            for (int k = 0; k < n - 1; k++)
            {
                var yhalf = y[k] + 0.5 * h * f(x[k], y[k]);
                var xhalf = x[k] + 0.5 * h;
                y[k + 1] = y[k] + h * f(xhalf, yhalf);
                x[k + 1] = x[k] + h;
                // y[k + 1] = y[k] + 0.5 * h * (f(x[k], y[k]) + f(x[k + 1], y[k + 1]));
            }
            return new DiscreteFunction2D(x, y);
        }


    }
    
}