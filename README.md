# Nuv Tools Common Library

Common library for Web, Desktop and Mobile (MAUI) applications.

## Overview

**NuvTools.Common** is a cross-platform utility library for .NET 8 and .NET 9, designed to streamline development for Web, Desktop, and Mobile (MAUI) applications. It provides a rich set of reusable components, extension methods, and helpers that address frequent programming needs, focusing on consistency, safety, and productivity.

### Main Components

#### 1. ResultWrapper

A robust pattern for representing the outcome of operations, supporting success, error, and validation states. It standardizes how results and errors are handled and communicated across layers.

- **Interfaces:**  
  - `IResult` / `IResult<T>`: Define contracts for result objects, supporting both generic and non-generic scenarios.
- **Implementations:**  
  - `Result` / `Result<T>`: Concrete classes encapsulating success status, messages, and optional data.
- **Message System:**  
  - `MessageDetail`: Encapsulates message metadata (code, text, severity).
  - `ResultType`: Enum for result classification (`Success`, `Error`, `ValidationError`).
- **Factory Methods:**  
  - `Success`, `Fail`, `ValidationFail`, `FailNotFound`: Static methods for creating standardized result objects.

**Benefits:**  
- Uniform error and success handling.
- Strongly-typed data payloads.
- Easy integration with logging and monitoring.

#### 2. NumbersExtensions

Extension methods for safe and flexible parsing of numeric values from strings, reducing boilerplate and runtime errors.

- `ParseToLongOrNull(string, bool)`: Converts a string to `long?`, with optional zero fallback.
- `ParseToIntOrNull(string, bool)`: Converts a string to `int?`.
- `ParseToShortOrNull(string, bool)`: Converts a string to `short?`.
- `ParseToDecimalOrNull(string, bool)`: Converts a string to `decimal?`.

**Benefits:**  
- Handles null, empty, and invalid input gracefully.
- Reduces repetitive parsing code.

#### 3. ContentTypes

Strongly-typed file content type management using annotated enumerations.

- `ContentTypes.Enumeration`: Enum with `DisplayAttribute` for name, extension, and MIME type.
- Extension methods:
  - `GetExtension()`: Returns file extension.
  - `GetContentType()`: Returns MIME type.
  - `GetFriendlyNameExtension()`: Returns friendly name.

**Benefits:**  
- Centralized file type metadata.
- Simplifies file upload/download logic.

#### 4. Web Utilities

Helpers for working with query strings, supporting object-to-query conversion and parsing.

- `GetQueryString()`: Converts objects to query string format, handling collections and complex types.
- `ParseQueryString()`: Parses query strings into key-value collections.

**Benefits:**  
- Simplifies web API integration.
- Handles edge cases in query string generation and parsing.

#### 5. Serialization Support

Types and helpers for JSON serialization, including support for enums, nested models, and collections.

**Benefits:**  
- Facilitates model serialization/deserialization.
- Supports complex object graphs.

---

### Why Use NuvTools.Common?

- **Consistency:** Standardizes common patterns across your codebase.
- **Safety:** Reduces runtime errors with robust parsing and result handling.
- **Productivity:** Minimizes boilerplate, letting you focus on business logic.
- **Modern:** Fully compatible with .NET 8/9 and C# 13 features.

For more details, see the XML documentation in the source code or explore the relevant namespaces.