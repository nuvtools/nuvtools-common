# Nuv Tools Common Library

Common library for Web, Desktop and Mobile (MAUI) applications.

## Overview

**NuvTools.Common** is a cross-platform utility library for .NET 8 and .NET 9, designed to streamline development for Web, Desktop, and Mobile (MAUI) applications. It provides a rich set of reusable components, extension methods, and helpers that address frequent programming needs, focusing on consistency, safety, and productivity.

## Main Features Overview

NuvTools.Common provides a suite of utilities and helpers to simplify development across .NET 8 and .NET 9 projects, including Web, Desktop, and Mobile (MAUI) applications. Its main features are:

### 1. ResultWrapper
- **Purpose:** Standardizes operation result handling with support for success, error, and validation outcomes.
- **Key Types:** `IResult`, `IResult<T>`, `Result`, `Result<T>`, `MessageDetail`, `ResultType`.
- **Highlights:** Factory methods for creating results, structured messages, and easy integration with logging.

### 2. NumbersExtensions
- **Purpose:** Safely parse strings to numeric types (`long`, `int`, `short`, `decimal`) with null or zero fallback.
- **Highlights:** Reduces boilerplate and handles invalid input gracefully.

### 3. ContentTypes
- **Purpose:** Centralized management of file content types using annotated enums.
- **Key Types:** `ContentTypes.Enumeration` with extension methods for retrieving file extension, MIME type, and friendly name.
- **Highlights:** Simplifies file handling and metadata retrieval.

### 4. Web Utilities
- **Purpose:** Streamlines query string generation and parsing for web APIs.
- **Key Methods:** `GetQueryString()`, `ParseQueryString()`.
- **Highlights:** Supports complex objects, collections, and edge cases in query string manipulation.

### 5. Serialization Support
- **Purpose:** Facilitates JSON serialization and deserialization, including support for enums and nested models.
- **Highlights:** Handles complex object graphs and collections.

### 6. Enums and Attribute Helpers
- **Purpose:** Simplifies working with enums and attributes, such as retrieving display names, descriptions, and custom metadata.
- **Highlights:** Extension methods for extracting information from enums using attributes like `DisplayAttribute`.

### 7. Date and Time Utilities
- **Purpose:** Provides helpers for parsing, formatting, and manipulating dates and times.
- **Highlights:** Methods for converting between different date/time representations and handling nullable values.

### 8. Collection and LINQ Extensions
- **Purpose:** Adds convenience methods for working with collections, lists, and LINQ queries.
- **Highlights:** Safe access, filtering, mapping, and transformation utilities.

### 9. IO and File Helpers
- **Purpose:** Facilitates file operations, such as reading, writing, and managing file paths and content.
- **Highlights:** Utilities for handling file streams, directories, and file type detection.

### 10. String Extensions
- **Purpose:** Enhances string manipulation with methods for formatting, parsing, validation, and conversion.
- **Highlights:** Null-safe operations, trimming, case conversion, and more.

### 11. Reflection Utilities
- **Purpose:** Provides helpers for runtime type inspection and dynamic member access.
- **Highlights:** Methods for getting property values, types, and attributes dynamically.

### 12. Validation Helpers
- **Purpose:** Supports data validation scenarios, including model and property validation.
- **Highlights:** Methods for checking required fields, data annotations, and custom validation logic.

---

**Benefits:**  
- Consistent patterns for error/success handling.
- Strongly-typed APIs for safer code.
- Reduces repetitive code and runtime errors.
- Designed for modern .NET and C# features.

For more details, refer to the XML documentation in the source code or explore the relevant namespaces.