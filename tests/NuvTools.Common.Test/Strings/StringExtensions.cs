using NUnit.Framework;
using NuvTools.Common.Strings;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NuvTools.Common.Tests.Strings
{
    [TestFixture()]
    public class StringExtensions
    {
        [Test()]
        public void FormatTest()
        {
            Assert.That("{0} Company {year} and profit {value:N2}".Format(new CultureInfo("en-US"), "Nuv Tools", 2023, 64.2893) == "Nuv Tools Company 2023 and profit 64.29");
        }

        [Test()]
        public void FormatDictionaryTest()
        {
            string value = "{0} testing {something} useful to {framework} and {something} " +
                        "again on {holiday:dd/MM/yyyy} and {notindictionary}. Only for {value:N2}";

            Dictionary<string, object> variables = new(){
                { "0", "Bruno" },
                { "something", "Format Function" },
                { "framework", "Nuv Tools" },
                { "holiday", new DateTime(2023, 11,02,10,11,12) },
                { "value", 45.3532 }
            };

            Assert.That(value.Format(variables, new CultureInfo("en-US")) == "Bruno testing Format Function useful to Nuv Tools and Format Function again on 02/11/2023 and . Only for 45.35");
        }

        [Test()]
        public void FormatDictionaryEmptyTest()
        {
            string value = "Nothing to replace";

            Dictionary<string, object> variables = new(){
                { "0", "Bruno" },
                { "something", "Format Function" },
                { "framework", "Nuv Tools" },
                { "holiday", new DateTime(2023, 11,02,10,11,12) },
                { "value", 45.3532 }
            };

            Assert.That(value.Format(variables) == value);

            Assert.Throws<ArgumentException>(() => "".Format(new Dictionary<string, object>()));
            Assert.Throws<ArgumentNullException>(() => value.Format(new Dictionary<string, object>()));
            Assert.Throws<ArgumentNullException>(() => value.Format(null, new CultureInfo("pt-BR")));
        }

        [Test()]
        public void FormatDictionaryCultureTest()
        {
            string value = "Testing {value:N2} and index {1}";

            Dictionary<string, object> variables = new(){
                { "1", "Nuv Tools" },
                { "value", 52.38532 } //will be rounded
            };

            Assert.That(value.Format(variables, new CultureInfo("pt-BR")) == "Testing 52,39 and index Nuv Tools");
        }



        [Test()]
        public void LeftTest()
        {
            Assert.That("Nuv Tools".Left(3) == "Nuv");

            Assert.That("Nuv Tools".Left(10) == "Nuv Tools");
        }

        [Test()]
        public void RightTest()
        {
            Assert.That("Nuv Tools".Right(5) == "Tools");

            Assert.That("Nuv Tools".Right(10) == "Nuv Tools");
        }

        [Test()]
        public void RemoveDiacriticsTest()
        {
            Assert.That("Sáo Pãulo".RemoveDiacritics() == "Sao Paulo");
        }

        [Test()]
        public void GetNumbersOnlyTest()
        {
            Assert.That("(555) 333-4444".GetNumbersOnly() == "5553334444");
        }
    }
}
