
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NuvTools.Common.Enums;
/// <summary>
/// Representation of enum item with Id and Description fields.
/// </summary>
internal class Enumerator<TKey> : IEnumerator<TKey> where TKey : IEquatable<TKey>

{
    public TKey Id { get; set; }

    public string ShortName { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string GroupName { get; set; }

    public int? Order { get; set; }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Type EnumeratorType { get; set; }

    public static implicit operator Enumerator<TKey>(Enum value)
    {
        if (value == null) return null;

        Enumerator<TKey> enumerator = null;

        var displayAttributes = (DisplayAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false);
        if (displayAttributes.Length > 0)
        {
            var item = displayAttributes[0];

            enumerator = new Enumerator<TKey>
            {
                ShortName = item.GetShortName(),
                Name = item.GetName(),
                Description = item.GetDescription(),
                GroupName = item.GetGroupName(),
                Order = item.GetOrder()
            };
        }

        if (enumerator == null)
        {
            var descriptionAttributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptionAttributes.Length > 0)
            {
                var item = descriptionAttributes[0];

                enumerator = new Enumerator<TKey>
                {
                    Name = item.Description,
                    Description = item.Description
                };
            }
        }

        if (enumerator == null) enumerator = new() { Name = value.ToString(), Description = value.ToString() };

        try
        {
            enumerator.Id = (TKey)(object)value;
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException("The Id type should be the same of Enum underlyting type.");
        }

        enumerator.EnumeratorType = value.GetType();

        return enumerator;
    }
}

/// <summary>
/// Representation of enum item with Id and Description fields.
/// </summary>
internal class Enumerator : Enumerator<int>, IEnumerator
{
    public Enumerator()
    {
    }

    public Enumerator(IEnumerator<int> enumerator)
    {
        ShortName = enumerator.ShortName;
        Name = enumerator.Name;
        Description = enumerator.Description;
        GroupName = enumerator.GroupName;
        Order = enumerator.Order;
        EnumeratorType = enumerator.EnumeratorType;
        Id = enumerator.Id;
    }

    public static implicit operator Enumerator(Enum value)
    {
        if (value == null) return null;
        return new Enumerator((Enumerator<int>)value);
    }
}