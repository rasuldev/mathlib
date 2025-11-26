using DiscreteFunctions;
using DiscreteFunctionsPlots;
using GraphBuilders;
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

namespace Demo
{
    public partial class LegendreForm : GraphBuilder2DForm
    {
        private readonly Plot2D _plot = new Plot2D("P_n");

        public LegendreForm()
        {
            InitializeComponent();
            GraphBuilder.DrawPlot(_plot);
        }

        void DrawLegendrePolynomial(int n)
        {
            _plot.DiscreteFunction = new DiscreteFunction2D(Legendre.Get(n), -1, 1, 1024);
            _plot.Refresh();
        }

        private void nupN_ValueChanged(object sender, EventArgs e)
        {
            DrawLegendrePolynomial((int)nupN.Value);
        }
    }
}
