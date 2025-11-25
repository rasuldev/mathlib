using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscreteFunctions;
using DiscreteFunctionsPlots;
using GraphBuilders;
using mathlib;
using mathlib.Functions;
using static System.Math;
using static System.Linq.Enumerable;

namespace Demo
{
    public partial class SobolevWalshPartSumExample : GraphBuilder2DForm
    {
        private readonly Plot2D _plot = new Plot2D("Original");
        private readonly Plot2D _plot2 = new Plot2D("Sobolev-Walsh partial sum r=1");
        //Func<double, double> f = x => Pow(x-0.5,2)+0.5*x;
        //Func<double, double> f = x => Pow(x,3);
        Func<double, double> f = x => Sin(6 * PI * x) + x;
        Func<double, double> df = x => 6 * PI * Cos(6 * PI * x) + 1; // df = f'
        public SobolevWalshPartSumExample()
        {
            InitializeComponent();
            GraphBuilder.DrawPlot(_plot);
            _plot.DiscreteFunction = new DiscreteFunction2D(f, 0, 1, 1000);
            _plot.Refresh();
            GraphBuilder.DrawPlot(_plot2);
        }

        private void nupN_ValueChanged(object sender, EventArgs e)
        {
            var n = (int) nupN.Value;
            _plot2.DiscreteFunction = new DiscreteFunction2D(PartialSum(f, n), 0, 1, 1000);
            _plot2.Refresh();
        }

        private Func<double,double> PartialSum(Func<double, double> f, int n)
        {
            var coeffs = new List<double> {f(0)};
            coeffs.AddRange(Range(1, n).Select(j => coeff(f, j)));
            //var coeffs = Range(1, n - 1).Select(j => coeffByDef(j)).ToArray();
            double partSum(double x)
            {
                var s = 0d;
                for (int j = 0; j <= n; j++)
                {
                    s += coeffs[j] * WalshSobolev.Get2(j)(x);
                }
                return s;
            }

            return partSum;
        }

        private double coeffByDef(int n)
        {
            var w = Walsh.Get(n - 1);
            return Integrals.Trapezoid(x => df(x) * w(x), 0, 1, 10000);
        }

        /// <summary>
        /// coeff(n)=c_{1,n}(f), n > 0
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private double coeff(Func<double, double> f, int n)
        {
            // we use formula c_{1,n}(f) = c_n(f') 
            --n;
            if (n == 0)
                return f(1) - f(0);

            var k = (int)Log(n, 2) + 1;
            var w = Walsh.GetMatrix(k);
            var pow2k = Pow(2, k);
            var s = 0d;
            for (int j = 0; j <= pow2k-1; j++)
            {
                s += w[n, j] * (f((j + 1) / pow2k) - f(j / pow2k));
            }

            return s;
        }
    }
}
