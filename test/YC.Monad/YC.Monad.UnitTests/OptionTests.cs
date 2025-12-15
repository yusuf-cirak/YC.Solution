﻿using Xunit;

namespace YC.Monad.UnitTests
{
    public class OptionTests
    {
        [Fact]
        public void Option_Create_WithValue_ReturnsSome()
        {
            // Arrange
            var value = 42;

            // Act
            var option = Option<int>.Create(value);

            // Assert
            Assert.True(option.TryGetValue(out var result));
            Assert.Equal(value, result);
        }

        [Fact]
        public void Option_Create_WithNull_ReturnsNone()
        {
            // Act
            var option = Option<string>.Create(null);

            // Assert
            Assert.False(option.TryGetValue(out _));
        }

        [Fact]
        public void Option_None_ReturnsNoneInstance()
        {
            // Act
            var option = Option<int>.None();

            // Assert
            Assert.False(option.TryGetValue(out _));
        }

        [Fact]
        public void Option_Match_ReturnsSomeOrNoneCorrectly()
        {
            // Arrange
            var someOption = Option<int>.Some(10);
            var noneOption = Option<int>.None();

            // Act
            var someResult = someOption.Match(() => -1, value => value * 2);
            var noneResult = noneOption.Match(() => -1, value => value * 2);

            // Assert
            Assert.Equal(20, someResult);
            Assert.Equal(-1, noneResult);
        }

        [Fact]
        public void Option_Map_TransformsValueWhenSome()
        {
            // Arrange
            var option = Option<int>.Some(5);

            // Act
            var mappedOption = option.Map(x => x * 2);

            // Assert
            Assert.True(mappedOption.TryGetValue(out var result));
            Assert.Equal(10, result);
        }

        [Fact]
        public void Option_Map_DoesNothingWhenNone()
        {
            // Arrange
            var option = Option<int>.None();

            // Act
            var mappedOption = option.Map(x => x * 2);

            // Assert
            Assert.False(mappedOption.TryGetValue(out _));
        }

        [Fact]
        public void Option_Bind_ChainsOptionValues()
        {
            // Arrange
            var option = Option<int>.Some(5);

            // Act
            var boundOption = option.Bind(x => Option<string>.Some($"Value is {x}"));

            // Assert
            Assert.True(boundOption.TryGetValue(out var result));
            Assert.Equal("Value is 5", result);
        }

        [Fact]
        public void Option_Bind_ReturnsNoneIfOriginalIsNone()
        {
            // Arrange
            var option = Option<int>.None();

            // Act
            var boundOption = option.Bind(x => Option<string>.Some($"Value is {x}"));

            // Assert
            Assert.False(boundOption.TryGetValue(out _));
        }

        [Fact]
        public void Option_ImplicitConversion_FromValue_ReturnsSome()
        {
            // Arrange
            int value = 100;

            // Act
            Option<int> option = value;

            // Assert
            Assert.True(option.TryGetValue(out var result));
            Assert.Equal(value, result);
        }

        [Fact]
        public void Option_ImplicitConversion_FromNull_ReturnsNone()
        {
            // Arrange
            string? value = null;

            // Act
            Option<string> option = value;

            // Assert
            Assert.False(option.TryGetValue(out _));
        }
    }

    public class OptionExtensionsTests
    {
        [Fact]
        public void FirstOrNone_FindsFirstMatchingValue()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            var option = list.FirstOrNone(x => x > 1);

            // Assert
            Assert.True(option.TryGetValue(out var result));
            Assert.Equal(2, result);
        }

        [Fact]
        public void FirstOrNone_ReturnsNoneWhenNoMatchFound()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            var option = list.FirstOrNone(x => x > 5);

