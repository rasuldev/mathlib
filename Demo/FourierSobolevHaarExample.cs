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

namespace Demo
{
    public partial class FourierSobolevHaarExample : GraphBuilder2DForm
    {
        private readonly Plot2D _plot1 = new Plot2D("FourierSobolevHaar");
        private readonly Plot2D _plot2 = new Plot2D("Fast FourierSobolevHaar");

        private int lengthP;
        private int lengthX;

        private double[] p, pWithZero;

        public FourierSobolevHaarExample()
        {
            InitializeComponent();

            lengthP = (int) NumP.Value;
            lengthX = (int) NumX.Value;

            FindP();

            _plot1.DiscreteFunction = new DiscreteFunction2D(FourierSobolevHaar.Calc(pWithZero!), 0, 1, lengthX);
            _plot2.DiscreteFunction = new DiscreteFunction2D(SobolevHaarLinearCombination.FastCalc(p!), 0, 1, lengthX);

            GraphBuilder.DrawPlot(_plot1);
            GraphBuilder.DrawPlot(_plot2);

            Refresh();
        }

        private void NumP_ValueChanged(object sender, EventArgs e)
        {
            lengthP = (int)NumP.Value;

            FindP();

            PlotRefresh();
        }

        private void NumX_ValueChanged(object sender, EventArgs e)
        {
            lengthX = (int)NumX.Value;

            PlotRefresh();
        }

        private void FindP()
        {
            p = new double[lengthP];

            for (int i = 0; i < lengthP; i++)
            {
                p[i] = 1;
            }
            pWithZero = p.Prepend(0).ToArray();
        }

        private void PlotRefresh()
        {
            _plot1.DiscreteFunction = new DiscreteFunction2D(FourierSobolevHaar.Calc(pWithZero), 0, 1, lengthX);
            _plot2.DiscreteFunction = new DiscreteFunction2D(SobolevHaarLinearCombination.FastCalc(p), 0, 1, lengthX);

            _plot1.Refresh();
            _plot2.Refresh();
        }
    }
}
