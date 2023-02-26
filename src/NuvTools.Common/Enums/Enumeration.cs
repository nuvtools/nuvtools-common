
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace NuvTools.Common.Enums;
/// <summary>
/// Helpers to interact with the Enumerations. 
/// </summary>
public static class Enumeration
{

    /// <summary>
    /// Returns the typed value according with the input value or name of enum.
    /// </summary>
    /// <typeparam name="TEnum">Enum type.</typeparam>
    /// <param name="value">Enum value.</param>
    /// <returns>Enum item from value.</returns>
    public static TEnum GetEnum<TEnum>(string value) where TEnum : Enum
    {
        return GetEnum<TEnum, string>(value);
    }

    /// <summary>
    /// Returns the typed value according with the input value or name of enum.
    /// </summary>
    /// <typeparam name="TEnum">Enum type.</typeparam>
    /// <param name="value">Enum value.</param>
    /// <returns>Enum item from value.</returns>
    public static TEnum GetEnum<TEnum>(int value) where TEnum : Enum
    {
        return GetEnum<TEnum, int>(value);
    }

    /// <summary>
    /// Returns the typed value according with the input value or name of enum.
    /// </summary>
    /// <typeparam name="TEnum">Enum type.</typeparam>
    /// <typeparam name="TUEnum">Underlyting enum type.</typeparam>
    /// <param name="value">Enum value.</param>
    /// <returns>Enum item from value.</returns>
    public static TEnum GetEnum<TEnum, TUEnum>(TUEnum value) where TEnum : Enum
                                                                where TUEnum : IEquatable<TUEnum>
    {
        var result = (TEnum)Enum.Parse(typeof(TEnum), value.ToString(), true);

        if (!Enum.IsDefined(typeof(TEnum), result))
            throw new IndexOutOfRangeException(value.ToString());

        return result;
    }

    /// <summary>
    /// Returns the typed value according with the input value or name of enum.
    /// </summary>
    /// <param name="enumType">Enum type.</param>
    /// <param name="value">Enum value.</param>
    /// <returns>Enum item from value.</returns>
    public static Enum GetEnum(Type enumType, object value)
    {
        var result = (Enum)Enum.Parse(enumType, value.ToString(), true);

        if (!Enum.IsDefined(enumType, result))
            throw new IndexOutOfRangeException(value.ToString());

        return result;
    }

    /// <summary>
    /// Returns the value of enum by short name. Work with <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">Enum type.</typeparam>
    /// <param name="value">Short name used in annotation.</param>
    /// <returns>Enum value</returns>
    public static TEnum GetEnumByShortName<TEnum>(string value) where TEnum : Enum
    {
        return GetEnumByProperties<TEnum>(shortName: value);
    }

    /// <summary>
    /// Returns the value of enum by name. Work with <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">Enum type.</typeparam>
    /// <param name="value">Name used in annotation.</param>
    /// <returns>Enum value</returns>
    public static TEnum GetEnumByName<TEnum>(string value) where TEnum : Enum
    {
        return GetEnumByProperties<TEnum>(name: value);
    }

    /// <summary>
    /// Returns the value of enum by description. Work with <see cref="DisplayAttribute"/> or <see cref="DescriptionAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">Enum type.</typeparam>
    /// <param name="value">Description used in annotation.</param>
    /// <returns>Enum value</returns>
    public static TEnum GetEnumByDescription<TEnum>(string value) where TEnum : Enum
    {
        return GetEnumByProperties<TEnum>(description: value);
    }

    /// <summary>
    /// Returns the value of enum by description.
    /// </summary>
    /// <typeparam name="TEnum">Enum type.</typeparam>
    /// <param name="shortName"></param>
    /// <param name="name"></param>
    /// <param name="description">Enum description (annotation).</param>
    /// <returns>Enum value</returns>
    private static TEnum GetEnumByProperties<TEnum>(string shortName = null, string name = null, string description = null) where TEnum : Enum
    {
        if (shortName == null
            && name == null
            && description == null)
            throw new ArgumentNullException();

        var enumList = ToList<TEnum>();

        var enumerator = enumList.FirstOrDefault(e => e.ShortName == shortName
                                                        || e.Name == name
                                                        || e.Description == description);

        if (enumerator == null)
            return (TEnum)Enum.Parse(typeof(TEnum), enumList[0].Id.ToString(), true);

        return (TEnum)Enum.Parse(typeof(TEnum), enumerator.Id.ToString(), true);
    }

