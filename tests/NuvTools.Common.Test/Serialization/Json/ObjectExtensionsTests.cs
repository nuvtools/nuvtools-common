using NUnit.Framework;
using NuvTools.Common.Serialization.Json;
using System;

namespace NuvTools.Common.Tests.Serialization.Json;

[TestFixture()]
public class ObjectExtensionsTests
{

    private readonly ModelListTest modelInstanceLists = new()
    {
        Name = "List",
        ChildrenList = [ new() {
                Id = 30,
                Name = "ABC",
                YearBirth = 1991,
                Date = new DateTime(1991, 1, 1)
            } ],
        ChildrenEnumerable = [ new() {
                Id = 31,
                Name = "ABCD",
                YearBirth = 1991,
                Date = new DateTime(1990, 1, 1)
            } ]
    };

    private readonly ModelListTest modelInstanceListsNested = new()
    {
        Name = "List Nested",
        Numbers = [1, 2, 3, 4],
        Strings = ["one", "two", "three", "four"],
        ChildrenNestedList = [
                                [
                                    new ModelTest
                                        {
                                            Id = 30,
                                            Name = "ABC",
                                            YearBirth = 1991,
                                            Date = new DateTime(1991, 1, 1),
                                            Strings = ["one", "two", "three", "four"]
                                        },
                                    new ModelTest
                                        {
                                            Id = 31,
                                            Name = "ABCD",
                                            YearBirth = 1992,
                                            Date = new DateTime(1991, 1, 1)
                                        }
                                ]
                            ]
    };

    private readonly ModelTest modelInstance = new()
    {
        Id = 1,
        Name = "Nuv Tools",
        YearBirth = 1984,
        Date = new DateTime(1984, 4, 20),
        Value = (decimal)1.43,
        Numbers = [1, 2, 3, 4],
        Strings = ["one", "two", "three", "four"],
        EnumP = EnumInt.Option2,
        EnumShortP = EnumShort.Option3,
        ChildrenList = [ new() {
                Id = 30,
                Name = "ABC",
                YearBirth = 1991,
                Date = new DateTime(1991, 1, 1)
            } ],
        Children =
        [
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
        ],
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
    private string? serializedObject;

    [Test(), Order(1)]
    public void SerializeTest()
    {
        serializedObject = modelInstance.Serialize(2);
        Assert.That(serializedObject, Is.Not.Null);
    }

    private string? serializedLists;
    [Test(), Order(2)]
    public void SerializeListTest()
    {
        serializedLists = modelInstanceLists.Serialize(2);
        Assert.That(serializedLists, Is.Not.Null);
    }

    private string? serializedNestedLists;
    [Test(), Order(3)]
    public void SerializeNestedListTest()
    {
        serializedNestedLists = modelInstanceListsNested.Serialize(4);
        Assert.That(serializedNestedLists, Is.Not.Null);
    }

    [Test(), Order(4)]
    public void DeserializeTest()
    {
        var copiedObject = modelInstance.Serialize(4).Deserialize<ModelTest>(3);
        Assert.That(copiedObject, Is.Not.Null);
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
        ModelTest modelEnum = new() { EnumP = EnumInt.Option3, EnumShortP = EnumShort.Option1 };

        serializedObject = modelEnum.Serialize(2);
        Assert.That(serializedObject, Is.Not.Null);

        var newModelTest = serializedObject!.Deserialize<ModelTest>();
        Assert.That(newModelTest, Is.Not.Null);
        Assert.That(newModelTest!.EnumShortP == EnumShort.Option1);
    }
}