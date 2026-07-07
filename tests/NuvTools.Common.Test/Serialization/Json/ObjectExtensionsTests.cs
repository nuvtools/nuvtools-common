using NUnit.Framework;
using NuvTools.Common.Serialization.Json;
using System;
using System.Collections.Generic;

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

    [Test(), Order(7)]
    public void SerializeModernTypesAsScalarsTest()
    {
        var model = new ModelModernTypesTest
        {
            BirthDate = new DateOnly(2026, 7, 1),
            OptionalDate = new DateOnly(2046, 7, 1),
            StartTime = new TimeOnly(8, 30),
            Id = Guid.Parse("019f29cf-c841-7a23-b161-39ef2b467ce6"),
            Site = new Uri("https://nuv.tools"),
            AppVersion = new Version(1, 2, 3)
        };

        var serialized = model.Serialize(2);

        // DateOnly/TimeOnly/Guid/Uri/Version must be serialized as JSON scalars,
        // not expanded property-by-property via reflection.
        Assert.That(serialized, Does.Contain("\"2026-07-01\""));
        Assert.That(serialized, Does.Contain("\"2046-07-01\""));
        Assert.That(serialized, Does.Contain("\"08:30:00\""));
        Assert.That(serialized, Does.Contain("\"019f29cf-c841-7a23-b161-39ef2b467ce6\""));
        Assert.That(serialized, Does.Contain("\"https://nuv.tools\""));
        Assert.That(serialized, Does.Contain("\"1.2.3\""));
        Assert.That(serialized, Does.Not.Contain("DayNumber"));
        Assert.That(serialized, Does.Not.Contain("MinValue"));
    }

    [Test(), Order(8)]
    public void SerializeIgnoresStaticPropertiesTest()
    {
        var serialized = new ModelModernTypesTest().Serialize(2);

        Assert.That(serialized, Does.Not.Contain(nameof(ModelModernTypesTest.StaticValue)));
    }

    [Test(), Order(9)]
    public void SerializeDictionaryAsJsonObjectTest()
    {
        var model = new ModelDictionaryTest
        {
            Name = "Dict",
            Values = new() { ["one"] = 1, ["two"] = 2 },
            Children = new() { ["a"] = new() { Id = 1, Name = "A" } },
            Codes = new() { [1] = "x" }
        };

        var serialized = model.Serialize(3);

        // Dictionaries must be JSON objects (key/value), not arrays of KeyValuePair.
        Assert.That(serialized, Does.Contain("\"Values\":{\"one\":1,\"two\":2}"));
        Assert.That(serialized, Does.Contain("\"a\":{"));
        Assert.That(serialized, Does.Contain("\"Name\":\"A\""));
        Assert.That(serialized, Does.Contain("\"Codes\":{\"1\":\"x\"}"));
        Assert.That(serialized, Does.Not.Contain("\"Key\""));
        Assert.That(serialized, Does.Not.Contain("Comparer"));
    }

    [Test(), Order(10)]
    public void SerializeRootDictionaryTest()
    {
        var serialized = new Dictionary<string, string> { ["pt"] = "Oi", ["en"] = "Hello" }.Serialize(2);

        Assert.That(serialized, Is.EqualTo("{\"pt\":\"Oi\",\"en\":\"Hello\"}"));
    }

    [Test(), Order(11)]
    public void SerializeDictionaryRespectsMaxDepthTest()
    {
        var model = new ModelDictionaryTest
        {
            Name = "Depth",
            Children = new() { ["a"] = new() { Id = 1, Name = "A" } }
        };

        var serialized = model.Serialize(1);

        // Depth 1: the dictionary property is beyond the limit and becomes null.
        Assert.That(serialized, Does.Contain("\"Children\":null"));
    }
}