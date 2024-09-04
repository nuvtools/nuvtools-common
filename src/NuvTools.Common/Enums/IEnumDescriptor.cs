using System.Text.Json.Serialization;

namespace NuvTools.Common.Enums;

public interface IEnumDescriptor<TKey> where TKey : IEquatable<TKey>
{
    TKey Id { get; }

    string? ShortName { get; }

    string? Name { get; }

    string? Description { get; }

    string? GroupName { get; }

    int? Order { get; }

    [JsonIgnore]
    Type? EnumeratorType { get; }
}

public interface IEnumDescriptor : IEnumDescriptor<int>
{
}