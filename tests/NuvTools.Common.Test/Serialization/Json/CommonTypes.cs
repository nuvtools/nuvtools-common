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
    public string? Name { get; set; }
    public int YearBirth { get; set; }
    public System.DateTime? Date { get; set; }
    public decimal Value { get; set; }
    public EnumShort EnumShortP { get; set; }
    public EnumInt EnumP { get; set; }
}

class ModelTest : ModelBasicTest
{
    public ModelTest? Cousin { get; set; }
    public ModelTest[]? Children { get; set; }
    public List<ModelTest>? ChildrenList { get; set; }

    public IEnumerable<ModelTest>? ChildrenEnumerable { get; set; }

    public int[]? Numbers { get; set; }
    public string[]? Strings { get; set; }
}

class ModelModernTypesTest
{
    public static string StaticValue { get; set; } = "must-not-serialize";

    public System.DateOnly BirthDate { get; set; }
    public System.DateOnly? OptionalDate { get; set; }
    public System.TimeOnly StartTime { get; set; }
    public System.Guid Id { get; set; }
    public System.Uri? Site { get; set; }
    public System.Version? AppVersion { get; set; }
}

class ModelDictionaryTest
{
    public string? Name { get; set; }
    public Dictionary<string, int>? Values { get; set; }
    public Dictionary<string, ModelBasicTest>? Children { get; set; }
    public Dictionary<int, string>? Codes { get; set; }
}

class ModelListTest
{
    public string? Name { get; set; }
    public ModelTest[]? Children { get; set; }

    public ModelTest[][]? ChildrenNested { get; set; }

    public List<ModelTest>? ChildrenList { get; set; }

    public List<List<ModelTest>>? ChildrenNestedList { get; set; }

    public IEnumerable<ModelTest>? ChildrenEnumerable { get; set; }

    public int[]? Numbers { get; set; }
    public string[]? Strings { get; set; }
}
