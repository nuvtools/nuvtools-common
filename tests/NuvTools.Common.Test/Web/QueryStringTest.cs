using NUnit.Framework;
using NuvTools.Common.Web;
using System;
using System.Collections.Generic;

namespace NuvTools.Common.Tests.Web;

public class QueryStringTest
{
    public class FakeTest
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public List<long> Codes { get; set; }
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var obj = new FakeTest
        {
            Date = new DateTime(2023, 1, 1),
            Id = 1,
            Name = "Hello World!",
            Codes = new List<long> { 1, 2, 3 }
        };

        var queryString = obj.GetQueryString("https://nuv.tools");
        Assert.That(queryString, Is.EqualTo("https://nuv.tools?Id=1&Date=2023-01-01T12%3A00%3A00&Name=Hello%20World!&Codes=1&Codes=2&Codes=3"));
    }
}