﻿using System;
using System.Runtime.InteropServices.ComTypes;

namespace mathlib
{
    public static class Matrix
    {
        public static T[,] Adjoint<T>(T[,] matrix)
        {
            var n = matrix.GetLength(0);
            var m = matrix.GetLength(1);
            var adj = new T[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    adj[i, j] = matrix[j, i];
                }
            }
            return adj;
        }

        public static double[,] Mul(double[,] a, double[,] b)
        {
            // check matrices consistency
            var n = a.GetLength(0);
            var m = a.GetLength(1);
            if (m != b.GetLength(0))
                throw new Exception("Inconsistent matrices"); // replace with special type exception
            var k = b.GetLength(1);
            var ab = new double[n, k];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    var v = 0.0;
                    for (int t = 0; t < m; t++)
                    {
                        v += a[i, t]*b[t, j];
                    }
                    ab[i, j] = v;
                }
            }
            return ab;
        }
    }
}