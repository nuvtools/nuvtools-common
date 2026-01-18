# NuvTools.Common

[![NuGet](https://img.shields.io/nuget/v/NuvTools.Common.svg)](https://www.nuget.org/packages/NuvTools.Common/)
[![License](https://img.shields.io/github/license/nuvtools/nuvtools-common.svg)](LICENSE)

A cross-platform utility library for .NET 8, 9, and 10, designed to streamline development for Web, Desktop, and Mobile (MAUI) applications.

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package NuvTools.Common
```

Or via Package Manager Console:

```powershell
Install-Package NuvTools.Common
```

## Overview

**NuvTools.Common** provides a rich set of reusable components, extension methods, and helpers that address frequent programming needs, focusing on consistency, safety, and productivity. The library follows modern .NET best practices with nullable reference types, async/await patterns, and minimal dependencies.

## Key Features

### 1. ResultWrapper - Railway-Oriented Programming

Standardize operation outcomes with a fluent result pattern supporting success, error, and validation states.

```csharp
using NuvTools.Common.ResultWrapper;

public IResult<User> GetUser(int id)
{
    if (id <= 0)
        return Result<User>.ValidationFail("Invalid user ID");

    var user = _repository.FindById(id);
    if (user == null)
        return Result<User>.FailNotFound("User not found");

    return Result<User>.Success(user, new MessageDetail("User retrieved successfully"));
}

// Usage
var result = GetUser(123);
if (result.Succeeded)
{
    Console.WriteLine($"Found user: {result.Data.Name}");
}
else
{
    Console.WriteLine($"Error: {result.Message}");
}
```

**Features:**
- Generic `Result<T>` and `Result<T, E>` for typed returns
- Factory methods: `Success()`, `Fail()`, `ValidationFail()`, `FailNotFound()`
- Structured messages with `MessageDetail` (code, severity, title, detail)
- Built-in logging integration with `ILogger`
- HTTP response conversion via `ToResultAsync()`

### 2. Timezone-Aware Date/Time Service

Cross-platform timezone handling with dependency injection support.

```csharp
using NuvTools.Common.Dates;
using NuvTools.Common.Dates.Enumerations;

// Configure in DI container
services.AddDateTimeService(TimeZoneRegion.Brasilia);

// Use in your service
public class OrderService
{
    private readonly IDateTimeService _dateTime;

    public OrderService(IDateTimeService dateTime)
    {
        _dateTime = dateTime;
    }

    public void CreateOrder()
    {
        var orderTime = _dateTime.Now; // Local time in Brasilia
        var utcTime = _dateTime.UtcNow; // Always UTC
    }
}

// Extension methods for conversion
var localTime = DateTime.UtcNow.ToTimeZone(TimeZoneRegion.Tokyo, UtcDirection.FromUtc);
```

**Features:**
- 19 predefined timezone regions (Brasilia, UTC, Tokyo, NYC, London, etc.)
- Automatic Windows/IANA timezone ID mapping
- `IDateTimeService` abstraction for testability
- Custom UTC offset support

### 3. Enhanced Enum Support

Extract rich metadata from enums using `DisplayAttribute` and `DescriptionAttribute`.

```csharp
using NuvTools.Common.Enums;
using System.ComponentModel.DataAnnotations;

public enum Status
{
    [Display(Name = "Active", ShortName = "ACT", Description = "Currently active")]
    Active = 1,

    [Display(Name = "Inactive", ShortName = "INA", Description = "Not active")]
    Inactive = 2
}

public enum Priority
{
    Low = 1,
    Medium = 2,
    High = 3
}

// Extract metadata
var name = Status.Active.GetName();           // "Active"
var shortName = Status.Active.GetShortName(); // "ACT"
var description = Status.Active.GetDescription(); // "Currently active"

// Convert to list for dropdowns
var statusList = Enumeration.ToList<Status>();

// Find enum by metadata
var status = Enumeration.GetEnumByShortName<Status>("ACT"); // Status.Active

// Concatenate enum values into a composite integer
int composite = Enumeration.ConcatEnumValues(Status.Active, Priority.High);
// Result: 13 (concatenation of "1" + "3")

int multiValue = Enumeration.ConcatEnumValues(Status.Active, Status.Inactive, Priority.Medium);
// Result: 122 (concatenation of "1" + "2" + "2")
```

**Features:**
- Extract `Name`, `ShortName`, `Description`, `GroupName` from `DisplayAttribute`
- Fallback to `DescriptionAttribute` when `DisplayAttribute` is not present
- Convert enums to lists for UI dropdowns with sorting options
- Find enum values by metadata (name, short name, description)
- Concatenate multiple enum values into a single composite integer
- Support for different underlying types (int, byte, short, long)

### 4. String Extensions

Powerful string manipulation utilities.

```csharp
using NuvTools.Common.Strings;

// Extract substrings
string text = "Hello World";
text.Left(5);  // "Hello"
text.Right(5); // "World"

// Remove diacritics
"Olá, Mundo!".RemoveDiacritics(); // "Ola, Mundo!"

// Advanced formatting with named placeholders
var template = "Hello {name}, you have {count} messages";
template.Format(new Dictionary<string, object>
{
    { "name", "John" },
    { "count", 5 }
}); // "Hello John, you have 5 messages"

// Extract numbers
"ABC-123-XYZ-456".GetNumbersOnly(); // "123456"

// Validate JSON
string json = "{\"key\":\"value\"}";
bool isValid = json.IsValidJson(); // true
```

### 5. Safe Number Parsing

Parse strings to numeric types with null or zero fallback.

```csharp
using NuvTools.Common.Numbers;

string value = "123";
long? number = value.ParseToLongOrNull(); // 123

string invalid = "abc";
long? nullResult = invalid.ParseToLongOrNull(); // null
long zeroResult = invalid.ParseToLongOrNull(returnZeroIsNull: true); // 0

// Also available for int, short, decimal
int? intValue = "42".ParseToIntOrNull();
decimal? decimalValue = "3.14".ParseToDecimalOrNull();
```

### 6. Web Utilities

Generate and parse query strings from objects.

```csharp
using NuvTools.Common.Web;

public class SearchRequest
{
    public string Query { get; set; }
    public int Page { get; set; }
    public DateTime? StartDate { get; set; }
}

var request = new SearchRequest
{
    Query = "test",
    Page = 1,
    StartDate = DateTime.Now
};

// Convert to query string
string url = request.GetQueryString("https://api.example.com/search");
// Result: https://api.example.com/search?Query=test&Page=1&StartDate=2025-12-06T10:30:00

// Parse query string back
var dict = "?Query=test&Page=1".ParseQueryString();
```

### 7. Exception Utilities

Aggregate multi-level exception messages for better error reporting.

```csharp
using NuvTools.Common.Exceptions;

try
{
    // Code that throws nested exceptions
}
catch (Exception ex)
{
    // Get all inner exception messages
    string fullMessage = ex.AggregateExceptionMessages(level: 3);
    // Result: "Level 0: Outer exception -> Level 1: Inner exception -> Level 2: Root cause"
}
```

### 8. Stream Extensions

Convert streams to byte arrays with sync and async support.

```csharp
using NuvTools.Common.IO;

// Async
byte[] bytes = await stream.ToByteListAsync();

// Sync
byte[] bytes = stream.ToByteList();
```

### 9. Assembly Reflection

Extract metadata from assemblies.

```csharp
using NuvTools.Common.Reflection;

var assembly = Assembly.GetExecutingAssembly();
var info = assembly.ProgramInfo();

Console.WriteLine($"Name: {info.Name}");
Console.WriteLine($"Version: {info.Version}");
Console.WriteLine($"Description: {info.Description}");

// List all referenced assemblies
var components = assembly.ListComponent();
```

### 10. JSON Serialization

Deep cloning and serialization utilities.

```csharp
using NuvTools.Common.Serialization.Json;

var original = new MyObject { Name = "Test" };
var clone = original.Clone();

string json = original.Serialize();
var deserialized = json.Deserialize<MyObject>();
```

## Why NuvTools.Common?

✅ **Type-Safe** - Strong typing and generics throughout
✅ **Null-Safe** - Nullable reference types enabled with defensive coding
✅ **Async-First** - Modern async/await patterns with `ConfigureAwait(false)`
✅ **Minimal Dependencies** - Only depends on `Microsoft.Extensions.Logging.Abstractions`
✅ **Cross-Platform** - Works on Windows, Linux, and macOS
✅ **Well-Documented** - Complete XML documentation for IntelliSense
✅ **Battle-Tested** - Strong-named, production-ready code
✅ **Multi-Targeted** - Supports .NET 8, 9, and 10

## Supported Frameworks

- .NET 8.0
- .NET 9.0
- .NET 10.0

## Requirements

- No external dependencies (except `Microsoft.Extensions.Logging.Abstractions` for logging integration)
- Works with Web, Desktop, Console, and MAUI applications

## Documentation

All public APIs include comprehensive XML documentation. IntelliSense will show detailed information about:
- Method purposes and usage
- Parameter descriptions
- Return values
- Exception scenarios
- Code examples

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the terms specified in the [LICENSE](LICENSE) file.

## Repository

- **GitHub**: [https://github.com/nuvtools/nuvtools-common](https://github.com/nuvtools/nuvtools-common)
- **NuGet**: [https://www.nuget.org/packages/NuvTools.Common/](https://www.nuget.org/packages/NuvTools.Common/)
- **Website**: [https://nuvtools.com](https://nuvtools.com)

---

Copyright © 2026 Nuv Tools