using NUnit.Framework;
using NuvTools.Common.Numbers;

namespace NuvTools.Common.Tests.Numbers
{
    [TestFixture()]
    public class NumbersExtensions
    {
        [Test()]
        public void ParseToIntOrNull()
        {
            Assert.That("".ParseToIntOrNull() is null);
            Assert.That("Text".ParseToIntOrNull() is null);
            Assert.That(0 == "Text".ParseToIntOrNull(true));
            Assert.That(1 == "1".ParseToIntOrNull());
        }

        [Test()]
        public void ParseToLongOrNull()
        {
            Assert.That("".ParseToLongOrNull() is null);
            Assert.That("0".ParseToLongOrNull(true) == 0);
        }
    }
}
