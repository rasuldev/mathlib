using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    public class Assert : NUnit.Framework.Assert
    {
        public const double Eps = 0.000001;
        public static void AreEqual(double[] expected, double[] actual, double delta = Eps)
        {
            That(expected.Length == actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                That(expected[i], Is.EqualTo(actual[i]).Within(delta));
            }
        }

        public static void AreEqual(double[,] expected, double[,] actual, double delta = Eps)
        {
            That(expected.GetLength(0) == actual.GetLength(0));
            That(expected.GetLength(1) == actual.GetLength(1));
            for (int i = 0; i < expected.GetLength(0); i++)
            {
                for (int j = 0; j < expected.GetLength(1); j++)
                {
                    That(expected[i, j], Is.EqualTo(actual[i, j]).Within(delta));
                }
            }
        }

        public static void AreEqual(int[] expected, int[] actual)
        {
            That(expected.Length == actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                That(expected[i], Is.EqualTo(actual[i]));
            }
        }

        public static void AreEqual(sbyte[,] expected, sbyte[,] actual)
        {
            That(expected.GetLength(0) == actual.GetLength(0));
            That(expected.GetLength(1) == actual.GetLength(1));
            for (int i = 0; i < expected.GetLength(0); i++)
            {
                for (int j = 0; j < expected.GetLength(1); j++)
                {
                    That(expected[i, j], Is.EqualTo(actual[i, j]));
                }
            }
        }

        public static void AreEqual(int a, int b) => That(a, Is.EqualTo(b));

        public static void AreEqual(double a, double b, double delta = Eps) => That(a, Is.EqualTo(b).Within(delta));
    }
}
