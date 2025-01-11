using NUnit.Framework;
using NuvTools.Common.Serialization.Json;
using NuvTools.Common.Serialization.Json.Converters;
using System;
using System.Text.Json;

namespace NuvTools.Common.Tests.Serialization.Json.Converters;

class ModelConverterTest
{
    [DateTimeConverter("dd/MM/yyyy")]
    public DateTime DateOutside { get; set; }

    [DateTimeConverter("dd/MM/yyyy")]
    public DateTimeOffset DateOutsideOffset { get; set; }

    [DateTimeConverter("yyyy-MM-dd")]
    public DateTime? DateOutsideOption2 { get; set; }

    [DateTimeConverter("yyyy-MM-dd HH:mm")]
    public DateTimeOffset? DateOutsideOffsetOption2 { get; set; }

    [DateTimeConverter("yyyy-MM-dd")]
    public DateTime? DateOutsideEmpty { get; set; }

    [DateTimeConverter("yyyy-MM-dd")]
    public DateTimeOffset? DateOutsideOffsetEmpty { get; set; }
}

class ModelConverterErrorTest
{
    [DateTimeConverter("dd/MM/yyyy")]
    public int Number { get; set; }

    [DateTimeConverter("dd/MM/yyyy")]
    public string Text { get; set; }
}

[TestFixture()]
public class DateTimeConverterTests
{

    private readonly ModelConverterTest modelInstance = new()
    {
        DateOutside = new DateTime(1984, 4, 20, 10, 30, 10),
        DateOutsideOffset = new DateTimeOffset(1984, 4, 20, 11, 20, 15, TimeSpan.Zero),
        DateOutsideOption2 = new DateTime(1985, 4, 20, 10, 30, 10),
        DateOutsideOffsetOption2 = new DateTimeOffset(1994, 4, 20, 11, 20, 15, TimeSpan.Zero),
    };

    private string serializedObject;

    [Test(), Order(0)]
    public void SerializeTest()
    {
        serializedObject = modelInstance.Serialize(2);

        //serializedObject = JsonSerializer.Serialize(modelInstance);
        Assert.That(serializedObject.Contains("\"DateOutside\":\"20/04/1984\""));

        Assert.That(serializedObject.Contains("\"DateOutsideOption2\":\"1985-04-20\""));

        Assert.That(serializedObject.Contains("\"DateOutsideOffsetOption2\":\"1994-04-20 11:20\""));
    }

    [Test(), Order(1)]
    public void DeserializeTest()
    {
        var copiedObject = modelInstance.Serialize().Deserialize<ModelConverterTest>();
        Assert.That(copiedObject is not null);
    }

    [Test(), Order(2)]
    public void SerializeErrorTest()
    {
        var modelInstance = new ModelConverterErrorTest
        {
            Number = 10,
            Text = "Test"
        };

        Assert.Catch<NotSupportedException>(() => modelInstance.Serialize().Deserialize<ModelConverterTest>());
    }
}