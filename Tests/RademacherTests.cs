using mathlib.Functions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class RademacherTests
    {
           
        [Test]
        public void RademacherFuncTest()
        {
            var f = Rademacher.Get(1);
            Assert.AreEqual(1, f(0.2));
            Assert.AreEqual(-1, f(0.4));
            Assert.AreEqual(1, f(0.6));
            Assert.AreEqual(-1, f(0.9));
        }
    }
}
