using NUnit.Framework;
using NuvTools.Common.Reflection;

namespace NuvTools.Common.Tests.Reflection
{
    [TestFixture()]
    public class AssemblyHelperTests
    {
        [Test()]
        public void ResourceByNameTest()
        {
            var resource = AssemblyHelper.ResourceByName("Assets.Image.bmp", "NuvTools.Common.Tests");
            Assert.IsNotNull(resource);
        }
    }
}