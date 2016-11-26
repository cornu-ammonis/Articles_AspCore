using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestLibrary
{
    [TestFixture]
    public class TestClass
    {
        public TestClass()
        {
        }

        [TestCase]
        public void firstTest()
        {
            Assert.That(2, Is.EqualTo(1 + 1));
        }
    }
}
