using System;
using System.Collections.Generic;
using System.Linq;

namespace mathlib
{
    public interface IOrthogonalWeight
    {
        Segment OrthogonalitySegment { get; }
        Func<double, double> Get();
        IEnumerable<double> GetValuesOnNet(double[] nodes);
    }

    public abstract class OrthogonalWeight : IOrthogonalWeight
    {
        public abstract Func<double, double> Get();
        public abstract Segment OrthogonalitySegment { get; }
        public virtual IEnumerable<double> GetValuesOnNet(double[] nodes) =>
            nodes.Select(Get());
    }
    


}