using System;

namespace mathlib
{
    public static class LinearSystem
    {
        /// <summary>
        /// Finds X vector in linear system AX=B. Rows count in matrix A must be equal to length of the vector B.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double[] Solve(double[,] A, double[] B)
        {
            var conjA = Matrix.Adjoint(A);
            var inv = Matrix.Inverse(Matrix.Mul(conjA, A));

            var matr = Matrix.Mul(inv, conjA);
            return Matrix.Mul(matr, B);
        }
    }
}