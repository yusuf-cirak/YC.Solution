using Microsoft.EntityFrameworkCore;
using Xunit;

namespace YC.Monad.EntityFrameworkCore.UnitTests;

public class OptionAsyncExtensionsTests
{
    private TestDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new TestDbContext(options);
    }

    #region FirstOrNoneAsync Tests

    [Fact]
    public async Task FirstOrNoneAsync_WithoutPredicate_WhenElementExists_ReturnsSome()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test1" },
            new TestEntity { Id = 2, Name = "Test2" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await context.TestEntities.FirstOrNoneAsync();

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.NotNull(value);
        Assert.Equal(1, value.Id);
    }

    [Fact]
    public async Task FirstOrNoneAsync_WithoutPredicate_WhenNoElements_ReturnsNone()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var result = await context.TestEntities.FirstOrNoneAsync();

        // Assert
        Assert.False(result.TryGetValue(out _));
    }

    [Fact]
    public async Task FirstOrNoneAsync_WithPredicate_WhenMatchingElementExists_ReturnsSome()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test1" },
            new TestEntity { Id = 2, Name = "Test2" },
            new TestEntity { Id = 3, Name = "Test3" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await context.TestEntities.FirstOrNoneAsync(e => e.Name == "Test2");

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(2, value.Id);
        Assert.Equal("Test2", value.Name);
    }

    [Fact]
    public async Task FirstOrNoneAsync_WithPredicate_WhenNoMatchingElement_ReturnsNone()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test1" },
            new TestEntity { Id = 2, Name = "Test2" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await context.TestEntities.FirstOrNoneAsync(e => e.Name == "NonExistent");

        // Assert
        Assert.False(result.TryGetValue(out _));
    }

    [Fact]
    public async Task FirstOrNoneAsync_WithCancellationToken_CanBeCancelled()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await context.TestEntities.FirstOrNoneAsync(cts.Token));
    }

    [Fact]
    public async Task FirstOrNoneAsync_WithPredicateAndCancellationToken_CanBeCancelled()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await context.TestEntities.FirstOrNoneAsync(e => e.Name == "Test", cts.Token));
    }

    #endregion

    #region SingleOrNoneAsync Tests

    [Fact]
    public async Task SingleOrNoneAsync_WithoutPredicate_WhenSingleElementExists_ReturnsSome()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
        await context.SaveChangesAsync();

        // Act
        var result = await context.TestEntities.SingleOrNoneAsync();

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(1, value.Id);
    }

    [Fact]
    public async Task SingleOrNoneAsync_WithoutPredicate_WhenNoElements_ReturnsNone()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var result = await context.TestEntities.SingleOrNoneAsync();

        // Assert
        Assert.False(result.TryGetValue(out _));
    }

    [Fact]
    public async Task SingleOrNoneAsync_WithoutPredicate_WhenMultipleElements_ThrowsException()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test1" },
            new TestEntity { Id = 2, Name = "Test2" }
        );
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await context.TestEntities.SingleOrNoneAsync());
    }

    [Fact]
    public async Task SingleOrNoneAsync_WithPredicate_WhenSingleMatchingElementExists_ReturnsSome()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test1" },
            new TestEntity { Id = 2, Name = "Test2" },
            new TestEntity { Id = 3, Name = "Test3" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await context.TestEntities.SingleOrNoneAsync(e => e.Name == "Test2");

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(2, value.Id);
        Assert.Equal("Test2", value.Name);
    }

    [Fact]
    public async Task SingleOrNoneAsync_WithPredicate_WhenNoMatchingElement_ReturnsNone()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
        await context.SaveChangesAsync();

        // Act
        var result = await context.TestEntities.SingleOrNoneAsync(e => e.Name == "NonExistent");

        // Assert
        Assert.False(result.TryGetValue(out _));
    }

    [Fact]
    public async Task SingleOrNoneAsync_WithPredicate_WhenMultipleMatchingElements_ThrowsException()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test" },
            new TestEntity { Id = 2, Name = "Test" }
        );
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await context.TestEntities.SingleOrNoneAsync(e => e.Name == "Test"));
    }

    [Fact]
    public async Task SingleOrNoneAsync_WithCancellationToken_CanBeCancelled()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await context.TestEntities.SingleOrNoneAsync(cts.Token));
    }

    #endregion

    #region LastOrNoneAsync Tests

    [Fact]
    public async Task LastOrNoneAsync_WithoutPredicate_WhenElementExists_ReturnsSome()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test1" },
            new TestEntity { Id = 2, Name = "Test2" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await context.TestEntities.OrderBy(e => e.Id).LastOrNoneAsync();

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(2, value.Id);
    }

    [Fact]
    public async Task LastOrNoneAsync_WithoutPredicate_WhenNoElements_ReturnsNone()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var result = await context.TestEntities.LastOrNoneAsync();

        // Assert
        Assert.False(result.TryGetValue(out _));
    }

    [Fact]
    public async Task LastOrNoneAsync_WithPredicate_WhenMatchingElementExists_ReturnsSome()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test1" },
            new TestEntity { Id = 2, Name = "Test2" },
            new TestEntity { Id = 3, Name = "Test1" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await context.TestEntities.OrderBy(e => e.Id).LastOrNoneAsync(e => e.Name == "Test1");

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(3, value.Id);
        Assert.Equal("Test1", value.Name);
    }

    [Fact]
    public async Task LastOrNoneAsync_WithPredicate_WhenNoMatchingElement_ReturnsNone()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test1" },
            new TestEntity { Id = 2, Name = "Test2" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await context.TestEntities.LastOrNoneAsync(e => e.Name == "NonExistent");

        // Assert
        Assert.False(result.TryGetValue(out _));
    }

    [Fact]
    public async Task LastOrNoneAsync_WithCancellationToken_CanBeCancelled()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await context.TestEntities.LastOrNoneAsync(cts.Token));
    }

    [Fact]
    public async Task LastOrNoneAsync_WithPredicateAndCancellationToken_CanBeCancelled()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await context.TestEntities.LastOrNoneAsync(e => e.Name == "Test", cts.Token));
    }

    #endregion

    #region FindOrNoneAsync Tests

    [Fact]
    public async Task FindOrNoneAsync_WithKeyValues_WhenEntityExists_ReturnsSome()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        // Act
        var result = await context.TestEntities.FindOrNoneAsync(1);

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(1, value.Id);
        Assert.Equal("Test1", value.Name);
    }

    [Fact]
    public async Task FindOrNoneAsync_WithKeyValues_WhenEntityDoesNotExist_ReturnsNone()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var result = await context.TestEntities.FindOrNoneAsync(999);

        // Assert
        Assert.False(result.TryGetValue(out _));
    }

    [Fact]
    public async Task FindOrNoneAsync_WithCancellationToken_WhenEntityExists_ReturnsSome()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        // Act
        var result = await context.TestEntities.FindOrNoneAsync(new object[] { 1 }, CancellationToken.None);

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(1, value.Id);
    }

    [Fact]
    public async Task FindOrNoneAsync_WithCancellationToken_WhenEntityDoesNotExist_ReturnsNone()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var result = await context.TestEntities.FindOrNoneAsync(new object[] { 999 }, CancellationToken.None);

        // Assert
        Assert.False(result.TryGetValue(out _));
    }

    [Fact]
    public async Task FindOrNoneAsync_WithCancellationToken_CanBeCancelled()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await context.TestEntities.FindOrNoneAsync(new object[] { 1 }, cts.Token));
    }

    [Fact]
    public async Task FindOrNoneAsync_WithCompositeKey_WhenEntityExists_ReturnsSome()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.CompositeKeyEntities.Add(new CompositeKeyEntity { Id1 = 1, Id2 = 2, Name = "Test" });
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        // Act
        var result = await context.CompositeKeyEntities.FindOrNoneAsync(1, 2);

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(1, value.Id1);
        Assert.Equal(2, value.Id2);
        Assert.Equal("Test", value.Name);
    }

    [Fact]
    public async Task FindOrNoneAsync_WithCompositeKey_WhenEntityDoesNotExist_ReturnsNone()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act
        var result = await context.CompositeKeyEntities.FindOrNoneAsync(999, 999);

        // Assert
        Assert.False(result.TryGetValue(out _));
    }

    [Fact]
    public async Task FindOrNoneAsync_WithMultipleKeys_ReturnsMostRecentEntity()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test1" },
            new TestEntity { Id = 2, Name = "Test2" },
            new TestEntity { Id = 3, Name = "Test3" }
        );
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        // Act
        var result = await context.TestEntities.FindOrNoneAsync(2);

        // Assert
        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(2, value.Id);
        Assert.Equal("Test2", value.Name);
    }

    #endregion

    #region Test Models and Context

    public class TestEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class CompositeKeyEntity
    {
        public int Id1 { get; set; }
        public int Id2 { get; set; }
        public string? Name { get; set; }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; } = null!;
        public DbSet<CompositeKeyEntity> CompositeKeyEntities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<CompositeKeyEntity>()
                .HasKey(e => new { e.Id1, e.Id2 });
        }
    }

    #endregion
}

