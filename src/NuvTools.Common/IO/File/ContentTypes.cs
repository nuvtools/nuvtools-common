using NuvTools.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace NuvTools.Common.IO.File;

/// <summary>
/// Provides utilities for working with file content types and extensions.
/// </summary>
public static class ContentTypes
{
    /// <summary>
    /// Enumeration using DisplayAttribute.
    /// <para>Each enum contains the title (Name property), extension (ShortName property) and content type information (Description property).</para>
    /// <para>Recommended to use the extensions methods like <seealso cref="GetExtension(Enumeration)"/>, <seealso cref="GetContentType(Enumeration)"/> or <seealso cref="GetFriendlyNameExtension(Enumeration)"/></para>
    /// </summary>
    public enum Enumeration
    {
        [Display(Name = "PDF", ShortName = "pdf", Description = "application/pdf")]
        Pdf = 1,
        [Display(Name = "Plain Text", ShortName = "txt", Description = "text/plain")]
        PlainText = 2,
        [Display(Name = "CSV", ShortName = "csv", Description = "text/csv")]
        Csv = 3,
        [Display(Name = "Excel 98/2000", ShortName = "xls", Description = "application/vnd.ms-excel")]
        Xls = 4,
        [Display(Name = "Icon", ShortName = "ico", Description = "imagem/x-icon")]
        Icon = 5,
        [Display(Name = "SVG", ShortName = "svg", Description = "image/svg+xml")]
        Svg = 6,
        [Display(Name = "Excel", ShortName = "xlsx", Description = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,")]
        Xlsx = 7,
        [Display(Name = "PNG", ShortName = "png", Description = "data:application/octet-stream;base64")]
        Png = 8
    }

    /// <summary>
    /// Gets the extension (e.g., pdf, png) from ContentType enumeration.
    /// </summary>
    /// <param name="enumeration">ContentType enumeration.</param>
    /// <returns></returns>
    public static string GetExtension(this Enumeration enumeration) {
        return enumeration.GetShortName();
    }

    /// <summary>
    /// Gets the content type (e.g., text/csv, application/vnd.ms-excel) from ContentType enumeration.
    /// </summary>
    /// <param name="enumeration">ContentType enumeration.</param>
    /// <returns></returns>
    public static string GetContentType(this Enumeration enumeration)
    {
        return enumeration.GetDescription();
    }

    /// <summary>
    /// Gets the friendly name (e.g., Excel, PNG, PDF) from ContentType enumeration.
    /// </summary>
    /// <param name="enumeration">ContentType enumeration.</param>
    /// <returns></returns>
    public static string GetFriendlyNameExtension(this Enumeration enumeration)
    {
        return enumeration.GetName();
    }
}
