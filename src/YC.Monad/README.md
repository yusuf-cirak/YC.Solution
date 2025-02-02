<!-- @format -->

# YC.Monad

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/YC.Monad)](https://www.nuget.org/packages/YC.Monad/)

## Description

YC.Monad is a .NET library implementing functional programming patterns through monadic types. It provides three main types:

- **Result**: A discriminated union for handling success/failure scenarios
- **Option**: A type-safe way to handle nullable values
- **Error**: A structured way to represent error information

This library is designed to improve code reliability and readability by providing functional programming patterns in an idiomatic C# way.

## Getting Started

### Dependencies

- .NET 6 or later

### Installation

You can install the YC.Monad package via NuGet:

```bash
dotnet add package YC.Monad
```

## Usage

### Error Type

The `Error` type represents error information with a code, message, and optional HTTP status code:

```csharp
using YC.Monad;

// Create an error with code and message
var error = Error.Create("USER_NOT_FOUND", "The specified user was not found");

// Create an error with HTTP status code
var httpError = Error.Create("UNAUTHORIZED", "Authentication required", 401);

// Common errors are cached for reuse
var notFound = ErrorCache.NotFound;      // 404 Not Found
var badRequest = ErrorCache.BadRequest;   // 400 Bad Request
var unauthorized = ErrorCache.Unauthorized; // 401 Unauthorized
var forbidden = ErrorCache.Forbidden;     // 403 Forbidden
```

### Result Type

The `Result` type represents operations that can succeed or fail:

```csharp
using YC.Monad;

// Result without value
Result Operation()
{
    if (/* success condition */)
        return Result.Success();
    else
        return Error.Create("OPERATION_FAILED", "The operation failed");
}

// Result with value
Result<int> Calculate(int number)
{
    if (number > 0)
        return number * 2; // Implicit conversion from value to success
    else
        return Error.Create("INVALID_INPUT", "Number must be positive"); // Implicit conversion from error to failure
}

// Pattern matching with Result
var result = Calculate(5);
var output = result.Match(
    success => $"Result: {success}",
    error => $"Error: {error.Message}"
);

// Converting between Result types
Result untyped = Result.Success();
Result<int> typed = untyped.ToTypedResult<Result<int>>();
```

### Option Type

The `Option` type provides a safe way to handle nullable values:

```csharp
using YC.Monad;

// Creating Options
Option<string> some = Option<string>.Some("Hello");
Option<string> none = Option<string>.None();
Option<string> fromNullable = Option<string>.Create(nullableString);

// Pattern matching with Option
var greeting = some.Match(
    () => "No greeting available",
    value => $"Greeting: {value}"
);

// LINQ query syntax support
var result =
    from x in Option<int>.Some(5)
    from y in Option<int>.Some(10)
    select x + y;

// Extension methods for collections
var items = new[] { 1, 2, 3, 4, 5 };
var firstEven = items.FirstOrNone(x => x % 2 == 0);
var singleOdd = items.SingleOrNone(x => x == 3);

// Safe value access
if (some.TryGetValue(out var value))
{
    Console.WriteLine(value);
}

// Default value handling
var defaultValue = none.GetValueOrDefault();

// Exception throwing for required values
var requiredValue = some.GetValueOrFail(); // Throws NoneException if none
```

### Functional Extensions

Both `Result` and `Option` types support functional programming patterns:

```csharp
// Mapping
var doubled = Option<int>.Some(5)
    .Map(x => x * 2);

// Binding
var result = Option<int>.Some(5)
    .Bind(x => x > 0
        ? Option<int>.Some(x * 2)
        : Option<int>.None());

// LINQ support
var combined =
    from x in Result<int>.Success(5)
    from y in Result<int>.Success(10)
    select x + y;
```

## Best Practices

1. Use `Result` for operations that can fail with meaningful errors
2. Use `Option` for values that might not exist
3. Leverage pattern matching with `Match` for clean control flow
4. Use the cached errors from `ErrorCache` for common scenarios
5. Take advantage of implicit conversions for cleaner code
6. Use LINQ query syntax for combining multiple Results or Options

## Authors

- [Yusuf Çırak](https://yusufcirak.net)

## License

This project is licensed under the MIT License - see the [LICENSE](docs/LICENSE.txt) file for details.
