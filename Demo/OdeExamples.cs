using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mathlib;
using static System.Math;

namespace Demo
{
    public static class OdeExamples
    {
        public static (double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem1()
        {
            var initVals = new[] { 1d, 1 };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>(3, args => args[1]*args[1]/(args[2]-args[0])),
                new DynFunc<double>(3, args => args[1]+1)
            };



            var h = 1d;
            return (initVals, f, h, new Func<double, double>[] { x => Exp(x), x => x + Exp(x) });
        }

        public static (double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem3()
        {
            var initVals = new[] { 1d, 2 };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>((x, y1, y2) => y1 - y2 +2*Sin(x)),
                new DynFunc<double>((x,y1,y2) => 2*y1-y2)
            };

            var h = 1d;
            return (initVals, f, h, new Func<double, double>[] { x => Cos(x) + x * Sin(x) - x * Cos(x), x => 2 * (Sin(x) + Cos(x)) - 2 * x * Cos(x) });
        }

        public static (double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem4()
        {
            var initVals = new[] { 1d, 2 };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>((x, y1, y2) => 4*y1 - 3*y2 + Sin(x)),
                new DynFunc<double>((x, y1, y2) => 2*y1 - y2 - 2*Cos(x))
            };

            var h = 1d;
            return (initVals, f, h, new Func<double, double>[] { x => Cos(x) - 2 * Sin(x), x => 2 * (Cos(x) - Sin(x)) });
        }

        public static (double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem5()
        {
            var initVals = new[] { 1d, 2 };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>((x, y1, y2) => (4d*y1 - 3d*y2 + Sin(2*PI*x))*2*PI),
                new DynFunc<double>((x, y1, y2) => (2d*y1 - y2 - 2d*Cos(2*PI*x))*2*PI)
            };

            var h = 1d;
            return (initVals, f, h, new Func<double, double>[] { x => Cos(2 * PI * x) - 2 * Sin(2 * PI * x), x => 2 * (Cos(2 * PI * x) - Sin(2 * PI * x)) });
        }

        public static (Segment segment, double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem6()
        {
            var initVals = new[] { 0, 0d };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>((x, y1, y2) => 11 - 20 * y2 * y2),
                new DynFunc<double>((x, y1, y2) => 2.5*(1+Sqrt(1-(y1-x)*(y1-x))))
            };

            var h = 0.1d;
            return (new Segment(0, h), initVals, f, h, new Func<double, double>[] { x => x + Sin(10 * x), x => Sin(5 * x) });
        }

        public static (Segment segment, double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem7()
        {
            var initVals = new[] { 2, 0d, -4 };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>(4, args => 1 * args[1] + -1 * args[2] + -1 * args[3]),
                new DynFunc<double>(4, args => 1 * args[1] + 1 * args[2] + 0 * args[3]),
                new DynFunc<double>(4, args => 3 * args[1] + 0 * args[2] + 1 * args[3])
            };

            var h = PI / 2;
            return (new Segment(0, h), initVals, f, h, new Func<double, double>[]
            {
                x => Exp(x) * (2 * Sin(2 * x) + 2 * Cos(2 * x)),
                x => Exp(x) * (1 - Cos(2 * x) + Sin(2 * x)),
                x => Exp(x) * (-1 - 3 * Cos(2 * x) + 3 * Sin(2 * x)),
            });
        }

        public static (Segment segment, double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem8()
        {
            Segment segment = new Segment(0, PI / 2);

            Func<double, double>[] funcs = new Func<double, double>[]
            {
                x => Exp(x) * (Cos(3 * x) + Sin(3 * x)),
                x => Exp(x) * (Sin(3 * x) - Cos(3 * x))
            };

            var initVals = new[] { funcs[0](segment.Start), funcs[1](segment.Start) };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>(3, args => 1 * args[1] + -3 * args[2]),
                new DynFunc<double>(3, args => 3 * args[1] + 1 * args[2])
            };

            return (segment, initVals, f, segment.End, funcs);
        }

        public static (Segment segment, double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem9()
        {
            var initVals = new[] { 1d, 1d };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>(3, args => -1 * args[1] + -5 * args[2]),
                new DynFunc<double>(3, args => 1 * args[1] + 1 * args[2])
            };

            var h = PI;
            return (new Segment(0, h), initVals, f, h, new Func<double, double>[]
            {
                x => Cos(2 * x) - 3 * Sin(2 * x),
                x => Cos(2 * x) + Sin(2 * x),
            });
        }

        public static (Segment segment, double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem10()
        {
            var initVals = new[] { 2d, 0d };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>(3, args => 1 * args[1] + -1 * args[2]),
                new DynFunc<double>(3, args => -4 * args[1] + 1 * args[2])
            };

            var h = 1;
            return (new Segment(0, h), initVals, f, h, new Func<double, double>[]
            {
                x => Exp(-x) + Exp(3 * x),
                x => 2 * Exp(-x) - 2 * Exp(3 * x),
            });
        }

        public static (double[] initVals, DynFunc<double>[] f, double h, Func<double, double>[] yExact) ExampleSystem2()
        {
            var initVals = new[] { 0d, 1, 1 };
            var f = new DynFunc<double>[]
            {
                new DynFunc<double>((x, y, z) => 1d),
                new DynFunc<double>((x, y, z) => 1),
                new DynFunc<double>(3, args => args[1]+1)
            };

            var h = 1d;
            return (initVals, f, h, new Func<double, double>[] { x => Exp(x), x => x + Exp(x) });
        }

