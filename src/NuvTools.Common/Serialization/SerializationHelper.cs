using System;
using System.Linq;

namespace NuvTools.Common.Serialization;

public static class SerializationHelper
{
    /// <summary>
    /// Verify if the value type is simple.
    /// </summary>
    /// <param name="valueType">Type to be verified.</param>
    /// <returns></returns>
    public static bool IsSimple(this Type valueType)
    {
        if (valueType is null) throw new ArgumentNullException(nameof(valueType));

        return
            valueType.IsPrimitive ||
            new Type[] {
        typeof(Enum),
        typeof(string),
        typeof(decimal),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(TimeSpan),
        typeof(Guid)
            }.Contains(valueType) ||
            Convert.GetTypeCode(valueType) != TypeCode.Object ||
            valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>) && valueType.GetGenericArguments()[0].IsSimple()
            ;
    }
}