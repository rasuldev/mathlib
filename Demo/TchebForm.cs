using DiscreteFunctions;
using DiscreteFunctionsPlots;
using GraphBuilders;
using mathlib;
using mathlib.Polynomials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Math;

namespace Demo
{
    public partial class TchebForm : GraphBuilder2DForm
    {

        private readonly Plot2D _plot = new Plot2D("T_n");
        private readonly Plot2D _plot2 = new Plot2D("");
        private readonly Plot2D _plot3 = new Plot2D("");

        public TchebForm()
        {
            InitializeComponent();
            GraphBuilder.DrawPlot(_plot);
            GraphBuilder.DrawPlot(_plot2);
            GraphBuilder.DrawPlot(_plot3);
        }

        void Draw(int n)
        {
            var t = 0;
            _plot.DiscreteFunction = new DiscreteFunction2D(u => Kernel(n, t, u), -1, 1, 1024);
            //_plot.DiscreteFunction = new DiscreteFunction2D(x => Sqrt(2 / PI) * Cos(n * Acos(x)), -1, 1, 2048);
            _plot.Refresh();
        }

        void DrawIntegralOfKernel(int n, double t)
        {
            _plot.DiscreteFunction = new DiscreteFunction2D(x => Integrals.Trapezoid(u => Kernel(n, t, u), -1, x, 1024),
                -1, 1, 128);
            _plot.Refresh();
        }

        void DrawIntegralOfKernels(int nFrom, int nTo)
        {
            // \int_{-1}^{1} |K_n(t,u)| du
            // let t = 0
            var t = 0.7;
            var x = 0.5;
            var dfx = new List<double>();
            var dfy = new List<double>();
            for (int n = nFrom; n < nTo; n++)
            {
                //var intKn = Integrals.Trapezoid(u => Abs(Kernel(n, t, u)), -1, 1, 1024);
                var intKn = Abs(Integrals.Trapezoid(u => Kernel(n, t, u), -1, x, 1024));
                dfy.Add(intKn);
                dfx.Add(n);
            }

            _plot.DiscreteFunction = new DiscreteFunction2D(dfx.ToArray(), dfy.ToArray());
            _plot.Refresh();
        }

        double Kernel(int n, double x, double t)
        {
            var s = Sqrt(1 / PI);
            for (int i = 1; i < n + 1; i++)
            {
                s += T(i, x) * T(i, t);
            }
            return s;
        }

        double T(int n, double x)
        {
            return Sqrt(2 / PI) * Cos(n * Acos(x));
        }

        private void nupN_ValueChanged(object sender, EventArgs e)
        {
            var n = (int)nupN.Value;
            var t = (double)nupT.Value;
            DrawIntegralOfKernel(n, t);
            //Draw(n);
        }

    }
}
