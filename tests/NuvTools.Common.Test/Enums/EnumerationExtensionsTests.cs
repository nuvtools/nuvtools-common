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
        Assert.That("Microsoft Office" == value);

        var value2 = FormatTypeLong.Word.GetGroupName();
        Assert.That("Microsoft Office" != value2);
    }

    [Test()]
    public void GetShortNameLongTest()
    {
        var value = FormatTypeLong.PowerPoint.GetShortName();
        Assert.That("ms" == value);
    }

    [Test()]
    public void GetNameLongTest()
    {
        var value = FormatTypeLong.Word.GetName();
        Assert.That("word" == value);
    }

    [Test()]
    public void GetDescriptionLongTest()
    {
        var value = FormatTypeLong.Word.GetDescription();
        Assert.That("word" == value);
    }

    [Test()]
    public void GetDescriptionShortTest()
    {
        var value = FormatTypeShort.Word.GetDescription();
        Assert.That("word" == value);
    }

    [Test()]
    public void GetDescriptionTest()
    {
        var value = FormatType.Word.GetDescription();
        Assert.That("word" == value);
    }

    [Test()]
    public void GetValueStringTest()
    {
        var value = FormatType.Word.GetValueAsString();
        Assert.That("1" == value);
    }

    [Test()]
    public void GetValueStringShortTest()
    {
        var value = FormatTypeShort.Word.GetValueAsString();
        Assert.That("1" == value);
    }

    [Test()]
    public void GetStringShortTest()
    {
        var value = FormatTypeShort.Word.GetValueAsString();
        Assert.That("1" == value);
    }

    [Test()]
    public void GetStringTest()
    {
        var list = new List<Enum> { FormatTypeShort.Word, FormatTypeLong.Excel };
        var value = list.ToStringSeparatorDelimited(',');
        Assert.That(value == "1,2");
    }

    [Test()]
    public void GetListEnumBySeparatorDelimitedTest()
    {
        var value = "1,2,3,4".ToListEnumFromSeparatorDelimited<FormatTypeShort>(',').ToList();

        Assert.That(value[0] == FormatTypeShort.Word);

        //Assert.IsTrue(value.Any(e => e == (short)20))
    }

}