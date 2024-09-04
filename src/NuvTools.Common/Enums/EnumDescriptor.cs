
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NuvTools.Common.Enums;

/// <summary>
/// Representation of enum item with Id and Description fields.
/// </summary>
internal class EnumDescriptor<TKey>(TKey id) : IEnumDescriptor<TKey> where TKey : IEquatable<TKey>

{
    public TKey Id { get; set; } = id;

    public string? ShortName { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? GroupName { get; set; }

    public int? Order { get; set; }

    [JsonIgnore]
    public Type? EnumeratorType { get; set; }

    public static implicit operator EnumDescriptor<TKey>(Enum value)
    {
        EnumDescriptor<TKey>? enumerator;

        try
        {
            Type underlyingType = Enum.GetUnderlyingType(value.GetType());
            var initialId = (TKey)Convert.ChangeType(value, underlyingType);

            enumerator = new EnumDescriptor<TKey>(initialId);
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException("The Id type should be the same of Enum underlyting type.");
        }

        enumerator.EnumeratorType = value.GetType();

        var displayAttributes = (DisplayAttribute[])value.GetType().GetField(value.ToString())!.GetCustomAttributes(typeof(DisplayAttribute), false);
        if (displayAttributes.Length > 0)
        {
            var item = displayAttributes[0];

            enumerator.ShortName = item.GetShortName();
            enumerator.Name = item.GetName();
            enumerator.Description = item.GetDescription();
            enumerator.GroupName = item.GetGroupName();
            enumerator.Order = item.GetOrder();
        }
        else
        {
            var descriptionAttributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString())!.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptionAttributes.Length > 0)
            {
                var item = descriptionAttributes[0];

                enumerator.Name = item.Description;
                enumerator.Description = item.Description;
            }
        }

        enumerator.Name ??= value.ToString();
        enumerator.Description ??= value.ToString();

        return enumerator;
    }

}

/// <summary>
/// Representation of enum item with Id and Description fields.
/// </summary>
internal class EnumDescriptor(int id) : IEnumDescriptor
{
    private readonly EnumDescriptor<int> _descriptor = new(id);

    public int Id => _descriptor.Id;
    public string? ShortName { get => _descriptor.ShortName; set => _descriptor.ShortName = value; }
    public string? Name { get => _descriptor.Name; set => _descriptor.Name = value; }
    public string? Description { get => _descriptor.Description; set => _descriptor.Description = value; }
    public string? GroupName { get => _descriptor.GroupName; set => _descriptor.GroupName = value; }
    public int? Order { get => _descriptor.Order; set => _descriptor.Order = value; }
    public Type? EnumeratorType { get => _descriptor.EnumeratorType; set => _descriptor.EnumeratorType = value; }

    

    public static implicit operator EnumDescriptor(EnumDescriptor<int> descriptor)
    {
        return new EnumDescriptor(descriptor.Id)
        {
            ShortName = descriptor.ShortName,
            Name = descriptor.Name,
            Description = descriptor.Description,
            GroupName = descriptor.GroupName,
            Order = descriptor.Order,
            EnumeratorType = descriptor.EnumeratorType
        };
    }

    public static implicit operator EnumDescriptor(Enum value)
    {
        EnumDescriptor<int> intDescriptor = (EnumDescriptor<int>)value;
        return (EnumDescriptor)intDescriptor;
    }
}