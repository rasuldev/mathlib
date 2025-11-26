using mathlib;
using NUnit.Framework;

namespace Tests
{
    public class FunctionsSystemTests
    {
        [Test]
        public void CosSytemOrthonormality()
        {
            var cosSystem = new CosSystem();
            for (int i = 0; i < 100; i++)
            {
                var i1 = i;
                var v = Integrals.Trapezoid(x => cosSystem.Get(i1)(x) * cosSystem.Get(i1)(x), 0, 1, 10000);
                Assert.That(1, Is.EqualTo(v).Within(0.0000001));
            }

            var val = Integrals.Trapezoid(x => cosSystem.Get(0)(x) * cosSystem.Get(5)(x), 0, 1, 10000);
            Assert.That(0, Is.EqualTo(val).Within(0.0000001));

            val = Integrals.Trapezoid(x => cosSystem.Get(4)(x) * cosSystem.Get(5)(x), 0, 1, 10000);
            Assert.That(0, Is.EqualTo(val).Within(0.0000001));
        }
    }
}