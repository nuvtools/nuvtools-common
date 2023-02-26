using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NuvTools.Common.Enums;

/// <summary>
/// Helpers to interact with the Enumerations. 
/// </summary>
public static class EnumerationExtensions
{

    /// <summary>
    /// Returns the group name of enum item. Work with <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <param name="value">Enum item.</param>
    /// <returns>Enum item description.</returns>
    public static string GetGroupName(this Enum value)
    {
        return value.GetDynamicEnumerator().GroupName;
    }

    /// <summary>
    /// Checks whether the value belongs to a group.
    /// </summary>
    /// <param name="value">Enum item.</param>
    /// <param name="groupName">Group name.</param>
    /// <returns></returns>
    public static bool IsInGroup(this Enum value, string groupName)
    {
        return value.GetGroupName() == groupName;
    }

    /// <summary>
    /// Returns the short name of enum item. Work with <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <param name="value">Enum item.</param>
    /// <returns>Enum item description.</returns>
    public static string GetShortName(this Enum value)
    {
        return value.GetDynamicEnumerator().ShortName;
    }

    /// <summary>
    /// Returns the name of enum item. Work with <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <param name="value">Enum item.</param>
    /// <returns>Enum item description.</returns>
    public static string GetName(this Enum value)
    {
        return value.GetDynamicEnumerator().Name;
    }

    /// <summary>
    /// Returns the description of enum item. Work with <see cref="DisplayAttribute"/> or <see cref="DescriptionAttribute"/>.
    /// </summary>
    /// <param name="value">Enum item.</param>
    /// <returns>Enum item description.</returns>
    public static string GetDescription(this Enum value)
    {
        return value.GetDynamicEnumerator().Description;
    }

    /// <summary>
    /// Returns the enum value as string.
    /// </summary>
    /// <param name="value">Enum item.</param>
    /// <returns>Enum value as string.</returns>
    public static string GetValueAsString(this Enum value)
    {
        return value.GetDynamicEnumerator().Id.ToString();
    }

    private static dynamic GetDynamicEnumerator(this Enum value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var tipo = Enum.GetUnderlyingType(value.GetType());

        dynamic enumerator;
        if (tipo == typeof(int))
            enumerator = ((Enumerator<int>)value);
        else if (tipo == typeof(byte))
            enumerator = ((Enumerator<byte>)value);
        else if (tipo == typeof(short))
            enumerator = ((Enumerator<short>)value);
        else
            enumerator = ((Enumerator<long>)value);

        return enumerator;
    }

    /// <summary>
    /// Returns the enum item description.
    /// </summary>
    /// <typeparam name="TUEnum">Id type that should be the same as enum underlyting type.</typeparam>
    /// <param name="value">Enum item.</param>
    /// <returns>Enum item description.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static IEnumerator<TUEnum> GetEnumerator<TUEnum>(this Enum value) where TUEnum : IEquatable<TUEnum>
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return (IEnumerator<TUEnum>)value;
    }

    /// <summary>
    /// Returns the enum list as string with separators.
    /// </summary>
    /// <typeparam name="TEnum">Enum type</typeparam>
    /// <param name="list">Enumerate list</param>
    /// <param name="separator">Item separator</param>
    /// <returns>List as string with separators.</returns>
    public static string ToStringSeparatorDelimited<TEnum>(this IEnumerable<TEnum> list, char separator) where TEnum : Enum
    {
        var result = list.Aggregate("", (current, item) => current + GetValueAsString(item) + separator);

        result = (result.Length > 0) ? result[..^1] : null;

        return result;
    }

    /// <summary>
    /// Returns the enum list from string with separators.
    /// </summary>
    /// <typeparam name="TEnum">Enum type.</typeparam>
    /// <param name="value">Value string with enums values separated by delimited char.</param>
    /// <param name="separator">Item separator.</param>
    /// <returns>Enum list.</returns>
    public static IEnumerable<TEnum> ToListEnumFromSeparatorDelimited<TEnum>(this string value, char separator) where TEnum : Enum
    {
        var listValue = value.Split(separator);

        var result = new List<TEnum>();
        foreach (var item in listValue)
        {
            result.Add(Enumeration.GetEnum<TEnum>(item));
        }

        return result.Distinct();
    }

    /// <summary>
    /// Returns the enum list from string with separators.
    /// </summary>
    /// <param name="value">Value string with enums values separated by delimited char.</param>
    /// <param name="enumType">Enum type</param>
    /// <param name="separator">Item separator.</param>
    /// <returns>Enum list.</returns>
    public static IEnumerable<Enum> ToListEnumFromSeparatorDelimited(this string value, Type enumType, char separator)
    {
        var listValue = value.Split(separator);

        var result = new List<Enum>();
        foreach (var item in listValue)
        {
            result.Add(Enumeration.GetEnum(enumType, item));
        }

        return result.Distinct();
    }

    public static DisplayAttribute[] GetDisplayAttributes(this Enum value)
    {
        return (DisplayAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false);
    }

    /// <summary>
    /// Returns the Order value from DisplayAttribute.
    /// </summary>
    /// <param name="value">Enum with DisplayAttribute.</param>
    /// <returns></returns>
    public static int GetOrder(this Enum value)
    {
        return value.GetType()
       .GetMember(value.ToString()).First()
       .GetCustomAttribute<DisplayAttribute>()
       .Order;
    }

    /// <summary>
    /// Returns the Order value from DisplayAttribute.
    /// </summary>
    /// <param name="value">Enum with DisplayAttribute.</param>
    /// <returns></returns>
    public static string GetPrompt(this Enum value)
    {
        return value.GetType()
       .GetMember(value.ToString()).First()
       .GetCustomAttribute<DisplayAttribute>()
       .Prompt;
    }
}