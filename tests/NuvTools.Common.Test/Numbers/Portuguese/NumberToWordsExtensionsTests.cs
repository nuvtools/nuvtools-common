using NUnit.Framework;
using NuvTools.Common.Numbers.Portuguese;
using System.Globalization;

namespace NuvTools.Common.Tests.Numbers.Portuguese
{
    [TestFixture()]
    public class NumberToWordsExtensionsTests
    {
        [Test()]
        public void ToWordsTest()
        {
            int number = 55;
            Assert.AreEqual("Cinquenta e cinco reais", number.ToWords());
        }

        [Test()]
        public void ToWordsTest1()
        {
            decimal number = 55.5M;
            Assert.AreEqual("Cinquenta e cinco reais e cinquenta centavos", number.ToWords());
        }

        [Test()]
        public void ToWordsTest2()
        {
            decimal number = 4000000000000000M;

            try
            {
                var test = number.ToWords();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(Resources.Messages.ResourceManager.GetString(nameof(Resources.Messages.ValueOutsideRange), CultureInfo.GetCultureInfo("pt-BR")), ex.Message);
            }
        }
    }
}