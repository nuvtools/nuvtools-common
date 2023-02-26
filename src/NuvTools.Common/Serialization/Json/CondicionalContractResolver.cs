using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace NuvTools.Common.Serialization.Json;

/// <summary>
/// Logical by Nathan Baulch (http://nathanbaulch.blogspot.com.br)
/// </summary>
internal class CondicionalContractResolver : DefaultContractResolver
{
    private readonly Func<int> _remainingLevel;

    public CondicionalContractResolver(Func<int> remainingLevel)
    {
        _remainingLevel = remainingLevel;
    }

    protected override JsonProperty CreateProperty(
        MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        var shouldSerialize = property.ShouldSerialize;
        property.ShouldSerialize = obj => (_remainingLevel() > -1 && property.PropertyType.IsSimple()
                                            || _remainingLevel() > 0)
                                            && (shouldSerialize == null
                                                    || shouldSerialize(obj));
        return property;
    }
}