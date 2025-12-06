# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**NuvTools.Common** is a cross-platform utility library for .NET 8, 9, and 10. It provides reusable components, extension methods, and helpers for Web, Desktop, and Mobile (MAUI) applications. The library is published as a NuGet package with strong-name signing.

**Key Facts:**
- Multi-targets: `net8`, `net9`, `net10.0`
- Single dependency: `Microsoft.Extensions.Logging.Abstractions`
- Strong-named assembly (NuvTools.Common.snk)
- Nullable reference types enabled
- Uses latest C# language version
- Current version: 10.0.0

## Build & Test Commands

### Building
```bash
# Build the entire solution
dotnet build NuvTools.Common.slnx

# Build specific target framework
dotnet build src/NuvTools.Common/NuvTools.Common.csproj -f net10.0

# Build in Release mode (generates NuGet package)
dotnet build -c Release
```

### Testing
```bash
# Run all tests
dotnet test

# Run tests for specific project
dotnet test tests/NuvTools.Common.Test/NuvTools.Common.Tests.csproj

# Run single test by filter
dotnet test --filter "FullyQualifiedName~NuvTools.Common.Tests.SpecificTestClass.SpecificTestMethod"

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

### Package Generation
The main project (`src/NuvTools.Common/NuvTools.Common.csproj`) has `GeneratePackageOnBuild` set to `true`, so building in Release mode automatically creates the NuGet package.

```bash
# Generate package
dotnet pack src/NuvTools.Common/NuvTools.Common.csproj -c Release

# Package location: src/NuvTools.Common/bin/Release/
```

## Core Architecture Patterns

### 1. ResultWrapper Pattern (Railway-Oriented Programming)

The `ResultWrapper` namespace implements a hierarchical result pattern for standardized operation outcomes:

**Hierarchy:**
- `ResultBase` → Base with `Succeeded`, `ResultType`, `Messages[]`, `ContainsNotFound`
- `Result` → Non-generic results (no data payload)
- `Result<T>` → Generic results with `Data` property
- `Result<T,E>` → Results with both data and custom error payload

**Usage Pattern:**
```csharp
// Factory methods are the primary API
public static IResult<User> GetUser(int id)
{
    if (id <= 0)
        return Result<User>.ValidationFail("Invalid user ID");

    var user = database.Find(id);
    if (user == null)
        return Result<User>.FailNotFound("User not found");

    return Result<User>.Success(user, new MessageDetail("User retrieved"));
}
```

**Key Features:**
- Factory methods: `Success()`, `Fail()`, `ValidationFail()`, `FailNotFound()`
- Automatic logging via optional `ILogger` parameter
- `MessageDetail` record for structured messages with severity, code, title, and detail
- HTTP integration via `HttpResponseMessageExtensions.ToResultAsync()`

### 2. EnumDescriptor Pattern

Provides rich metadata for enums using reflection and attributes:

**Key Types:**
- `IEnumDescriptor<TKey>` - Interface for enum metadata
- `EnumDescriptor<TKey>` - Implementation with implicit conversion from enum
- `Enumeration` - Static helpers for enum operations

**Metadata Sources:**
- `DisplayAttribute` - Preferred (Name, ShortName, Description, GroupName, Order, Prompt)
- `DescriptionAttribute` - Fallback

**Usage Pattern:**
```csharp
// Extract metadata from DisplayAttribute
var shortName = myEnum.GetShortName();
var description = myEnum.GetDescription();
var groupName = myEnum.GetGroupName();

// Convert to descriptor
IEnumDescriptor<int> descriptor = myEnum.GetEnumerator<int>();

