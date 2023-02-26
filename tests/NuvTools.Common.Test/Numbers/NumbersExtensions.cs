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
            Assert.IsNull("".ParseToIntOrNull());
            Assert.IsNull("Text".ParseToIntOrNull());
            Assert.AreEqual(0, "Text".ParseToIntOrNull(true));
            Assert.AreEqual(1, "1".ParseToIntOrNull());
        }

        [Test()]
        public void ParseToLongOrNull()
        {
            Assert.IsNull("".ParseToLongOrNull());
            Assert.IsNull("0".ParseToLongOrNull(true));
            Assert.IsNotNull("0".ParseToLongOrNull());
        }
    }
}
