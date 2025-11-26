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
    public partial class OdeSpectralLegendre : GraphBuilder2DForm
    {
        MultiPlot2D exactSolutionPlot = new MultiPlot2D(name: "Точное решение");
        MultiPlot2D numSolutionPlotIter = new MultiPlot2D(name: "Численное решение итерационным методом");
        MultiPlot2D numSolutionPlotIter2 = new MultiPlot2D(name: "Численное решение итерационным методом 2");

        private static Color[] Colors = new[]
        {
            Color.Red
        };

        public OdeSpectralLegendre()
        {
            InitializeComponent();
            GraphBuilder.DrawPlot(exactSolutionPlot);
            GraphBuilder.DrawPlot(numSolutionPlotIter);
            //GraphBuilder.DrawPlot(numSolutionPlotIter2);
        }



        double Solve(int partSumOrder, int iterCount, int nodesCount, int chunksCount)
        {
            var (segment, y0, f, yExact) = Example3();
            var nodes = Range(0, nodesCount).Select(j => segment.Start + segment.Length * j / (nodesCount - 1)).ToArray();
            if (yExact != null)
            {
                exactSolutionPlot.DiscreteFunctions = new[] { new DiscreteFunction2D(x => yExact(x), nodes) };
                exactSolutionPlot.Refresh();
            }

            var legendreSystem = new CosSystem();
            var sobLegendreSystem = new SobolevCosSystem();
            var solverIter = new SobolevSpectralSolverIter(1000, legendreSystem, sobLegendreSystem);

            //var solverIter = new HaarSpectralSolverIter(1000);

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
            var chunksCount = (int)nupChunksCount.Value;
            Solve((int)nupOrder.Value, (int)nupIterCount.Value, (int)nupNodesCount.Value, chunksCount);
            //SolveSystem((int)nupOrder.Value, (int)nupIterCount.Value, (int)nupNodesCount.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const int partSumOrder = 10;
            tbLog.Text += $"order {partSumOrder};\r\n";
            tbLog.Text += $"iter; delta;\r\n";
            var experimentResults = new List<ExperimentResult>();
            experimentResults.AddRange(RunExperiment(partSumOrder, 1, 30));
            //experimentResults.AddRange(RunExperiment(15, 5, 30));
            //experimentResults.AddRange(RunExperiment(20, 5, 30));
            tbLog.Text += ToString(experimentResults);
        }

        private string ToString(IEnumerable<ExperimentResult> results)
        {
            var sb = new StringBuilder();

            foreach (var r in results.OrderBy(x => x.IterationsCount))
            {
                sb.AppendLine($"{r.IterationsCount};{r.Delta};");
            }

            return sb.ToString();
        }

        private IEnumerable<ExperimentResult> RunExperiment(int partSumOrder, int startIter, int endIter)
        {
            const int nodesCount = 100;

            var dict = new ConcurrentDictionary<int, double>();
            Parallel.For(startIter, endIter+1, iter =>
            {
                var delta = Solve(partSumOrder, iter, nodesCount, 1);
                dict[iter] = delta;
            });
            return dict.Select(i => new ExperimentResult(partSumOrder, i.Key, i.Value));
        }
    }
}
