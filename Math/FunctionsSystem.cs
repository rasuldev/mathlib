using mathlib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using RealFunction = System.Func<double, double>;

namespace mathlib
{
    public interface IFunctionsSystem
    {
        Segment OrthogonalitySegment { get; }
        RealFunction Get(int k);
        RealFunction GetWeighted(int k);
        IEnumerable<double> GetValuesOnNet(int k, double[] nodes);
        IEnumerable<double> GetWeightedValuesOnNet(int k, double[] nodes);
    }

    public abstract class FunctionsSystem : IFunctionsSystem
    {
        public abstract RealFunction Get(int k);
        public abstract Segment OrthogonalitySegment { get; }
        public virtual IEnumerable<double> GetValuesOnNet(int k, double[] nodes) =>
            nodes.Select(Get(k));

        //for weighted functions systems
        public RealFunction weight = OrthogonalWeights.UniteWeight;
        public virtual RealFunction GetWeighted(int k) =>
                                                          (x => Get(k)(x) * weight(x));
        public virtual IEnumerable<double> GetWeightedValuesOnNet(int k, double[] nodes) =>
            nodes.Select(GetWeighted(k));
    }

    public class HaarSystem : FunctionsSystem
    {
        public override Segment OrthogonalitySegment => new Segment(0, 1);

        public override RealFunction Get(int k)
        {
            return MixHaar.Haar(++k);
        }

        public override RealFunction GetWeighted(int k) => Get(k);
    }

    public class SobolevHaarSystem : FunctionsSystem
    {
        public override Segment OrthogonalitySegment => new Segment(0, 1);

        public override RealFunction Get(int k)
        {
            return MixHaar.MixedHaar1(++k);
        }

        public override RealFunction GetWeighted(int k) => Get(k);
    }

    public class SobolevWalshSystem : FunctionsSystem
    {
        public override Segment OrthogonalitySegment => new Segment(0, 1);

        public override RealFunction Get(int k)
        {
            return WalshSobolev.Get2(k);
        }

        public override RealFunction GetWeighted(int k) => Get(k);
    }

    /*      *****************************************         */
    /*      Cosine system and associated Sobolev sys.         */

    public class CosSystem : FunctionsSystem
    {
        private static readonly double Sqrt2 = Math.Sqrt(2);

        public override RealFunction Get(int k)
        {
            if (k == 0)
                return x => 1;
            return x => Sqrt2 * Math.Cos(k * Math.PI * x);
        }

        public override Segment OrthogonalitySegment => new Segment(0, 1);

        public override RealFunction GetWeighted(int k) => Get(k);
    }

    public class SobolevCosSystem : FunctionsSystem
    {
        private static readonly double Sqrt2OverPi = Math.Sqrt(2.0) / Math.PI;
        public override RealFunction Get(int k)
        {
            switch (k)
            {
                case 0: return x => 1;
                case 1: return x => x;
                default:
                    k -= 1;
                    return x => Sqrt2OverPi * Math.Sin(k * Math.PI * x) / k;
            }
        }

        public override Segment OrthogonalitySegment => new Segment(0, 1);

        public override RealFunction GetWeighted(int k) => Get(k);
    }




    /*      ***************************************************         */
    /*      Chebyshev 1 kind system and associated Sobolev sys.         */

    /// <summary>
    /// Chebyshev polynomials orthonormal on (-1,1) with weight \mu(x)=\frac2\pi(1-x^2)^{-\frac12}
    /// </summary>
    public class Cheb1SystemMF : FunctionsSystem
    {
        private static readonly double OneOverSqrt2 = 1.0 / Math.Sqrt(2.0);

        public override RealFunction Get(int k) {
            if (k == 0)
                return x => OneOverSqrt2;
            return x => Math.Cos(k * Math.Acos(x));
        }

        //public override Segment OrthogonalitySegment => new Segment(-1, 1);
        private double eps = 1E-5;
        public override Segment OrthogonalitySegment => new Segment(-1 + eps, 1 - eps);


        //weights and stuff

        new public RealFunction weight = OrthogonalWeights.Cheb1WeightMF;

        public override RealFunction GetWeighted(int k)
        {
            if (k == 0)
                return x => OneOverSqrt2 * weight(x);
            return x => Math.Cos(k * Math.Acos(x)) * weight(x);
        }

    }

