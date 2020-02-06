using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscreteFunctions;
using DiscreteFunctionsPlots;
using Endless;
using GraphBuilders;
using mathlib;
using mathlib.DiffEq;
using Steema.TeeChart.Styles;
using static System.Math;
using static System.Linq.Enumerable;
using static Demo.OdeExamples;
using System.Collections.Concurrent;

namespace Demo
{
    public partial class OdeSpectral : GraphBuilder2DForm
    {
        MultiPlot2D exactSolutionPlot = new MultiPlot2D(name: "Точное решение");
        MultiPlot2D numSolutionPlotIter = new MultiPlot2D(name: "Численное решение итерационным методом");
        MultiPlot2D numSolutionPlotIter2 = new MultiPlot2D(name: "Численное решение итерационным методом 2");

        private static Color[] Colors = new[]
        {
            Color.Red
        };

        public OdeSpectral()
        {
            InitializeComponent();
            GraphBuilder.DrawPlot(exactSolutionPlot);
            GraphBuilder.DrawPlot(numSolutionPlotIter);
            //GraphBuilder.DrawPlot(numSolutionPlotIter2);
        }



        void Solve(int partSumOrder, int iterCount, int nodesCount)
        {
            var chunksCount = (int)nupChunksCount.Value;
            var (segment, y0, f, yExact) = Example3();
            var nodes = Range(0, nodesCount).Select(j => segment.Start + segment.Length * j / (nodesCount - 1)).ToArray();
            if (yExact != null)
            {
                //nodes = Range(0, nodesCount).Select(j => 1.0*j / nodesCount).ToArray();
                exactSolutionPlot.DiscreteFunctions = new[] { new DiscreteFunction2D(x => yExact(x), nodes) };
                exactSolutionPlot.Refresh();
            }
            //nodes = Range(0, nodesCount).Select(j => segment.Start + segment.Length * j / nodesCount).ToArray();

            // *************** Cos System: ***************
            /*var cosSystem = new CosSystem();
            var sobCosSystem = new SobolevCosSystem();
            var solverIter = new SobolevSpectralSolverIter(1000, cosSystem, sobCosSystem);*/
            //var solverIter = new CosSpectralSolverIter(1000);

            // *************** Haar System: ***************
            var solverIter = new HaarSpectralSolverIter(1000);

            // *************** Chebyshev 1 MF System:  ***************
            //var cheb1SystemMF = new Cheb1SystemMF_rec();
            //var cheb1SystemMF = new Cheb1SystemMF();
            //var sobCheb1SystemMF = new SobolevCheb1SystemMF();
            //var solverIter = new SobolevSpectralSolverIter(1000, cheb1SystemMF, sobCheb1SystemMF, true);
            //var cheb1SystemMF2 = new Cheb1SystemMF_weighted();
            //var solverIter = new SobolevSpectralSolverIter(1000, cheb1SystemMF2, sobCheb1SystemMF);

            var problem = new CauchyProblem(f, y0, segment);
            var df = solverIter.Solve(problem, chunksCount, partSumOrder, iterCount, nodesCount);
            //df.X = df.X.Select(x => x).ToArray();
            numSolutionPlotIter.Colors = Colors;
            numSolutionPlotIter.DiscreteFunctions = df.Select(d => d[0]).ToArray();
            numSolutionPlotIter.Refresh();
            //numSolutionPlotIter2.DiscreteFunctions = df[1];
            //numSolutionPlotIter2.Refresh();
        }

