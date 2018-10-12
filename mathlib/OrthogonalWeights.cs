using System;
using System.Collections.Generic;
using System.Linq;

namespace mathlib
{
    
    public static class OrthogonalWeights
    {
        private static readonly double Sqrt2OverPi = Math.Sqrt(2.0) / Math.PI;


        public static Func<double, double> Cheb1WeightMF
        {
            get
            {
                return x => Sqrt2OverPi / Math.Sqrt( 1.0 - x * x);
            }
        }

        public static Func<double, double> Cheb1Weight
        {
            get
            {
                return x => 1.0 / Math.Sqrt( 1.0 - x * x);
            }
        }

        public static Func<double, double> UniteWeight
        {
            get
            {
                return x => 1.0;
            }
        }

        public static Func<double, double> UniformWeight(double A = 1.0)
        {
            return x => A;
        }

    }






}