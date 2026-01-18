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

        Assert.That(list, Is.Not.Null);
        Assert.That("excel" == list![0].Description);

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

        Assert.That(list, Is.Not.Null);
        Assert.That(list![0].Id.GetType() == typeof(short));
        Assert.That("word" == list[0].Description); //sorted

        //Using display
        Assert.That("Microsoft tools" == list[2].Description);
        Assert.That("Microsoft Office" == list[2].GroupName);

        Assert.That("duplo" == list[3].Name);
    }

    [Test()]
    public void ToListShortWithSortTest()
    {
        var list = Enumeration.ToList<FormatTypeShort, short>(true);

        Assert.That(list, Is.Not.Null);
        Assert.That(list![0].Id.GetType() == typeof(short));
        Assert.That("excel" == list[0].Description); //sorted

        //Using display
        Assert.That("More than one" == list[2].Description);
        Assert.That("Should take this" == list[2].GroupName);

        Assert.That("word" == list[3].Name);
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

    [Test()]
    public void ConcatEnumValues_MultipleEnums_ReturnsConcatenatedValue()
    {
        // FormatType: Word=1, Excel=2, PowerPoint=3
        var result = Enumeration.ConcatEnumValues(FormatType.Word, FormatType.Excel, FormatType.PowerPoint);
        Assert.That(result, Is.EqualTo(123));
    }

    [Test()]
    public void ConcatEnumValues_SingleEnum_ReturnsValue()
    {
        var result = Enumeration.ConcatEnumValues(FormatType.PowerPoint);
        Assert.That(result, Is.EqualTo(3));
    }

    [Test()]
    public void ConcatEnumValues_MixedEnumTypes_ReturnsConcatenatedValue()
    {
        // FormatType.Word=1, FormatTypeByte.Excel=2, FormatTypeLong.Duplo=4
        var result = Enumeration.ConcatEnumValues(FormatType.Word, FormatTypeByte.Excel, FormatTypeLong.Duplo);
        Assert.That(result, Is.EqualTo(124));
    }

    [Test()]
    public void ConcatEnumValues_EmptyArray_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Enumeration.ConcatEnumValues());
    }

    [Test()]
    public void ConcatEnumValues_NullArray_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Enumeration.ConcatEnumValues(null!));
    }

    [Test()]
    public void ConcatEnumValues_MultiDigitValues_ReturnsConcatenatedValue()
    {
        // FormatType: Word=1, Excel=2 → should produce 12
        // FormatTypeLong: Duplo=4, PowerPoint=3 → should produce 43
        var result = Enumeration.ConcatEnumValues(FormatType.Word, FormatType.Excel);
        Assert.That(result, Is.EqualTo(12));
    }
}