            // Assert
            Assert.False(option.TryGetValue(out _));
        }

        [Fact]
        public void SingleOrNone_FindsSingleMatchingValue()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            var option = list.SingleOrNone(x => x == 2);

            // Assert
            Assert.True(option.TryGetValue(out var result));
            Assert.Equal(2, result);
        }

        [Fact]
        public void SingleOrNone_ReturnsNoneWhenNoMatchFound()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            var option = list.SingleOrNone(x => x == 5);

            // Assert
            Assert.False(option.TryGetValue(out _));
        }

        [Fact]
        public void Where_ReturnsSameOptionIfPredicateIsTrue()
        {
            // Arrange
            var option = Option<int>.Some(10);

            // Act
            var filteredOption = option.Where(x => x > 5);

            // Assert
            Assert.True(filteredOption.TryGetValue(out var result));
            Assert.Equal(10, result);
        }

        [Fact]
        public void Where_ReturnsNoneIfPredicateIsFalse()
        {
            // Arrange
            var option = Option<int>.Some(10);

            // Act
            var filteredOption = option.Where(x => x < 5);

            // Assert
            Assert.False(filteredOption.TryGetValue(out _));
        }

        [Fact]
        public void SelectMany_ChainsMultipleOptions()
        {
            // Arrange
            var option1 = Option<int>.Some(10);
            var option2 = Option<string>.Some("Success");

            // Act
            var resultOption = option1.SelectMany(
                value1 => option2,
                (value1, value2) => $"{value2}: {value1}");

            // Assert
            Assert.True(resultOption.TryGetValue(out var result));
            Assert.Equal("Success: 10", result);
        }
    }

    public class OptionLinqTests
    {
        [Fact]
        public void Select_OnSome_TransformsValue()
        {
            // Arrange
            var option = Option<int>.Some(5);

            // Act
            var mapped = option.Select(x => x * 2);

            // Assert
            Assert.True(mapped.TryGetValue(out var result));
            Assert.Equal(10, result);
        }

        [Fact]
        public void Select_OnNone_ReturnsNone()
        {
            // Arrange
            var option = Option<int>.None();

            // Act
            var mapped = option.Select(x => x * 2);

            // Assert
            Assert.False(mapped.TryGetValue(out _));
        }

        [Fact]
        public void LinqQuery_WithTwoSomeOptions_CombinesValues()
        {
            // Arrange
            var option1 = Option<int>.Some(5);
            var option2 = Option<int>.Some(10);

            // Act
            var result =
                from x in option1
                from y in option2
                select x + y;

            // Assert
            Assert.True(result.TryGetValue(out var value));
            Assert.Equal(15, value);
        }

        [Fact]
        public void LinqQuery_WithFirstNone_ReturnsNone()
        {
            // Arrange
            var option1 = Option<int>.None();
            var option2 = Option<int>.Some(10);

            // Act
            var result =
                from x in option1
                from y in option2
                select x + y;

            // Assert
            Assert.False(result.TryGetValue(out _));
        }

        [Fact]
        public void LinqQuery_WithSecondNone_ReturnsNone()
        {
            // Arrange
            var option1 = Option<int>.Some(5);
            var option2 = Option<int>.None();

            // Act
            var result =
                from x in option1
                from y in option2
                select x + y;

            // Assert
            Assert.False(result.TryGetValue(out _));
        }

        [Fact]
        public void LinqQuery_WithWhere_FiltersCorrectly()
        {
            // Arrange
            var option = Option<int>.Some(10);

            // Act
            var result =
                from x in option
                where x > 5
                select x * 2;

            // Assert
            Assert.True(result.TryGetValue(out var value));
            Assert.Equal(20, value);
        }

        [Fact]
        public void LinqQuery_WithFailingWhere_ReturnsNone()
        {
            // Arrange
            var option = Option<int>.Some(3);

            // Act
            var result =
                from x in option
                where x > 5
                select x * 2;

            // Assert
            Assert.False(result.TryGetValue(out _));
        }

        [Fact]
        public void ComplexLinqQuery_WithMultipleOperations_WorksCorrectly()
        {
            // Arrange
            var option1 = Option<int>.Some(5);
            var option2 = Option<int>.Some(10);

            // Act
            var result =
                from x in option1
                from y in option2
                let sum = x + y
                where sum > 10
                select sum * 2;

            // Assert
            Assert.True(result.TryGetValue(out var value));
            Assert.Equal(30, value);
        }

        [Fact]
        public void SelectMany_WithNone_PropagatesNone()
        {
            // Arrange
            var option1 = Option<int>.Some(5);
            var option2 = Option<int>.None();

            // Act
            var result = option1.SelectMany(
                x => option2,
                (x, y) => x + y);

            // Assert
            Assert.False(result.TryGetValue(out _));
        }

        [Fact]
        public void Where_OnNone_ReturnsNone()
        {
            // Arrange
            var option = Option<int>.None();

            // Act
            var filtered = option.Where(x => x > 5);

            // Assert
            Assert.False(filtered.TryGetValue(out _));
        }

        [Fact]
        public void ChainedLinqOperations_WorkCorrectly()
        {
            // Arrange
            var option = Option<int>.Some(5);

            // Act
            var result = option
                .Select(x => x * 2)
                .Where(x => x > 5)
                .Select(x => x.ToString());

            // Assert
            Assert.True(result.TryGetValue(out var value));
            Assert.Equal("10", value);
        }
    }
}
