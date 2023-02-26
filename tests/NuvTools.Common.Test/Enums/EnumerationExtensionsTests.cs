namespace NuvTools.Common.Tests.Enums;

using NUnit.Framework;
using NuvTools.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

[TestFixture()]
public class EnumerationExtensionsTests
{
    [Test()]
    public void GetGroupNameLongTest()
    {
        var value = FormatTypeLong.PowerPoint.GetGroupName();
        Assert.AreEqual("Microsoft Office", value);

        var value2 = FormatTypeLong.Word.GetGroupName();
        Assert.AreNotEqual("Microsoft Office", value2);
    }

    [Test()]
    public void GetShortNameLongTest()
    {
        var value = FormatTypeLong.PowerPoint.GetShortName();
        Assert.AreEqual("ms", value);
    }

    [Test()]
    public void GetNameLongTest()
    {
        var value = FormatTypeLong.Word.GetName();
        Assert.AreEqual("word", value);
    }

    [Test()]
    public void GetDescriptionLongTest()
    {
        var value = FormatTypeLong.Word.GetDescription();
        Assert.AreEqual("word", value);
    }

    [Test()]
    public void GetDescriptionShortTest()
    {
        var value = FormatTypeShort.Word.GetDescription();
        Assert.AreEqual("word", value);
    }

    [Test()]
    public void GetDescriptionTest()
    {
        var value = FormatType.Word.GetDescription();
        Assert.AreEqual("word", value);
    }

    [Test()]
    public void GetValueStringTest()
    {
        var value = FormatType.Word.GetValueAsString();
        Assert.AreEqual("1", value);
    }

    [Test()]
    public void GetValueStringShortTest()
    {
        var value = FormatTypeShort.Word.GetValueAsString();
        Assert.AreEqual("1", value);
    }

    [Test()]
    public void GetStringShortTest()
    {
        var value = FormatTypeShort.Word.GetValueAsString();
        Assert.AreEqual("1", value);
    }

    [Test()]
    public void GetStringTest()
    {
        var list = new List<Enum> { FormatTypeShort.Word, FormatTypeLong.Excel };
        var value = list.ToStringSeparatorDelimited(',');
        Assert.AreEqual(value, "1,2");
    }

    [Test()]
    public void GetListEnumBySeparatorDelimitedTest()
    {
        var value = "1,2,20,40,3".ToListEnumFromSeparatorDelimited<FormatTypeShort>(',').ToList();

        Assert.AreEqual(value[0], FormatTypeShort.Word);

        //Assert.IsTrue(value.Any(e => e == (short)20))
    }

}