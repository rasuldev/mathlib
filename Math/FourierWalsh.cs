using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mathlib.Functions;
using static System.Math;

namespace mathlib
{
    public class FourierWalsh
    {
        /// <summary>
        /// Partial sum max order
        /// </summary>
        private int _maxOrder;
        /// <summary>
        /// Values of Walsh functions 
        /// </summary>
        private byte[,] _walshMatrix;

        public FourierWalsh(int maxOrder)
        {
            _maxOrder = maxOrder;

        }

        

        //public static double Coeff(Func<double, double> f, int k)
        //{
        //    Walsh.Get(k)
        //}
    }
}
