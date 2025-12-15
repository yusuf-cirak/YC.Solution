<!-- @format -->

# YC.Monad.EntityFrameworkCore

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/YC.Monad.EntityFrameworkCore)](https://www.nuget.org/packages/YC.Monad.EntityFrameworkCore/)

## Description

YC.Monad.EntityFrameworkCore is an extension library for [YC.Monad](https://www.nuget.org/packages/YC.Monad/) that provides Entity Framework Core integration. It offers async extension methods that seamlessly convert EF Core query results into `Option<T>` types, making it easier to work with potentially absent data in a type-safe, functional way.

## Features

- **Async Option Extensions**: Convert EF Core query results to `Option<T>` types
- **FirstOrNoneAsync**: Safe alternative to `FirstOrDefaultAsync`
- **SingleOrNoneAsync**: Safe alternative to `SingleOrDefaultAsync`
- **LastOrNoneAsync**: Safe alternative to `LastOrDefaultAsync`
- **FindOrNoneAsync**: Safe alternative to `FindAsync` for DbSet operations
- **Type-Safe**: No more null reference exceptions
- **Cancellation Token Support**: Full support for async cancellation

## Getting Started

### Dependencies

- .NET 6 or later
- YC.Monad 1.1.0 or later
- Microsoft.EntityFrameworkCore 6.0 or later

### Installation

You can install the YC.Monad.EntityFrameworkCore package via NuGet:

```bash
dotnet add package YC.Monad.EntityFrameworkCore
```

Or via Package Manager Console:

```powershell
Install-Package YC.Monad.EntityFrameworkCore
```

## Usage

### FirstOrNoneAsync

Retrieves the first element of a query or returns `None` if no element is found:

```csharp
using YC.Monad;
using YC.Monad.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Without predicate
var user = await dbContext.Users
    .FirstOrNoneAsync();

user.Match(
    some: u => Console.WriteLine($"Found: {u.Name}"),
    none: () => Console.WriteLine("No user found")
);

// With predicate
var activeUser = await dbContext.Users
    .FirstOrNoneAsync(u => u.IsActive);

// With cancellation token
var user = await dbContext.Users
    .FirstOrNoneAsync(cancellationToken);

// With predicate and cancellation token
var user = await dbContext.Users
    .FirstOrNoneAsync(u => u.Age > 18, cancellationToken);
```

### SingleOrNoneAsync

Retrieves the single element that matches the query or returns `None` if no element is found. Throws if more than one element exists:

```csharp
// Find a unique user by email
var user = await dbContext.Users
    .SingleOrNoneAsync(u => u.Email == "user@example.com");

user.Match(
    some: u => Console.WriteLine($"User ID: {u.Id}"),
    none: () => Console.WriteLine("User not found")
);

// Without predicate
var singleUser = await dbContext.Users
    .Where(u => u.Id == userId)
    .SingleOrNoneAsync();

// With cancellation token
var user = await dbContext.Users
    .SingleOrNoneAsync(u => u.Username == "admin", cancellationToken);
```

### LastOrNoneAsync

Retrieves the last element of a query or returns `None` if no element is found:

```csharp
// Get the most recent order
var lastOrder = await dbContext.Orders
    .OrderBy(o => o.CreatedAt)
    .LastOrNoneAsync();

lastOrder.Match(
    some: o => Console.WriteLine($"Last order: {o.Id}"),
    none: () => Console.WriteLine("No orders found")
);

// With predicate
var lastActiveOrder = await dbContext.Orders
    .LastOrNoneAsync(o => o.Status == OrderStatus.Active);

// With cancellation token
var lastOrder = await dbContext.Orders
    .LastOrNoneAsync(o => o.CustomerId == customerId, cancellationToken);
```

### FindOrNoneAsync

Finds an entity by its primary key or returns `None` if not found:

```csharp
// Find by single key
var user = await dbContext.Users
    .FindOrNoneAsync(userId);

// Find by composite key
var orderItem = await dbContext.OrderItems
    .FindOrNoneAsync(orderId, productId);

// With cancellation token
var user = await dbContext.Users
    .FindOrNoneAsync(new object[] { userId }, cancellationToken);

// Usage with pattern matching
await dbContext.Products
    .FindOrNoneAsync(productId)
    .Match(
        some: async product => 
        {
            product.Stock -= quantity;
            await dbContext.SaveChangesAsync();
            Console.WriteLine("Stock updated");
        },
        none: () => Console.WriteLine("Product not found")
    );
```

## Complete Examples

### Repository Pattern with Option

```csharp
public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Option<User>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Users.FindOrNoneAsync(id, ct);
    }

    public async Task<Option<User>> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Users
            .FirstOrNoneAsync(u => u.Email == email, ct);
    }

    public async Task<Option<User>> GetActiveUserByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _context.Users
            .Where(u => u.IsActive)
            .SingleOrNoneAsync(u => u.Username == username, ct);
    }
}
```

### Service Layer with Railway-Oriented Programming

```csharp
public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Order>> CreateOrderAsync(int userId, List<int> productIds)
    {
        var user = await _context.Users.FindOrNoneAsync(userId);

        return await user
            .ToResult(Error.Create("USER_NOT_FOUND", "User not found"))
            .BindAsync(async u => await ValidateUserAsync(u))
            .BindAsync(async u => await CreateOrderForUserAsync(u, productIds));
    }

    private async Task<Result<User>> ValidateUserAsync(User user)
    {
        if (!user.IsActive)
            return Result<User>.Failure(Error.Create("USER_INACTIVE", "User is not active"));

        return Result<User>.Success(user);
    }

    private async Task<Result<Order>> CreateOrderForUserAsync(User user, List<int> productIds)
    {
        var order = new Order { UserId = user.Id, CreatedAt = DateTime.UtcNow };
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Result<Order>.Success(order);
    }
}
```

### Query with Optional Filtering

```csharp
public async Task<List<Product>> GetProductsAsync(
    Option<string> category,
    Option<decimal> minPrice,
    CancellationToken ct = default)
{
    var query = _context.Products.AsQueryable();

    // Apply optional filters
    query = category.Match(
        some: cat => query.Where(p => p.Category == cat),
        none: () => query
    );

    query = minPrice.Match(
        some: price => query.Where(p => p.Price >= price),
        none: () => query
    );

    return await query.ToListAsync(ct);
}
```

## Benefits

### Type Safety

Instead of dealing with null values:

```csharp
// Traditional approach - null checks required
var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
if (user != null)
{
    // Use user
}
else
{
    // Handle null case
}
```

Use Option types for explicit handling:

```csharp
// With Option - explicit and type-safe
var userOption = await dbContext.Users.FirstOrNoneAsync(u => u.Id == userId);

userOption.Match(
    some: user => ProcessUser(user),
    none: () => HandleNotFound()
);
```

### Railway-Oriented Programming

Chain operations safely without null checking:

```csharp
var result = await dbContext.Users
    .FirstOrNoneAsync(u => u.Email == email)
    .MapAsync(async user => await UpdateUserAsync(user))
    .BindAsync(async user => await SendEmailAsync(user))
    .ToResult(Error.Create("USER_NOT_FOUND", "User not found"));
```

## Related Packages

- [YC.Monad](https://www.nuget.org/packages/YC.Monad/) - Core functional programming types (Result, Option, Error)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Author

**Yusuf Çırak**

- GitHub: [@yusuf-cirak](https://github.com/yusuf-cirak)

## Support

If you encounter any issues or have questions, please [open an issue](https://github.com/yusuf-cirak/YC.Solution/issues) on GitHub.