    /// <summary>
    /// Sobolev orthogonal polynomials, associated with Chebyshev polynomials 
    /// orthonormal on (-1,1) with weight \mu(x)=\frac2\pi(1-x^2)^{-\frac12}
    /// </summary>
    public class SobolevCheb1SystemMF : FunctionsSystem
    {
        private static readonly double OneOverSqrt2 = 1.0 / Math.Sqrt(2.0);
        private static readonly FunctionsSystem chebSystem = new Cheb1SystemMF();

        public override RealFunction Get(int k)
        {
            switch (k) {
                case 0: return x => 1.0;
                case 1: return x => (1.0 + x) * OneOverSqrt2;
                case 2: return x => (x * x - 1.0) * 0.5;
                default:
                    var Tk = chebSystem.Get(k); //T[k] -- Chebyshev polynomial
                    var Tk2 = chebSystem.Get(k - 2); //T[k-2] -- Chebyshev polynomial
                    if (k % 2 == 0) {
                        return x =>
                            Tk(x) * 0.5 / (double)k - Tk2(x) * 0.5 / (k - 2.0) + 1.0 / (k * k - 2.0 * k);
                    } else {
                        return x =>
                            Tk(x) * 0.5 / (double)k - Tk2(x) * 0.5 / (k - 2.0) - 1.0 / (k * k - 2.0 * k);
                    }
            }
        }

        //public override Segment OrthogonalitySegment => new Segment(-1, 1);
        private double eps = 1E-5;
        public override Segment OrthogonalitySegment => new Segment(-1 + eps, 1 - eps);

        //weights and stuff
        new public RealFunction weight = OrthogonalWeights.Cheb1WeightMF;
    }




    public class Cheb1SystemMF_weighted : FunctionsSystem
    {
        private static readonly double Sqrt2OverPi = Math.Sqrt(2.0) / Math.PI;
        private static readonly double TwoOverPi = 2.0 / Math.PI;

        public override RealFunction Get(int k)
        {
            if (k == 0)
                return x => Sqrt2OverPi / Math.Sqrt(1.0 - x * x);

            return x => TwoOverPi * Math.Cos(k * Math.Acos(x)) / Math.Sqrt(1.0 - x * x);
        }

        //public override Segment OrthogonalitySegment => new Segment(-1, 1);
        private double eps = 1E-5;
        public override Segment OrthogonalitySegment => new Segment(-1 + eps, 1 - eps);


        //weights and stuff

        new public RealFunction weight = OrthogonalWeights.UniteWeight;

        public override RealFunction GetWeighted(int k) => Get(k);

    }





    /// <summary>
    /// Chebyshev polynomials orthonormal on (-1,1) with weight \mu(x)=\frac2\pi(1-x^2)^{-\frac12}
    /// by recurrent formulas.
    /// </summary>
    public class Cheb1SystemMF_rec : FunctionsSystem
    {
        private static readonly double OneOverSqrt2 = 1.0 / Math.Sqrt(2.0);

        public override RealFunction Get(int k)
        {
            switch (k) {
                case 0:
                    return x => OneOverSqrt2;
                case 1:
                    return x => x;
                default:
                    return x => 2.0 * x * Get(k - 1)(x) - Get(k - 2)(x);
            }

        }

        //public override Segment OrthogonalitySegment => new Segment(-1, 1);
        private double eps = 1E-5;
        public override Segment OrthogonalitySegment => new Segment(-1 + eps, 1 - eps);


        //weights and stuff

        new public RealFunction weight = OrthogonalWeights.Cheb1WeightMF;

        public override RealFunction GetWeighted(int k)
        {
            if (k == 0)
                return x => OneOverSqrt2 * weight(x);
            return x => Math.Cos(k * Math.Acos(x)) * weight(x);
        }

    }




    /// <summary>
    /// 
    /// </summary>
    public class LegendreSystem : IFunctionsSystem
    {
        public Segment OrthogonalitySegment => new Segment(-1, 1);

        public RealFunction Get(int k)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<double> GetValuesOnNet(int k, double[] nodes)
        {
            throw new NotImplementedException();
        }

        public RealFunction GetWeighted(int k)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<double> GetWeightedValuesOnNet(int k, double[] nodes)
        {
            throw new NotImplementedException();
        }
    }








}