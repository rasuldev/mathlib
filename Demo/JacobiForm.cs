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
using mathlib.Polynomials;
using static System.Math;
using static System.Linq.Enumerable;

namespace Demo
{
    public partial class JacobiForm : GraphBuilder2DForm
    {
        Plot2D plot = new Plot2D("Jacobi");
        Plot2D plot2 = new Plot2D("Jacobi 2");
        Plot2D plotWeight = new Plot2D("Weight");
        Plot3D plotCore = new Plot3D("Core");
        Plot3D plotReducedCore = new Plot3D("Reduced core");

        public JacobiForm()
        {
            InitializeComponent();
            //GraphBuilder.DrawPlot(plot);
            //GraphBuilder.DrawPlot(plot2);
            GraphBuilder.Set3DMode();
            GraphBuilder.DrawPlot(plotCore);
            //GraphBuilder.DrawPlot(plotReducedCore);
        }

        void Draw()
        {
            var n = (int)nupOrder.Value;
            var alpha = (double) nupAlpha.Value;
            var beta = (double) nupBeta.Value;
            beta = alpha;
            var jac = new mathlib.Polynomials.Jacobi(alpha, beta);
            //plot.DiscreteFunction = new DiscreteFunction2D(x => jac.GetOrthonormalValue(n, x), -1, 1, 1000);
            //plot.Refresh();

            //plot2.DiscreteFunction = new DiscreteFunction2D(x => jac.GetOrthonormalValue(n+1, x), -1, 1, 1000);
            //plot2.Refresh();

            //plotWeight.DiscreteFunction = new DiscreteFunction2D(x => Math.Pow(1-x, alpha) * Math.Pow(1+x, beta), -1+.001, 1-0.01, 1000);
            //plotWeight.Refresh();

            var nodesCount = 100;
            var nodes = Range(0, nodesCount).Select(j => -1 + j * 1d / nodesCount).ToArray();
            var z = new double[nodesCount, nodesCount];
            var rz = new double[nodesCount, nodesCount];
            for (int i = 0; i < nodesCount; i++)
            {
                for (int j = 0; j < nodesCount; j++)
                {
                    z[i, j] = jac.GetOrthonormalValue(n, nodes[i]) * jac.GetOrthonormalValue(n, nodes[j]);
                    //z[i, j] = ValleePoussinCore(n, n, alpha, beta, nodes[i], nodes[j]);
                    //rz[i, j] = ValleePoussinReducedCore(n, n, alpha, beta, nodes[i], nodes[j]);
                }
            }
            
            plotCore.DiscreteFunction = new DiscreteFunction3D(nodes, nodes, z);
            plotCore.Refresh();

            //plotReducedCore.DiscreteFunction = new DiscreteFunction3D(nodes, nodes, rz);
            //plotReducedCore.Refresh();
        }

        double Core(int n, double alpha, double beta, double x, double t)
        {
            // to prevent division on zero
            if (Abs(x - t) < 0.00000001) return 0;

            var jac = new Jacobi(alpha, beta);
            return Sqrt(Jacobi.CalcLambdaCoeff(n, alpha, beta))
                   * (jac.GetOrthonormalValue(n + 1, x) * jac.GetOrthonormalValue(n, t) -
                      jac.GetOrthonormalValue(n + 1, t) * jac.GetOrthonormalValue(n, x))
                / (x - t);
        }

        double ValleePoussinCore(int n, int m, double alpha, double beta, double x, double t)
        {
            var s = 0d;
            for (int i = n; i <= n+m; i++)
            {
                s += Core(i, alpha, beta, x, t);
            }

            return s / (m + 1);
        }

        double ReducedCore(int n, double alpha, double beta, double x, double t)
        {
            // to prevent division on zero
            if (Abs(x - t) < 0.00000001) return 0;

            var jac = new Jacobi(alpha, beta);
            return Sqrt(Jacobi.CalcLambdaCoeff(n, alpha, beta))
                   * Abs(jac.GetOrthonormalValue(n + 1, x) * jac.GetOrthonormalValue(n, t) 
                      //-jac.GetOrthonormalValue(n + 1, t) * jac.GetOrthonormalValue(n, x)
                      )
                   / Abs(x - t);
        }

        double ValleePoussinReducedCore(int n, int m, double alpha, double beta, double x, double t)
        {
            var s = 0d;
            for (int i = n; i <= n + m; i++)
            {
                s += ReducedCore(i, alpha, beta, x, t);
            }

            return s / (m + 1);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void nupBeta_ValueChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void nupAlpha_ValueChanged(object sender, EventArgs e)
        {
            Draw();
        }
    }
}
