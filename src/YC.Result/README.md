# YC.Result

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/YC.Result)](https://www.nuget.org/packages/YC.Result/)


## Description

YC.Result is a .NET library implementing the Result pattern. It provides a consistent way to handle success and failure in your applications, encapsulating error information and result values. This library is designed to improve code readability and error handling by using records for results and errors.

## Getting Started

### Dependencies

* .NET 6 or later

### Installation

You can install the YC.Result package via NuGet:

```bash
dotnet add package YC.Result
```

## Usage

### Result without Value

```csharp
using YC.Result;

Result MyOperation(bool isSuccessful)
{
    if (isSuccessful)
    {
        return Result.Success(); // cached success result
        // Same for Result.Failure() for cached empty failure result
    }
    else
    {
        return Error.Create("Operation Error", "Failed to complete operation.", 500);
    }
}

var result = MyOperation(false);

result.Match(
    () => Console.WriteLine("Operation was successful."),
    error => Console.WriteLine($"Operation failed with error: {error.Detail}")
);
```

### Result with Value

```csharp
using YC.Result;
using Microsoft.AspNetCore.Http;

Result<string> GetGreeting(bool isSuccessful)
{
    if (isSuccessful)
    {
        return "Hello, World!"; // creating successful result implicitly
    }
    else
    {
        return Error.Create("Greeting Error", "Failed to generate greeting.", 500); // creating failure result implicitly
    }
}

var result = GetGreeting(false);

var httpResult = result.Match(
    value => Results.Ok(value),
    error => Results.StatusCode(error.Status)
);
```

## Authors
* [Yusuf Çırak](https://yusufcirak.net)


## License
This project is licensed under the MIT License - see the [LICENSE](docs/LICENSE.txt) file for details.