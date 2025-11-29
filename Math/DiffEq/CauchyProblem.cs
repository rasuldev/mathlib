using System;
using System.Security.AccessControl;

namespace mathlib.DiffEq
{
    /// <summary>
    /// Describes Cauchy problem for the first order ODE system on given segment:
    /// y_i'(x)=f_i(x,y_1,y_2,...,y_m), i=1,...,m.
    /// y_i(a), i=1,...,m
    /// </summary>
    public class CauchyProblem
    {
        /// <summary>
        /// Array of f_i
        /// </summary>
        public DynFunc<double>[] RightSides { get; set; }

        /// <summary>
        /// y_i(Segment.Start)
        /// </summary>
        public double[] InitialValues { get; set; }

        public Segment Segment { get; set; }

        public int EquationsCount => RightSides.Length;

        public CauchyProblem(DynFunc<double>[] rightSides, double[] initialValues, Segment segment)
        {
            RightSides = rightSides;
            // Right sides should be functions with EquationsCount+1 args
            foreach (var rightSide in rightSides)
            {
                if (rightSide.ArgsCount != EquationsCount + 1)
                    throw new ArgumentException("Right sides should be functions with rightSides.Length+1 arguments count");
            }
            InitialValues = initialValues;
            if (initialValues.Length != EquationsCount)
                throw new ArgumentException("initialValues should have rightSides.Length elements");

            Segment = segment;
        }

        /// <summary>
        /// Cauchy problem for one equation
        /// </summary>
        /// <param name="f"></param>
        /// <param name="initialValue"></param>
        /// <param name="segment"></param>
        public CauchyProblem(Func<double, double, double> f, double initialValue, Segment segment)
            : this(new[] { new DynFunc<double>(2, doubles => f(doubles[0], doubles[1])) }, 
                   new[] { initialValue }, 
                   segment) { }
    }
}