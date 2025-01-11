using System.Collections.Generic;

namespace NuvTools.Common.Tests.Serialization.Json;

public enum EnumShort : short
{
    Option1 = 0,
    Option2 = 1,
    Option3 = 2,
}

public enum EnumInt
{
    Option1 = 0,
    Option2 = 1,
    Option3 = 2
}

class ModelBasicTest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int YearBirth { get; set; }
    public System.DateTime? Date { get; set; }
    public decimal Value { get; set; }
    public EnumShort EnumShortP { get; set; }
    public EnumInt EnumP { get; set; }
}

class ModelTest : ModelBasicTest
{
    public ModelTest Cousin { get; set; }
    public ModelTest[] Children { get; set; }
    public List<ModelTest> ChildrenList { get; set; }

    public IEnumerable<ModelTest> ChildrenEnumerable { get; set; }

    public int[] Numbers { get; set; }
    public string[] Strings { get; set; }
}

class ModelListTest
{
    public string Name { get; set; }
    public ModelTest[] Children { get; set; }

    public ModelTest[][] ChildrenNested { get; set; }

    public List<ModelTest> ChildrenList { get; set; }

    public List<List<ModelTest>> ChildrenNestedList { get; set; }

    public IEnumerable<ModelTest> ChildrenEnumerable { get; set; }

    public int[] Numbers { get; set; }
    public string[] Strings { get; set; }
}
