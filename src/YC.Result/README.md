# YC.Result

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/YC.Result)](https://www.nuget.org/packages/YC.Result/)


## Description

YC.Result is a .NET library implementing the Result pattern. It provides a consistent way to handle success and failure in your applications, encapsulating error information and result values. This library is designed to improve code readability and error handling by using record structs for results and errors.

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

var successResult = Result.Success();
var failureResult = Result.Failure(new Error("Error Title", "Error Detail", 400));

if (successResult.IsSuccess)
{
    Console.WriteLine("Operation was successful.");
}
else
{
    Console.WriteLine($"Operation failed with error: {successResult.Error.Detail}");
}
```

### Result with Value

```csharp
using YC.Result;

Result<string> GetGreeting(bool isSuccessful)
{
    if (isSuccessful)
    {
        return "Hello, World!";
    }
    else
    {
        return Error.Create("Greeting Error", "Failed to generate greeting.", 500);
    }
}

var result = GetGreeting(true);

result.Match(
    value => Console.WriteLine(value),
    error => Console.WriteLine($"Error: {error.Detail}")
);
```

## Authors
* [Yusuf Çırak](https://yusufcirak.net)


## License
This project is licensed under the MIT License - see the [LICENSE](docs/LICENSE.txt) file for details.