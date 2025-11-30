using mathlib;
using mathlib.Polynomials;
using NUnit.Framework;

namespace Tests.Polynomials
{
    [TestFixture]
    public class LegendreSobolevTests
    {
        [Test]
        public void OrthonormalityTest()
        {
            int maxN = 15; // Test up to degree 4
            double tolerance = 1e-3; // Tolerance for numerical integration

            for (int i = 0; i < maxN; i++)
            {
                var p_i = LegendreSobolev.Get(i);
                for (int j = 0; j < maxN; j++)
                {
                    var p_j = LegendreSobolev.Get(j);
                    double innerProduct = InnerProd.SobolevLegendre(p_i, p_j, nodesCount: 2000);

                    if (i == j)
                    {
                        Assert.That(innerProduct, Is.EqualTo(1.0).Within(tolerance), $"Norm of P_{i} should be 1");
                    }
                    else
                    {
                        Assert.That(innerProduct, Is.EqualTo(0.0).Within(tolerance), $"Inner product of P_{i} and P_{j} should be 0");
                    }
                }
            }
        }
    }
}
