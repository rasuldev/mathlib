using System;
using System.Collections.Generic;
using System.Linq;
using mathlib;
using mathlib.Symbolic;
using NUnit.Framework;
using static System.Math;

namespace Tests
{
    [TestFixture]
    public class IntegralsTests
    {
        private List<TestCase> TestCases;

        [OneTimeSetUp]
        public void Setup()
        {
            TestCases = new List<TestCase>()
            {
                new TestCase(x => x, 0, 1, 0.5, 1000, 0.01),
                new TestCase(x => x, -1, 1, 0.0, 1000, 0.01),
                new TestCase(x => x, -1, 1, 0.0, 10000, 0.001),
                new TestCase(x => x, -1, 3, 4.0, 10000, 0.01),
                new TestCase(x =>
                {
                    if (x <= 0.25)
                        return 1;
                    if (x <= 0.5)
                        return -1;
                    return 0;
                }, 0, 1, 0, 500, 0.001)
            };

        }

        public void RunTestCases(Func<TestCase, double> testedFunc)
        {
            for (var i = 0; i < TestCases.Count; i++)
            {
                var testCase = TestCases[i];
                var value = testedFunc(testCase);
                Assert.That(testCase.Result, Is.EqualTo(value).Within(testCase.Delta), $"TestCase {i} failed");
            }
        }

        double[] GenerateNodes(double a, double b, int count)
        {
            var nodes = new double[count];
            for (int i = 0; i < count; i++)
            {
                nodes[i] = a + i * (b - a) / (count - 1);
            }
            return nodes;
        }

        [Test]
        public void RectangularLeft()
        {
            RunTestCases(testCase => Integrals.Rectangular(testCase.Func, testCase.A, testCase.B, testCase.NodesCount,
                    Integrals.RectType.Left));
        }

        [Test]
        public void RectangularRight()
        {
            RunTestCases(testCase => Integrals.Rectangular(testCase.Func, testCase.A, testCase.B, testCase.NodesCount,
                    Integrals.RectType.Right));
        }

        [Test]
        public void RectangularCenter()
        {
            RunTestCases(testCase => Integrals.Rectangular(testCase.Func, testCase.A, testCase.B, testCase.NodesCount,
                        Integrals.RectType.Center));
        }

        [Test]
        public void TrapezoidTest()
        {
            RunTestCases(
                testCase => Integrals.Trapezoid(testCase.Func, GenerateNodes(testCase.A, testCase.B, testCase.NodesCount))
            );
        }

        [Test]
        public void TrapezoidDiscreteTest()
        {
            RunTestCases(
                testCase =>
                {
                    var nodes = GenerateNodes(testCase.A, testCase.B, testCase.NodesCount);
                    var fD = nodes.Select(t => testCase.Func(t)).ToArray();
                    return Integrals.Trapezoid(fD, nodes);
                });
        }


        [Test]
        public void TrapezoidEquiDistantNetTest()
        {
            RunTestCases(
                testCase => Integrals.Trapezoid(testCase.Func, testCase.A, testCase.B, testCase.NodesCount)
            );
        }

        [Test]
        public void RectangularInfititeTest()
        {
            var integral = new Integral(t => Pow(E, -t), 0, double.PositiveInfinity);
            var result = Integrals.RectangularInfinite(integral, 100000, 10);
            Assert.That(result, Is.EqualTo(1).Within(0.0001));
        }

    }


    public struct TestCase
    {
        public Func<double, double> Func { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double Result { get; set; }
        public int NodesCount { get; set; }
        public double Delta { get; set; }

        public TestCase(Func<double, double> func, double a, double b, double result, int nodesCount, double delta)
        {
            Func = func;
            A = a;
            B = b;
            Result = result;
            NodesCount = nodesCount;
            Delta = delta;
        }
    }
}