using DiscreteFunctionsPlots;
using GraphBuilders;
using mathlib.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class WalshForm : GraphBuilder2DForm
    {
        Plot2D _plot = new Plot2D("Walsh partial sum");
        public WalshForm()
        {
            InitializeComponent();
            GraphBuilder.DrawPlot(_plot);
        }



        void Draw(int n)
        {
            //_plot.DiscreteFunction = new DiscreteFunctions.DiscreteFunction2D(Rademacher.Get(n), 0, 1, (int)Math.Pow(2, n + 3));
            _plot.DiscreteFunction = new DiscreteFunctions.DiscreteFunction2D(x => Math.Abs(PartialSum(n, x)), 0, 1, (int)Math.Pow(2, n + 3));
            _plot.Refresh();
        }

        double PartialSum(int n, double x)
        {
            var s = 0d;
            for (int k = 1; k <= n; k++)
            {
                s += Rademacher.Get(k)(x) / Math.Sqrt(k);
            }
            return s;
        }



        private void nupN_ValueChanged(object sender, EventArgs e)
        {
            Draw((int)nupN.Value);
        }
    }
}