        double SolveWalsh(int partSumOrder, int iterCount, int nodesCount, int chunksCount)
        {
            var (segment, y0, f, yExact) = ExampleDiscontinuous4();
            var nodes = Range(0, nodesCount).Select(j => segment.Start + segment.Length * j / (nodesCount - 1)).ToArray();
            if (yExact != null)
            {
                //nodes = Range(0, nodesCount).Select(j => 1.0*j / nodesCount).ToArray();
                exactSolutionPlot.DiscreteFunctions = new[] { new DiscreteFunction2D(x => yExact(x), nodes) };
                exactSolutionPlot.Refresh();
            }
            //nodes = Range(0, nodesCount).Select(j => segment.Start + segment.Length * j / nodesCount).ToArray();

            // *************** Walsh System: ***************
            var solverIter = new WalshSpectralSolverIter(1000);


            var problem = new CauchyProblem(f, y0, segment);
            var df = solverIter.Solve(problem, chunksCount, partSumOrder, iterCount, nodesCount);
            //df.X = df.X.Select(x => x).ToArray();
            numSolutionPlotIter.Colors = Colors;
            numSolutionPlotIter.DiscreteFunctions = df.Select(d => d[0]).ToArray();
            numSolutionPlotIter.Refresh();
            //numSolutionPlotIter2.DiscreteFunctions = df[1];
            //numSolutionPlotIter2.Refresh();

            var delta = exactSolutionPlot.DiscreteFunctions[0].Y
                .Zip(numSolutionPlotIter.DiscreteFunctions[0].Y)
                .Select(y => Abs(y.Item1 - y.Item2)).Max();

            label5.Text = $"{delta:F5}";
            return delta;
        }




        void SolveSystem(int partSumOrder, int iterCount, int nodesCount)
        {
            var chunksCount = (int)nupChunksCount.Value;
            var (segment, initVals, f, h, yExact) = ExampleSystem7();
            var nodes = Range(0, nodesCount).Select(j => segment.Start + segment.Length * j / (nodesCount - 1)).ToArray();
            if (yExact != null)
            {
                exactSolutionPlot.DiscreteFunctions =
                    yExact.Select(y => new DiscreteFunction2D(y, nodes)).ToArray();
                exactSolutionPlot.Refresh();
            }

            var cosSystem = new CosSystem();
            var sobCosSystem = new SobolevCosSystem();

            //var solverIter = new CosSpectralSolverIter(1000);

            // *************** Haar System: ***************
            var solverIter = new HaarSpectralSolverIter(1000);

            var problem = new CauchyProblem(f, initVals, segment);
            var solution = solverIter.Solve(problem, chunksCount, partSumOrder, iterCount, nodesCount);
            var dfs = new List<DiscreteFunction2D>();
            foreach (var s in solution)
            {
                dfs.AddRange(s);
            }
            numSolutionPlotIter.Colors = Colors;
            numSolutionPlotIter.DiscreteFunctions = dfs.ToArray();
            numSolutionPlotIter.Refresh();
            //var deltas = solution.Zip(yExact, (df, y) => Abs(df.Y.Last() - y(df.X.Last()))).ToArray();
            //var deltas = solution.Zip(yExact, (df, y) =>
            //    {
            //        var eDf = new DiscreteFunction2D(y, df.X);
            //        return Sqrt((df - eDf).Y.Average(v => v * v));
            //    }
            //    ).ToArray();
            //Trace.WriteLine($"iter={iterCount}; N={partSumOrder}; dy1={deltas[0]};dy2={deltas[1]}");

        }

        private void ValueChanged(object sender, EventArgs e)
        {
            //Solve((int)nupOrder.Value, (int)nupIterCount.Value, (int)nupNodesCount.Value);
            //SolveSystem((int)nupOrder.Value, (int)nupIterCount.Value, (int)nupNodesCount.Value);
            var chunksCount = (int)nupChunksCount.Value;
            SolveWalsh((int)nupOrder.Value, (int)nupIterCount.Value, (int)nupNodesCount.Value, chunksCount);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //tbLog.Text += $"order {partSumOrder}; nodes {nodesCount};\r\n";
            //tbLog.Text += $"iter; delta;\r\n";
        }

        private void RunExperiment(int partSumOrder, int startIter, int endIter)
        {
            const int nodesCount = 100;

            var dict = new ConcurrentDictionary<int, double>();
            Parallel.For(startIter, endIter, iter =>
            {
                var delta = SolveWalsh(partSumOrder, iter, nodesCount, 1);
                dict[iter] = delta;
            });

            foreach (var item in dict.OrderBy(el => el.Key))
            {
                tbLog.Text += $"{item.Key}; {item.Value};\r\n";
            }
        }
    }


    public class Experiment
    {
        public int Order { get; set; }
        public int IterationsCount { get; set; }
        public double Delta { get; set; }

        public Experiment(int order, int iterationsCount, double delta)
        {
            Order = order;
            IterationsCount = iterationsCount;
            Delta = delta;
        }
    }


}
