using System.Collections;

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
        ArgumentNullException.ThrowIfNull(valueType);

        // Handle Nullable<T> types
        if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
            return valueType.GetGenericArguments()[0].IsSimple();

        // Check for primitive types, common structs, enums, and basic types
        return valueType.IsPrimitive // Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single
            || new Type[] {
                typeof(Enum),
                typeof(string),
                typeof(decimal),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid)
                    }.Contains(valueType)
            || Convert.GetTypeCode(valueType) != TypeCode.Object; // Covers basic types handled by TypeCode
    }

    /// <summary>
    /// Verify if the value type is some kind of list (IEnumerable).
    /// </summary>
    /// <param name="valueType">Type to be verified.</param>
    /// <returns></returns>
    public static bool IsList(this Type valueType)
    {
        ArgumentNullException.ThrowIfNull(valueType);

        return typeof(IEnumerable).IsAssignableFrom(valueType);
    }
}