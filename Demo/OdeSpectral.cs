﻿using System;
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

namespace Demo
{
    public partial class OdeSpectral : GraphBuilder2DForm
    {
        MultiPlot2D exactSolutionPlot = new MultiPlot2D(name: "Точное решение");
        MultiPlot2D numSolutionPlotIter = new MultiPlot2D(name: "Численное решение итерационным методом");
        MultiPlot2D numSolutionPlotIter2 = new MultiPlot2D(name: "Численное решение итерационным методом2");

        private static Color[] Colors = new[]
        {
            Color.Red, Color.DarkGreen
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
            var cosSystem = new CosSystem();
            var sobCosSystem = new SobolevCosSystem();

            var solverIter = new CosSpectralSolverIter(1000);
            var problem = new CauchyProblem(f, y0, segment);
            var df = solverIter.Solve(problem, chunksCount, partSumOrder, iterCount, nodesCount);
            //df.X = df.X.Select(x => x).ToArray();
            numSolutionPlotIter.Colors = Colors;
            numSolutionPlotIter.DiscreteFunctions = df.Select(d => d[0]).ToArray();
            numSolutionPlotIter.Refresh();
            //numSolutionPlotIter2.DiscreteFunctions = df[1];
            //numSolutionPlotIter2.Refresh();
        }





        void SolveSystem(int partSumOrder, int iterCount, int nodesCount)
        {
            var chunksCount = (int)nupChunksCount.Value;
            var (segment, initVals, f, h, yExact) = ExampleSystem6();
            var nodes = Range(0, nodesCount).Select(j => segment.Start + segment.Length * j / (nodesCount - 1)).ToArray();
            if (yExact != null)
            {
                exactSolutionPlot.DiscreteFunctions =
                    yExact.Select(y => new DiscreteFunction2D(y, nodes)).ToArray();
                exactSolutionPlot.Refresh();
            }

            var cosSystem = new CosSystem();
            var sobCosSystem = new SobolevCosSystem();

            var solverIter = new CosSpectralSolverIter(1000);
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
            Solve((int)nupOrder.Value, (int)nupIterCount.Value, (int)nupNodesCount.Value);
            //SolveSystem((int)nupOrder.Value, (int)nupIterCount.Value, (int)nupNodesCount.Value);
        }


    }

    //public static class ArrayExts
    //{
    //    public static void Deconstruct(this double[] arr, params out double[] x)
    //    {
    //        x = arr[0];
    //        y = arr[1];
    //    }
    //}
}
