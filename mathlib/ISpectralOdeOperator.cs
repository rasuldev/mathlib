namespace mathlib
{
    public interface ISpectralOdeOperator<T>
    {
        void SetParams(DynFunc<double>[] odeRightSides, double[] initialValues, int partialSumOrder);
        T GetValue(T x);
        Segment OrthogonalitySegment { get; set; }
    }
}