using NUnit.Framework;
using NuvTools.Common.Web;
using System;
using System.Collections.Generic;

namespace NuvTools.Common.Tests.Web;

public class QueryStringTest
{
    public enum EnumFake
    {
        Option1 = 0,
        Option2 = 1,
        Option3 = 2
    }

    public class FakeTest
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTimeOffset? DateTimeOffset { get; set; }    
        public string Name { get; set; }
        public List<long> Codes { get; set; }

        public EnumFake? EnumFake { get; set; }
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetQueryString()
    {
        var obj = new FakeTest
        {
            Date = new DateTime(2023, 1, 1, 12, 0, 0, 0),
            Id = 1,
            Name = "Hello World!",
            Codes = new List<long> { 1, 2, 3 }
        };

        var queryString = obj.GetQueryString("https://nuvtools.com");
        Assert.That(queryString, Is.EqualTo("https://nuvtools.com?Id=1&Date=2023-01-01T12%3A00%3A00&Name=Hello%20World!&Codes=1&Codes=2&Codes=3"));
    }

    [Test]
    public void GetQueryStringDateTimeOffset()
    {
        var obj = new FakeTest
        {
            DateTimeOffset = new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero),
            Id = 1,
            Name = "Hello World!",
            Codes = new List<long> { 1, 2, 3 }
        };

        var queryString = obj.GetQueryString("https://nuvtools.com");

        Assert.That(queryString, Is.EqualTo("https://nuvtools.com?Id=1&DateTimeOffset=2023-01-01T12%3A00%3A00&DateTimeOffsetOffset=00%3A00&Name=Hello%20World!&Codes=1&Codes=2&Codes=3"));
    }


    [Test]
    public void GetQueryString2()
    {
        var obj = new FakeTest
        {
            Date = new DateTime(2023, 1, 1, 12, 0, 0),
            Id = 1,
            Name = "Hello World!",
            Codes = new List<long> { 1, 2, 3 }
        };

        var queryString = obj.GetQueryString();
        Assert.That(queryString, Is.EqualTo("?Id=1&Date=2023-01-01T12%3A00%3A00&Name=Hello%20World!&Codes=1&Codes=2&Codes=3"));
    }

    [Test]
    public void GetQueryStringEnum()
    {
        var obj = new FakeTest
        {
            Date = new DateTime(2023, 1, 1, 12, 0, 0),
            Id = 1,
            Name = "Hello World!",
            Codes = [1, 2, 3],
            EnumFake = EnumFake.Option2
        };

        var queryString = obj.GetQueryString();
        Assert.That(queryString, Is.EqualTo("?Id=1&Date=2023-01-01T12%3A00%3A00&Name=Hello%20World!&Codes=1&Codes=2&Codes=3&EnumFake=1"));
    }

    [Test]
    public void ParseQueryString()
    {
        var parsedQueryString = "https://nuvtools.com?Id=1&Date=2023-01-01T12%3A00%3A00&Name=Hello%20World!&Codes=1&Codes=2&Codes=3&Exists".ParseQueryString();
        Assert.That(parsedQueryString.Count, Is.EqualTo(6));
    }

    [Test]
    public void ParseQueryString2()
    {
        var parsedQueryString = "Id=1&Date=2023-01-01T12%3A00%3A00&Name=Hello%20World!&Codes=1&Codes=2&Codes=3&Exists".ParseQueryString();
        Assert.That(parsedQueryString.Count, Is.EqualTo(5));
    }

    [Test]
    public void ParseQueryString3()
    {
        var parsedQueryString = "https://nuvtools.com/".ParseQueryString();
        Assert.That(parsedQueryString.Count, Is.EqualTo(1));
    }

    [Test]
    public void ParseQueryString4()
    {
        var parsedQueryString = "".ParseQueryString();
        Assert.That(parsedQueryString.Count, Is.EqualTo(0));
    }
}