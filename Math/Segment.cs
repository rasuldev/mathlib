using System.Linq;

namespace mathlib
{
    public struct Segment
    {
        public double Start;
        public double End;

        public Segment(double start, double end)
        {
            Start = start;
            End = end;
        }

        public double Length => End - Start;

        public double[] GetUniformPartition(int nodesCount)
        {
            var h = Length / (nodesCount - 1);
            var a = Start;
            return Enumerable.Range(0, nodesCount).Select(j => a + j * h).ToArray();
        }

        public void Deconstruct(out double start, out double end)
        {
            start = Start;
            end = End;
        }
    }
}