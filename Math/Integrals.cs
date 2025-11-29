using System;
using System.Linq;
using mathlib.Symbolic;
using MoreLinq;

namespace mathlib
{
    public static class Integrals
    {
        /// <summary>
        /// Calculates integral on (a,b) using nodes (nodes[i] that not in [a,b] are not taken into account)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="nodes">Sorted array of nodes</param>
        /// <param name="formulaType"></param>
        /// <returns></returns>
        public static double Rectangular(Func<double, double> f, double a, double b, double[] nodes,
            RectType formulaType)
        {
            //var nodesInSegment = nodes.SkipWhile(x => x < a).TakeWhile(x => x < b).ToList();
            //var i = 0;
            //while (nodes[i] < a && i < nodes.Length) ++i;



            //for (int i = 0; i < nodesInSegment.Length; i++)
            //{
                
            //}
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns integral from nodes[0] to nodes[last] of function f.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="nodes"></param>
        /// <param name="formulaType"></param>
        /// <returns></returns>
        public static double Rectangular(Func<double, double> f, double[] nodes, RectType formulaType)
        {
            double sum = 0;
            double a, b;
            switch (formulaType)
            {
                case RectType.Left:
                    a = 1;
                    b = 0;
                    break;
                case RectType.Right:
                    a = 0;
                    b = 1;
                    break;
                case RectType.Center:
                    a = b = 0.5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(formulaType), formulaType, null);
            }

            for (int i = 0; i < nodes.Length - 1; i++)
            {
                sum += f(a * nodes[i] + b * nodes[i + 1]) * (nodes[i + 1] - nodes[i]);
            }
            return sum;
        }

        public static double Trapezoid(Func<double, double> f, double[] nodes)
        {
            double sum = 0;
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                sum += (f(nodes[i + 1]) + f(nodes[i])) * (nodes[i + 1] - nodes[i]);
            }
            return sum / 2;
        }

        /// <summary>
        /// Calculates integral for discretized f. 
        /// It is supposed that nodes are increasing sequence of points.
        /// </summary>
        /// <param name="f">f[j]=f(nodes[j])</param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static double Trapezoid(double[] f, double[] nodes)
        {
            double sum = 0;
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                sum += (f[i + 1] + f[i]) * (nodes[i + 1] - nodes[i]);
            }
            return sum / 2;
        }

        /// <summary>
        /// Uses equidistant net
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="nodesCount"></param>
        /// <returns></returns>
        public static double Trapezoid(Func<double, double> f, double a, double b, int nodesCount)
        {
            double sum = 0.5 * (f(a) + f(b));
            var delta = (b - a) / (nodesCount - 1);
            var x = a + delta;
            
            // In fact, here we must use condition x < b, but due to calc errors 
            // last x = a+delta*(nodesCount-1) can be less than b
            while (x<b-0.5*delta)
            {
                sum += f(x);
                x += delta;
            }
            return sum*delta;
        }

        public static double Rectangular(Func<double, double> f, double a, double b, int nodesCount, RectType formulaType = RectType.Left)
        {
            var nodes = Enumerable.Range(0, nodesCount).Select(j => a + j * (b - a) / (nodesCount - 1)).ToArray();
            return Rectangular(f, nodes, formulaType);
        }

        public static double Rectangular(Integral integral, int nodesCount, RectType formulaType = RectType.Left)
        {
            return Rectangular(integral.Function, integral.LowerBound, integral.UpperBound, nodesCount, formulaType);
        }

        /// <summary>
        /// If upper bound of integral is infinite then for numerically calculation we should set some finite upper bound.
        /// </summary>
        /// <param name="integral"></param>
        /// <param name="nodesCount"></param>
        /// <param name="upperBound"></param>
        /// <param name="formulaType"></param>
        /// <returns></returns>
        public static double RectangularInfinite(Integral integral, int nodesCount, double upperBound, RectType formulaType = RectType.Left)
        {
            return Rectangular(integral.Function, integral.LowerBound, upperBound, nodesCount, formulaType);
        }

        public static double Trapezoid(Integral integral, int nodesCount)
        {
            return Trapezoid(integral.Function, integral.LowerBound, integral.UpperBound, nodesCount);
        }

        public enum RectType
        {
            Left,
            Right,
            Center
        }
    }
}