using System.Text.Json.Serialization;

namespace NuvTools.Common.Enums;

/// <summary>
/// Represents a descriptor for enumeration values with rich metadata.
/// </summary>
/// <typeparam name="TKey">The underlying type of the enumeration identifier (e.g., int, byte, long).</typeparam>
public interface IEnumDescriptor<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets the unique identifier of the enumeration value.
    /// </summary>
    TKey Id { get; }

    /// <summary>
    /// Gets the short name or abbreviation of the enumeration value.
    /// </summary>
    string? ShortName { get; }

    /// <summary>
    /// Gets the display name of the enumeration value.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the description of the enumeration value.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Gets the group name for categorizing related enumeration values.
    /// </summary>
    string? GroupName { get; }

    /// <summary>
    /// Gets the display order for sorting enumeration values.
    /// </summary>
    int? Order { get; }

    /// <summary>
    /// Gets the type of the enumerator.
    /// </summary>
    [JsonIgnore]
    Type? EnumeratorType { get; }
}

/// <summary>
/// Represents a descriptor for enumeration values with an integer identifier.
/// </summary>
public interface IEnumDescriptor : IEnumDescriptor<int>
{
}