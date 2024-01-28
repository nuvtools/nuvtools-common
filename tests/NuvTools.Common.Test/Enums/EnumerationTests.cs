namespace NuvTools.Common.Tests.Enums;

using NUnit.Framework;
using NuvTools.Common.Enums;
using System;

public enum FormatType
{
    [System.ComponentModel.Description("word")]
    Word = 1,
    [System.ComponentModel.Description("excel")]
    Excel = 2,

    [System.ComponentModel.DataAnnotations.Display(Name = "powerpoint", Description = "Microsoft tools", GroupName = "Microsoft Office")]
    PowerPoint = 3,
    [System.ComponentModel.Description("duplod")]
    [System.ComponentModel.DataAnnotations.Display(Name = "duplo", Description = "More than one", GroupName = "Should take this")]
    Duplo = 4
}

public enum FormatTypeByte : byte
{
    [System.ComponentModel.Description("word")]
    Word = 1,
    [System.ComponentModel.Description("excel")]
    Excel = 2,

    [System.ComponentModel.DataAnnotations.Display(Name = "powerpoint", Description = "Microsoft tools", GroupName = "Microsoft Office")]
    PowerPoint = 3,
    [System.ComponentModel.Description("duplod")]
    [System.ComponentModel.DataAnnotations.Display(Name = "duplo", Description = "More than one", GroupName = "Should take this")]
    Duplo = 4
}

public enum FormatTypeShort : short
{
    [System.ComponentModel.Description("word")]
    Word = 1,
    [System.ComponentModel.Description("excel")]
    Excel = 2,

    [System.ComponentModel.DataAnnotations.Display(Name = "powerpoint", Description = "Microsoft tools", GroupName = "Microsoft Office")]
    PowerPoint = 3,
    [System.ComponentModel.Description("duplod")]
    [System.ComponentModel.DataAnnotations.Display(Name = "duplo", Description = "More than one", GroupName = "Should take this")]
    Duplo = 4
}

public enum FormatTypeLong : long
{
    [System.ComponentModel.Description("word")]
    Word = 1,
    [System.ComponentModel.Description("excel")]
    Excel = 2,

    [System.ComponentModel.DataAnnotations.Display(ShortName = "ms", Name = "powerpoint", Description = "Microsoft tools", GroupName = "Microsoft Office")]
    PowerPoint = 3,
    [System.ComponentModel.Description("duplod")]
    [System.ComponentModel.DataAnnotations.Display(Name = "duplo", Description = "More than one", GroupName = "Should take this")]
    Duplo = 4
}

[TestFixture()]
public class EnumerationTests
{
    [Test()]
    public void GetEnumTest()
    {
        var value = Enumeration.GetEnum<FormatType>("word");
    }

    [Test()]
    public void GetEnumTest1()
    {
        var value = Enumeration.GetEnum<FormatType>(3);

        Assert.That(FormatType.PowerPoint == value);
    }

    [Test()]
    public void GetEnumNotExisting()
    {
        Assert.Throws<IndexOutOfRangeException>(() => Enumeration.GetEnum<FormatType>(20));
    }

    [Test()]
    public void GetEnumByDescriptionTest()
    {
        var value = Enumeration.GetEnumByDescription<FormatType>("excel");
        Assert.That(FormatType.Excel == value);
    }

    [Test()]
    public void ToListTest()
    {
        var list = Enumeration.ToList<FormatType>(true);

        Assert.That(list is not null);
        Assert.That("excel" == list[0].Description);

        //Using display
        Assert.That("More than one" == list[2].Description);
        Assert.That("Should take this" == list[2].GroupName);

        Assert.That("word" == list[3].Name);
    }

    [Test()]
    public void ToListShortWithExceptionTest()
    {
        try
        {
            Enumeration.ToList<FormatTypeShort>(true);
            Assert.Fail();
        }
        catch (System.InvalidCastException)
        {
        }
    }

    [Test()]
    public void ToListByteTest()
    {
        var list = Enumeration.ToList<FormatTypeByte, byte>(false);

        Assert.That(list[0].Id.GetType() == typeof(byte));
        Assert.That("word" == list[0].Description); //sorted

        //Using display
        Assert.That("Microsoft tools" == list[2].Description);
        Assert.That("Microsoft Office" == list[2].GroupName);

        Assert.That("duplo" == list[3].Name);
    }

    [Test()]
    public void ToListShortTest()
    {
        var list = Enumeration.ToList<FormatTypeShort, short>(false);

        Assert.That(list != null);
        Assert.That(list[0].Id.GetType() == typeof(short));
        Assert.That("word" == list[0].Description); //sorted

        //Using display
        Assert.That("Microsoft tools" == list[2].Description);
        Assert.That("Microsoft Office" == list[2].GroupName);

        Assert.That("duplo" == list[3].Name);
    }

    [Test()]
    public void ToListLongTest()
    {
        var list = Enumeration.ToList<FormatTypeLong, long>(false);

        Assert.That(list[0].Id.GetType() == typeof(long));
        Assert.That("word" == list[0].Description); //sorted

        //Using display
        Assert.That("Microsoft tools" == list[2].Description);
        Assert.That("Microsoft Office" == list[2].GroupName);

        Assert.That("duplo" == list[3].Name);
    }

    [Test()]
    public void GetListTest()
    {

    }

    [Test()]
    public void GetValueTest()
    {

    }
}