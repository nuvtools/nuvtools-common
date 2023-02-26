namespace NuvTools.Common.Tests.Enums;

using NUnit.Framework;
using NuvTools.Common.Enums;

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

        Assert.AreEqual(FormatType.PowerPoint, value);
    }

    [Test()]
    public void GetEnumTest2()
    {
        var value = Enumeration.GetEnum<FormatType>(20);

        Assert.AreEqual(FormatType.PowerPoint, value);
    }

    [Test()]
    public void GetEnumByDescriptionTest()
    {
        var value = Enumeration.GetEnumByDescription<FormatType>("excel");
        Assert.AreEqual(FormatType.Excel, value);
    }

    [Test()]
    public void ToListTest()
    {
        var list = Enumeration.ToList<FormatType>(true);

        Assert.IsNotNull(list);
        Assert.AreEqual("excel", list[0].Description);

        //Using display
        Assert.AreEqual("More than one", list[2].Description);
        Assert.AreEqual("Should take this", list[2].GroupName);

        Assert.AreEqual("word", list[3].Name);
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

        Assert.AreEqual(list[0].Id.GetType(), typeof(byte));
        Assert.AreEqual("word", list[0].Description); //sorted

        //Using display
        Assert.AreEqual("Microsoft tools", list[2].Description);
        Assert.AreEqual("Microsoft Office", list[2].GroupName);

        Assert.AreEqual("duplo", list[3].Name);
    }

    [Test()]
    public void ToListShortTest()
    {
        var list = Enumeration.ToList<FormatTypeShort, short>(false);

        Assert.IsNotNull(list);
        Assert.AreEqual(list[0].Id.GetType(), typeof(short));
        Assert.AreEqual("word", list[0].Description); //sorted

        //Using display
        Assert.AreEqual("Microsoft tools", list[2].Description);
        Assert.AreEqual("Microsoft Office", list[2].GroupName);

        Assert.AreEqual("duplo", list[3].Name);
    }

    [Test()]
    public void ToListLongTest()
    {
        var list = Enumeration.ToList<FormatTypeLong, long>(false);

        Assert.AreEqual(list[0].Id.GetType(), typeof(long));
        Assert.AreEqual("word", list[0].Description); //sorted

        //Using display
        Assert.AreEqual("Microsoft tools", list[2].Description);
        Assert.AreEqual("Microsoft Office", list[2].GroupName);

        Assert.AreEqual("duplo", list[3].Name);
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