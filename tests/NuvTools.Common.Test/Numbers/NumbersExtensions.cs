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

        [Test()]
        public void ParseToShortOrNull()
        {
            Assert.That("".ParseToShortOrNull() is null);
            Assert.That(11 == "11".ParseToShortOrNull());
            Assert.That("40000".ParseToShortOrNull() is null);
        }

        [Test()]
        public void ParseToDoubleOrNull()
        {
            Assert.That("".ParseToDoubleOrNull() is null);
            Assert.That(((string?)null).ParseToDoubleOrNull() is null);
            Assert.That("Text".ParseToDoubleOrNull() is null);
            Assert.That(0 == "Text".ParseToDoubleOrNull(true));
        }

        // Without a provider the current culture decides the separator: in pt-BR the dot is the
        // thousands separator, so "3.14" is 314. That is the behaviour the overload keeps.
        [Test()]
        [SetCulture("pt-BR")]
        public void WithoutProviderTheCurrentCultureDecidesTheSeparator()
        {
            Assert.That("3.14".ParseToDecimalOrNull(), Is.EqualTo(314m));
            Assert.That("3,14".ParseToDecimalOrNull(), Is.EqualTo(3.14m));
            Assert.That("-23,55".ParseToDoubleOrNull(), Is.EqualTo(-23.55));
        }

        // Machine-written text reads the same whatever culture the process runs in.
        [Test()]
        [SetCulture("pt-BR")]
        public void WithInvariantProviderTheDotIsTheDecimalSeparator()
        {
            var invariant = System.Globalization.CultureInfo.InvariantCulture;

            Assert.That("1521.202".ParseToDecimalOrNull(invariant), Is.EqualTo(1521.202m));
            Assert.That("-23.56287".ParseToDoubleOrNull(invariant), Is.EqualTo(-23.56287));
            Assert.That("42".ParseToIntOrNull(invariant), Is.EqualTo(42));
            Assert.That("42".ParseToLongOrNull(invariant), Is.EqualTo(42L));
            Assert.That("11".ParseToShortOrNull(invariant), Is.EqualTo((short)11));
            Assert.That("".ParseToDecimalOrNull(invariant) is null);
            Assert.That(0 == "Text".ParseToDoubleOrNull(invariant, returnZeroIsNull: true));
        }
    }
}