        public static (Segment segment, double y0, Func<double, double, double> f, Func<double, double> yExact) Example1()
        {
            //var x0 = 0d;
            Func<double, double, double> f = (x, y) => x * y;
            //var b = 1;

            var h = 0.5d;
            var segment = new Segment(0, h);
            Func<double, double> yExact = x => Exp(x * x / 2);
            var y0 = yExact(segment.Start);
            return (segment, y0, f, yExact);
        }

        public static (Segment segment, double y0, Func<double, double, double> f, Func<double, double> yExact) Example2()
        {
            //var x0 = 0d;
            Func<double, double, double> f = (x, y) => x * y;
            //var b = 1;

            //var h = 1d;
            Func<double, double> yExact = x => Exp(x * x / 2) / 10;
            var segment = new Segment(0, 6);
            var y0 = yExact(segment.Start);
            return (segment, y0, f, yExact);
        }

        public static (Segment segment, double y0, Func<double, double, double> f, Func<double, double> yExact) Example3()
        {
            //var x0 = 0d;
            Func<double, double, double> f = (x, y) => -2 * x * y * y / (x * x - 1);
            //var b = 1;

            var h = 0.5d;
            double YExact(double x) => 1.0 / (Log(1 - x * x) + 1);
            var segment = new Segment(0, 0.5);
            var y0 = YExact(segment.Start);
            return (segment, y0, f, YExact);
        }

        public static (Segment segment, double y0, Func<double, double, double> f, Func<double, double> yExact) ExampleDiscontinuous1()
        {
            //var x0 = 0d;
            Func<double, double, double> f = (x, y) => x < 0.5 ? 0 : 1;
            //var b = 1;
            var y0 = 1;
            double YExact(double x) => (x < 0.5 ? y0 : y0 + x - 0.5);
            var segment = new Segment(0, 1);
            //var y0 = YExact(segment.Start);
            return (segment, y0, f, YExact);
        }

        public static (Segment segment, double y0, Func<double, double, double> f, Func<double, double> yExact) ExampleDiscontinuous2()
        {
            //var x0 = 0d;
            Func<double, double, double> f = (x, y) => x < 0.5 ? y : y + 1;
            //var b = 1;
            var y0 = 1;
            double YExact(double x) => (x < 0.5 ? y0 * Exp(x) : (y0 * Sqrt(E) + 1) / Sqrt(E) * Exp(x) - 1);
            var segment = new Segment(0, 1);
            //var y0 = YExact(segment.Start);
            return (segment, y0, f, YExact);
        }

        public static (Segment segment, double y0, Func<double, double, double> f, Func<double, double> yExact) ExampleDiscontinuous3()
        {
            //var x0 = 0d;
            Func<double, double, double> f = (x, y) => Sign(x - 0.5);
            //var b = 1;
            var y0 = 1;
            double YExact(double x) => (x < 0.5 ? y0 * Exp(x) : (y0 * Sqrt(E) + 1) / Sqrt(E) * Exp(x) - 1);
            var segment = new Segment(0, 1);
            //var y0 = YExact(segment.Start);
            return (segment, y0, f, YExact);
        }

        public static (Segment segment, double y0, Func<double, double, double> f, Func<double, double> yExact) ExampleDiscontinuous4()
        {
            var b = 5d;
            var T = 0.5;
            Func<double, double, double> f = (x, y) => -b * y + (1 + Sign(x - T)) * 0.5;
            //var b = 1;
            var y0 = 0;
            double YExact(double x) => (x < T ? 0 : 1 / b * (1 - Exp(-b * (x - T))));
            var segment = new Segment(0, 1);
            //var y0 = YExact(segment.Start);
            return (segment, y0, f, YExact);
        }

        public static (Segment segment, double y0, Func<double, double, double> f, Func<double, double> yExact) ExampleDiscontinuous5()
        {
            double finner(double x)
            {
                if (x > 0.25 && x < 0.75)
                    return x;

                if (x >= 0.75 && x <= 1)
                    return 1;

                return 0;
            }

            double f(double x, double y)
            {
                return -2 * y + finner(x);
            }

            //Func<double, double, double> f = (x, y) => -2*y + ;
            //var b = 1;
            var y0 = 0;
            double YExact(double x)
            {
                if (x < 0.25) return 0;
                if (x < 0.75) return 0.5 * (x + 0.25 * Exp(0.5 - 2 * x) - 0.5);
                return 0.5 + Exp(0.5 - 2 * x) * (-3 * E + 1) / 8;
            }

            double YExact2(double x)
            {
                return Exp(-2 * x) * Integrals.Trapezoid(t => Exp(2 * t) * finner(t), 0, x, 1000);
            }

            var segment = new Segment(0, 1);
            //var y0 = YExact(segment.Start);
            return (segment, y0, f, YExact);
        }

        public static (Segment segment, double y0, Func<double, double, double> f, Func<double, double> yExact) ExampleDiscontinuous6()
        {
            double f(double x, double y)
            {
                if (x < 0.5)
                    return y - y * y + Exp(2 * x);
                return 1 + y * (x - 0.5 + Sqrt(E)) - y * y;
            }

            var y0 = 1;
            double YExact(double x)
            {
                if (x < 0.5) return Exp(x);
                return x - 0.5 + Sqrt(E);
            }

            var segment = new Segment(0, 1);
            return (segment, y0, f, YExact);
        }
    }
}
