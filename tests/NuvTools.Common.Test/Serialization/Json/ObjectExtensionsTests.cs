using NUnit.Framework;
using NuvTools.Common.Serialization.Json;
using System;
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

class ModelTest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int YearBirth { get; set; }
    public DateTime? Date { get; set; }
    public decimal Value { get; set; }
    public ModelTest Cousin { get; set; }
    public ModelTest[] Children { get; set; }
    public List<ModelTest> ChildrenList { get; set; }

    public IEnumerable<ModelTest> ChildrenEnumerable { get; set; }

    public int[] Numbers { get; set; }
    public string[] Strings { get; set; }

    public EnumShort EnumShortP { get; set; }
    public EnumInt EnumP { get; set; }
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

[TestFixture()]
public class ObjectExtensionsTests
{

    private readonly ModelListTest modelInstanceLists = new()
    {
        Name = "List",
        ChildrenList = new List<ModelTest> { new() {
                Id = 30,
                Name = "ABC",
                YearBirth = 1991,
                Date = new DateTime(1991, 1, 1)
            } },
        ChildrenEnumerable = new List<ModelTest> { new ModelTest
            {
                Id = 31,
                Name = "ABCD",
                YearBirth = 1991,
                Date = new DateTime(1990, 1, 1)
            } }
    };

    private readonly ModelListTest modelInstanceListsNested = new()
    {
        Name = "List Nested",
        Numbers = new[] { 1, 2, 3, 4 },
        Strings = new[] { "one", "two", "three", "four" },
        ChildrenNestedList = new List<List<ModelTest>> {
                                new List<ModelTest> {
                                    new ModelTest
                                        {
                                            Id = 30,
                                            Name = "ABC",
                                            YearBirth = 1991,
                                            Date = new DateTime(1991, 1, 1),
                                            Strings = new[] { "one", "two", "three", "four" }
                                        },
                                    new ModelTest
                                        {
                                            Id = 31,
                                            Name = "ABCD",
                                            YearBirth = 1992,
                                            Date = new DateTime(1991, 1, 1)
                                        }
                                }
                            }
    };

    private readonly ModelTest modelInstance = new()
    {
        Id = 1,
        Name = "Bruno Melo",
        YearBirth = 1984,
        Date = new DateTime(1984, 4, 20),
        Value = (decimal)1.43,
        Numbers = new[] { 1, 2, 3, 4 },
        Strings = new[] { "one", "two", "three", "four" },
        EnumP = EnumInt.Option2,
        EnumShortP = EnumShort.Option3,
        ChildrenList = new List<ModelTest> { new ModelTest
            {
                Id = 30,
                Name = "ABC",
                YearBirth = 1991,
                Date = new DateTime(1991, 1, 1)
            } },
        Children = new[]
        {
            new ModelTest
            {
                Id = 10,
                Name = "A",
                YearBirth = 1991,
                Date = new DateTime(1991, 1, 1)
            },
            new ModelTest
            {
                Id = 11,
                Name = "B",
                YearBirth = 1991,
                Date = new DateTime(1991, 1, 1),
                Cousin = new ModelTest
                            {
                                Id = 21,
                                Name = "BA",
                                YearBirth = 1991,
                                Date = new DateTime(1991, 1, 1)
                            }
            }
        },
        Cousin = new ModelTest
        {
            Id = 2,
            Name = "Cássia",
            YearBirth = 1991,
            Date = new DateTime(1991, 1, 1),
            Cousin = new ModelTest
            {
                Id = 3,
                Name = "Julia",
                YearBirth = 2010,
                Date = new DateTime(2010, 11, 1)
            }
        }
    };
    private string serializedObject;

    [Test(), Order(1)]
    public void SerializeTest()
    {
        serializedObject = modelInstance.Serialize(2);
        Assert.That(serializedObject is not null);
    }

    private string serializedLists;
    [Test(), Order(2)]
    public void SerializeListTest()
    {
        serializedLists = modelInstanceLists.Serialize(2);
        Assert.That(serializedLists is not null);
    }

    private string serializedNestedLists;
    [Test(), Order(3)]
    public void SerializeNestedListTest()
    {
        serializedNestedLists = modelInstanceListsNested.Serialize(4);
        Assert.That(serializedNestedLists is not null);
    }

    [Test(), Order(4)]
    public void DeserializeTest()
    {
        var copiedObject = modelInstance.Serialize(4).Deserialize<ModelTest>(3);
        Assert.That(copiedObject is not null);
    }

    [Test(), Order(5)]
    public void CopyTest()
    {
        var copiedObject = modelInstance.Clone(2);
        Assert.That(modelInstance != copiedObject);
    }

    [Test(), Order(6)]
    public void SerializeEnumTest()
    {
        var modelEnum = new ModelTest { EnumP = EnumInt.Option3, EnumShortP= EnumShort.Option1 };

        serializedObject = modelEnum.Serialize(2);
        Assert.That(serializedObject is not null);

        ModelTest newModelTest = serializedObject.Deserialize<ModelTest>();
        Assert.That(newModelTest.EnumShortP == EnumShort.Option1);
    }
}