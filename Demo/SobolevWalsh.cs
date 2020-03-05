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
using mathlib.Symbolic;
using static System.Linq.Enumerable;
using static System.Math;

namespace Demo
{
    public partial class SobolevWalsh : GraphBuilder2DForm
    {
        private readonly Plot2D _plot = new Plot2D("Walsh - Sobolev");
        private readonly Plot2D _plot2 = new Plot2D("Walsh - Sobolev - 2^k");
        private readonly Plot2D _plot3 = new Plot2D("Sum");

        public SobolevWalsh()
        {
            InitializeComponent();
            GraphBuilder.DrawPlot(_plot);
            GraphBuilder.DrawPlot(_plot2);
            GraphBuilder.DrawPlot(_plot3);
        }

        private void nupNum_ValueChanged(object sender, EventArgs e)
        {
            var n = (int)nupNum.Value;
            var pow2 = (int)Pow(2, Floor(Log(n - 1, 2)));

            var a = 0.5;
            _plot.DiscreteFunction = new DiscreteFunction2D(x => -1 / Log(a * x), 0, 1, 1000);
            _plot.Refresh();

            //var y = Range(1, 20).Select(k => Sum((int)(Pow(2, k) + 1))(1.0 / Pow(2, k + 1))).ToArray();
            //_plot.DiscreteFunction = y;
            //_plot.Refresh();

            //_plot.DiscreteFunction =
            //    new DiscreteFunction2D(WalshSobolev.Get(n), 0, 1, 1024);
            //_plot.Refresh();
            _plot2.DiscreteFunction =
               new DiscreteFunction2D(WalshSobolev.Get(2, n), 0, 1, 1024);
            _plot2.Refresh();

            //_plot.DiscreteFunction = new DiscreteFunction2D(x => W12(Math.Pow(2, n) * x) / Math.Pow(2, n), 0, 1, 1024 * 64);
            //_plot.Refresh();

            //_plot2.DiscreteFunction = new DiscreteFunction2D(Sum(n), 0, 1, 1024 * 128);
            //_plot2.Refresh();

            //_plot3.DiscreteFunction = new DiscreteFunction2D(SumOfAbs(n),0,1,1024*128);
            //_plot3.Refresh();
            //_plot2.DiscreteFunction = new DiscreteFunction2D(SumOfAbs((int)Math.Pow(2, n)), 0, 1, 1024 * 128);
            //_plot2.Refresh();
            //_plot3.DiscreteFunction = new DiscreteFunction2D(SumOfAbs((int)Math.Pow(2, n-1)), 0, 1, 1024 * 128);
            //_plot3.Refresh();


            //var max = MaxOfS2k(n+1).Y.Last();
            //_plot3.DiscreteFunction = new DiscreteFunction2D(new []{0d,1}, new []{max,max});
            //_plot3.Refresh();
            //_plot2.DiscreteFunction = UpperEstMaxOfS2k(n);
            //_plot2.Refresh();

            //_plot.DiscreteFunction = new DiscreteFunction2D(x=>1d/2,0,1,1024);
            //_plot.Refresh();
            //_plot2.DiscreteFunction = new DiscreteFunction2D(x =>x*x + 1d / 4, 0, 1, 1024);
            //_plot2.Refresh();
        }

        double W12(double x)
        {
            x = x - (int)x;
            return x < 0.5 ? x : 1 - x;
        }



        //private Func<double, double> w12Di()

        private Func<double, double> SumOfSquares(int n)
        {

            double SumOfn(double x)
            {
                var s = 0d;
                for (int k = 0; k < n; k++)
                {
                    var powk = Math.Pow(2, k);
                    s += Math.Pow(W12(powk * x), 2) / powk;
                }
                return s;
            }

            return SumOfn;

        }

        private DiscreteFunction2D MaxOfS2k(int n)
        {
            var y = new double[n];
            y[0] = 0;
            y[1] = 0.5d;
            for (int i = 2; i < y.Length; i++)
            {
                y[i] = 0.5 * (y[i - 1] + y[i - 2] + 1);
            }
            return new DiscreteFunction2D(y);
        }

        private DiscreteFunction2D UpperEstMaxOfS2k(int n)
        {
            var y = new double[n];
            y[0] = 1;
            for (int i = 1; i < y.Length; i++)
            {
                y[i] = y[i - 1] + 1d / Math.Pow(2, 0.5 * i);
            }
            return new DiscreteFunction2D(y);
        }

        private Func<double, double> Sum(int n)
        {

            double SumOfn(double x)
            {
                var s = 0d;
                for (int k = 2; k <= n; k++)
                {
                    s += WalshSobolev.Get2(k)(x);
                }
                return s;
            }

            return SumOfn;

        }

        private Func<double, double> SumOfAbs(int n)
        {

            double SumOfn(double x)
            {
                var s = 0d;
                for (int k = 2; k <= n; k++)
                {
                    s += Math.Abs(WalshSobolev.Get2(k)(x));
                }
                return s;
            }

            return SumOfn;

        }
    }
}
