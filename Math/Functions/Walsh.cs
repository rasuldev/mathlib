using System;
using System.Linq;
using MoreLinq;
using static System.Math;

namespace mathlib.Functions
{
    public class Walsh
    {
        public static Func<double, double> Get(int n)
        {
            var binary = Common.ToBinary(n);

            double Func(double x)
            {
                var xBin = Common.ToBinary(x, binary.Length);
                var s = binary.EquiZip(xBin, (ei, ni) => ei * ni).Sum();
                return s % 2 == 0 ? 1 : -1;
            }

            return Func;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k">Should be greater than zero</param>
        /// <returns></returns>
        public static sbyte[,] GetMatrix(int k)
        {
            var prevMatrix = new sbyte[,] { { 1 } };
            var order = 0;
            while (order < k)
            {
                ++order;
                var prevLength = prevMatrix.GetLength(0);
                var nextMatrix = new sbyte[2 * prevLength, 2 * prevLength];
                for (int i = 0; i < prevLength; i++)
                {
                    for (int j = 0; j < prevLength; j++)
                    {
                        nextMatrix[2 * i, j] = nextMatrix[2 * i + 1, j] = nextMatrix[2 * i, prevLength + j] = prevMatrix[i, j];
                        nextMatrix[2 * i + 1, prevLength + j] = (sbyte)-prevMatrix[i, j];
                    }
                }

                prevMatrix = nextMatrix;
                if (order >= k) break;
            }
            return prevMatrix;
        }
    }
}