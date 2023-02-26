using NUnit.Framework;
using NuvTools.Common.Strings;

namespace NuvTools.Common.Tests.Strings
{
    [TestFixture()]
    public class StringExtensions
    {
        [Test()]
        public void FormatTest()
        {
            Assert.AreEqual("{0} Company".Format("Nuv Tools"), "Nuv Tools Company");
        }

        [Test()]
        public void LeftTest()
        {
            Assert.AreEqual("Nuv Tools".Left(3), "Nuv");

            Assert.AreEqual("Nuv Tools".Left(10), "Nuv Tools");
        }

        [Test()]
        public void RightTest()
        {
            Assert.AreEqual("Nuv Tools".Right(5), "Tools");

            Assert.AreEqual("Nuv Tools".Right(10), "Nuv Tools");
        }

        [Test()]
        public void RemoveDiacriticsTest()
        {
            Assert.AreEqual("Sáo Pãulo".RemoveDiacritics(), "Sao Paulo");
        }
    }
}
