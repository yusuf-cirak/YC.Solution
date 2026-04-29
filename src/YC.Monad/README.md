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

### Result Railway (Railroad Oriented Programming)

Railway Oriented Programming (ROP) is a functional pattern for composing operations while automatically propagating failures. The `ResultRailway` extensions enable a linear workflow where errors automatically bypass subsequent operations:

```csharp
using YC.Monad;

// Chain operations with automatic error propagation
Result<User> result = FetchUser(id)
    .Bind(ValidateUser)           // Chain result-returning operations
    .Tap(user => Console.WriteLine($"Processing {user.Name}"))  // Side-effects
    .Map(user => user.Email)      // Transform successful values
    .Ensure(email => !string.IsNullOrEmpty(email), Error.Create("INVALID_EMAIL", "Email is required"))
    .Bind(SendNotification);       // Continue the chain

// Using Bind for chaining operations that return Result
Result<Order> GetUserOrder(int userId) =>
    GetUser(userId)
        .Bind(user => ValidateUser(user))
        .Bind(user => FetchOrdersForUser(user.Id))
        .Bind(orders => orders.Count > 0 
            ? Result<Order>.Success(orders[0]) 
            : Error.Create("NO_ORDERS", "User has no orders"));

// Using Tap for side-effects that don't transform the value
var user = GetUser(id)
    .Tap(u => logger.LogInformation($"User loaded: {u.Id}"))
    .TapError(err => logger.LogError($"Failed to load user: {err.Message}"));

// Using Ensure to validate conditions
Result<int> ValidateAge(int age) =>
    Result.Success(age)
        .Ensure(a => a >= 18, Error.Create("UNDERAGE", "Must be 18 or older"));

// Using Try/TryAsync to capture exceptions
Result<int> ParseNumber(string input) =>
    ResultRailway.Try(
        () => int.Parse(input),
        ex => Error.Create("PARSE_ERROR", $"Invalid number: {ex.Message}")
    );

// Async operations
Task<Result<User>> async_example = GetUserAsync(id)
    .TapAsync(u => logger.LogInformationAsync($"User: {u.Name}"))
    .BindAsync(u => ValidateUserAsync(u))
    .TapErrorAsync(err => SendErrorNotificationAsync(err));
```

**ROP Methods:**

- **Map**: Transforms the value of a successful result
- **MapAsync**: Asynchronous version of Map
- **Bind**: Chains result-returning operations (equivalent to SelectMany)
- **BindAsync**: Asynchronous version of Bind
- **Tap**: Executes a side-effect for successful results without transforming the value
- **TapError**: Executes a side-effect for failed results
- **TapAsync**: Asynchronous version of Tap
- **TapErrorAsync**: Asynchronous version of TapError
- **Ensure**: Validates a condition; converts success to failure if predicate is not satisfied
- **Try**: Executes a function and captures exceptions as a failure result
- **TryAsync**: Asynchronous version of Try

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