    /// <summary>
    /// Convert the enumerator into a generic list.
    /// </summary>
    /// <typeparam name="T">Enumerator type.</typeparam>
    /// <param name="sortByDescription">Sort the list using the description field.</param>
    /// <returns>Generic list corresponding to the enumerator.</returns>
    public static List<IEnumerator> ToList<T>(bool sortByDescription = false) where T : Enum
    {
        return ToList<T, int>(sortByDescription).Select(e => new Enumerator(e)).Cast<IEnumerator>().ToList();
    }

    /// <summary>
    /// Convert the enumerator into a generic list.
    /// </summary>
    /// <typeparam name="T">Enumerator type.</typeparam>
    /// <typeparam name="TKey">Id type that should be the same as enum underlyting type.</typeparam>
    /// <param name="sortByDescription">Sort the list using the description field.</param>
    /// <returns>Generic list corresponding to the enumerator.</returns>
    /// <exception cref="InvalidCastException"></exception>
    public static List<IEnumerator<TKey>> ToList<T, TKey>(bool sortByDescription = false) where T : Enum
                                                                                            where TKey : IEquatable<TKey>
    {
        var enumValues = GetListValues<T>();

        var list = enumValues.Select(e => (Enumerator<TKey>)e);

        if (sortByDescription)
            list = list.OrderBy(e => e.Description);
        else
            list = list.OrderBy(e => e.Order);

        return list.Cast<IEnumerator<TKey>>().ToList();
    }

    /// <summary>
    /// Returns a typed list with the enum values at the fields informed.
    /// </summary>
    /// <typeparam name="TClass">Type list to be filled and returned.</typeparam>
    /// <typeparam name="TEnum">Enum type from which information will be retrieved.</typeparam>
    /// <param name="idField">The list's field which will be filled with the enumerator's item id.</param>
    /// <param name="descriptionField">The list's field which will be filled with the enumerator's item description.</param>
    /// <returns>List with the enum values filled.</returns>
    public static List<TClass> GetList<TEnum, TClass>(string idField, string descriptionField) where TEnum : Enum
                                                                                                where TClass : class
    {
        return GetList<TEnum, TClass, int>(idField, descriptionField);
    }

    /// <summary>
    /// Returns a typed list with the enum values at the fields informed.
    /// </summary>
    /// <typeparam name="TClass">Type list to be filled and returned.</typeparam>
    /// <typeparam name="TEnum">Enum type from which information will be retrieved.</typeparam>
    /// <typeparam name="TKey">Id type.</typeparam>
    /// <param name="idField">The list's field which will be filled with the enumerator's item id.</param>
    /// <param name="descriptionField">The list's field which will be filled with the enumerator's item description.</param>
    /// <returns>List with the enum values filled.</returns>
    public static List<TClass> GetList<TEnum, TClass, TKey>(string idField, string descriptionField) where TEnum : Enum
                                                                                                        where TClass : class
                                                                                                        where TKey : IEquatable<TKey>
    {
        var list = new List<TClass>();
        var enumList = ToList<TEnum, TKey>();

        foreach (var e in enumList)
        {
            var obj = (TClass)Activator.CreateInstance(typeof(TClass));

            obj.GetType().GetRuntimeProperty(idField).SetValue(obj, e.Id, null);
            obj.GetType().GetRuntimeProperty(descriptionField).SetValue(obj, e.Description, null);

            list.Add(obj);
        }

        return list;
    }

    public static List<TEnum> GetListValues<TEnum>() where TEnum : Enum
    {
        var enumType = typeof(TEnum);

        var fields = enumType.GetRuntimeFields().Where(f => f.IsLiteral);

        return fields.Select(field => field.GetValue(enumType)).Cast<TEnum>().ToList();
    }

}