// Get enum from metadata
var enumValue = Enumeration.GetEnumByShortName<MyEnum>("code");
```

### 3. Timezone Abstraction

Cross-platform timezone handling with region-based configuration:

**Key Types:**
- `IDateTimeService` - DI-friendly abstraction for timezone-aware operations
- `SystemDateTimeService` - Default implementation
- `TimeZoneRegion` enum - Predefined regions (Brasilia, UTC, NYC, Tokyo, etc.)
- `TimeZoneExtensions` - Conversion utilities

**Design Decisions:**
- Dual platform support: Maps `TimeZoneRegion` to Windows IDs (e.g., "E. South America Standard Time") OR IANA IDs (e.g., "America/Sao_Paulo")
- Runtime platform detection via `RuntimeInformation.IsOSPlatform()`
- Manual offset fallback: Accepts UTC offset in minutes for custom timezones
- Direction-based API: `UtcDirection` enum (ToUtc, FromUtc, None)

**DI Registration:**
```csharp
services.AddDateTimeService(TimeZoneRegion.Brasilia);
```

### 4. Extension Method Patterns

Extension methods are pervasive throughout the library:

**Conventions:**
- Null-safe: Early guard clauses with `ArgumentNullException.ThrowIfNull()`
- Fluent API: Methods return types suitable for chaining
- Consistent naming: Verb-noun pattern (ToTimeZone, GetQueryString, RemoveDiacritics)
- Overload strategy: Multiple signatures for convenience (string, List<string>, Exception, etc.)

**Important Extension Locations:**
- `StringExtensions` - Left/Right, Format, RemoveDiacritics, IsValidJson, GetNumbersOnly
- `NumbersExtensions` - ParseToLongOrNull, ParseToIntOrNull, ParseToDecimalOrNull
- `EnumerationExtensions` - GetName, GetDescription, GetShortName, GetGroupName
- `ExceptionExtensions` - AggregateExceptionMessages (traverses inner exceptions)
- `TimeZoneExtensions` - ToTimeZone, ToTimeZoneOffset, DateParseToUtc
- `AssemblyExtensions` - Name, Version, Description, ProgramInfo, ListComponent

## Important Coding Conventions

### Null Handling
- Nullable reference types enabled (`<Nullable>enable</Nullable>`)
- Modern guard clauses: `ArgumentNullException.ThrowIfNull(value)`
- Null-conditional operators: `result?.Data`, `properties?.Any()`
- Null-coalescing for defaults: `logger ?? null`, `messages ?? []`

### Async Patterns
- Async methods end with `Async` suffix
- Use `ConfigureAwait(false)` in library code (avoid UI thread capture)
- Sync wrappers call async with `.Result` (e.g., `ToByteList()` → `ToByteListAsync().Result`)
- Prefer async overloads for I/O operations

### Factory Method Pattern
- Static factory methods are the primary API for `Result` types
- Multiple overloads accept different message types (string, MessageDetail, List, Exception)
- Optional `ILogger` parameter for automatic logging
- Protected constructors prevent direct instantiation

### Assembly Strong-Naming
- Assembly is strong-named using `NuvTools.Common.snk`
- All builds must sign the assembly (`<SignAssembly>True</SignAssembly>`)
- Do not modify or remove the key file

## Namespace Organization

### ResultWrapper
Operation result pattern with success/error/validation outcomes. Contains `Result`, `Result<T>`, `Result<T,E>`, `MessageDetail`, and HTTP integration.

### Enums
Enum utilities with metadata extraction via attributes. Includes `Enumeration` static helpers and `EnumDescriptor<TKey>` for rich enum representation.

### Dates
Timezone-aware date/time handling. Contains `IDateTimeService`, region-based timezone configuration, and cross-platform timezone mapping.

### Strings
Text manipulation utilities including formatting, diacritics removal, JSON validation, and custom Format() with dictionary support.

### Numbers
Numeric utilities and parsing. Includes safe string-to-number parsing with null fallback and Portuguese number-to-words conversion.

### Numbers.Portuguese
Brazilian Portuguese specific utilities for converting numbers to words (currency format).

### Reflection
Assembly introspection, embedded resource loading, version extraction, and `ProgramInfo` metadata container.

### Serialization
Type classification helpers (`IsSimple()`, `IsList()`). JSON-specific utilities in `Serialization.Json` namespace.

### Serialization.Json
JSON serialization extensions including deep cloning and custom converters (`DateTimeJsonConverter`, `MaxDepthJsonConverter`).

### Exceptions
Exception handling utilities for aggregating multi-level exception messages.

### IO
Stream operations (ToByteList sync/async) and file content type mappings.

### Web
HTTP/URL utilities for object-to-query-string conversion and query string parsing.

### RegularExpressions
Regex patterns and extension methods for Match, IsMatch, and ReplacePattern operations.

## XML Documentation

This library generates XML documentation files (`GenerateDocumentationFile=True`). When adding new public APIs:

1. **Always** add XML documentation comments
2. Use `<summary>`, `<param>`, `<returns>`, `<exception>`, and `<remarks>` tags
3. Reference related types using `<see cref="TypeName"/>`
4. Document all parameters and return values
5. Include code examples in `<example>` tags for complex APIs

## Testing Framework

Tests use **NUnit** with these packages:
- NUnit 4.4.0
- NUnit3TestAdapter 5.2.0
- Microsoft.NET.Test.Sdk 18.0.1

Test project targets `net10.0` only (no multi-targeting needed for tests).

## Key Design Principles

1. **Minimal Dependencies** - Only Microsoft.Extensions.Logging.Abstractions
2. **Cross-Platform** - Platform-aware timezone handling, IANA/Windows timezone mapping
3. **Type Safety** - Extensive use of generics and strong typing
4. **Null Safety** - Nullable annotations and defensive null checks
5. **Extension Method First** - Prefer extension methods for discoverability
6. **Factory Methods** - Use static factory methods for complex object construction
7. **Async by Default** - Provide async methods for I/O operations, with sync wrappers when needed
8. **Logging Integration** - Optional ILogger parameters for automatic logging
9. **Multi-Targeting** - Support multiple .NET versions for broad compatibility

## Common Patterns to Follow

### Adding a New Extension Method
```csharp
public static class MyTypeExtensions
{
    /// <summary>
    /// Brief description of what this does.
    /// </summary>
    /// <param name="value">Description of parameter.</param>
    /// <returns>Description of return value.</returns>
    /// <exception cref="ArgumentNullException">When thrown.</exception>
    public static string MyExtension(this MyType value)
    {
        ArgumentNullException.ThrowIfNull(value);
        // Implementation
    }
}
```

### Adding a New Result Type Operation
```csharp
public static IResult<TData> MyOperation<TData>(TData data, ILogger? logger = null)
{
    // Validation
    if (data == null)
        return Result<TData>.ValidationFail("Data cannot be null", logger);

    // Success
    return Result<TData>.Success(data, new MessageDetail("Operation succeeded"));
}
```

### Adding Timezone Support
Use `IDateTimeService` for timezone-aware operations:
```csharp
public class MyService
{
    private readonly IDateTimeService _dateTimeService;

    public MyService(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public DateTime GetLocalTime()
    {
        return _dateTimeService.Now; // Returns time in configured region
    }
}
```
