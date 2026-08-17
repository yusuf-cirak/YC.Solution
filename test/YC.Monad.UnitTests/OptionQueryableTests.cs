using System.Linq;
using Xunit;

namespace YC.Monad.UnitTests
{
    public class OptionQueryableTests
    {
        [Fact]
        public void FirstOrNone_WithElements_ReturnsFirstElement()
        {
            // Arrange
            var source = new[] { 1, 2, 3 }.AsQueryable();

            // Act
            var option = source.FirstOrNone();

            // Assert
            Assert.True(option.TryGetValue(out var value));
            Assert.Equal(1, value);
        }

        [Fact]
        public void FirstOrNone_OnEmptySequenceOfValueType_ReturnsNone()
        {
            // Regression test: FirstOrDefault() on an empty IQueryable<int> returns 0,
            // which must not be mistaken for a real Some(0) value.

            // Arrange
            var source = System.Array.Empty<int>().AsQueryable();

            // Act
            var option = source.FirstOrNone();

            // Assert
            Assert.False(option.TryGetValue(out _));
        }

        [Fact]
        public void FirstOrNone_OnEmptySequenceOfReferenceType_ReturnsNone()
        {
            // Arrange
            var source = System.Array.Empty<string>().AsQueryable();

            // Act
            var option = source.FirstOrNone();

            // Assert
            Assert.False(option.TryGetValue(out _));
        }

        [Fact]
        public void FirstOrNonePredicate_WithMatch_ReturnsMatchingElement()
        {
            // Arrange
            var source = new[] { 1, 2, 3 }.AsQueryable();

            // Act
            var option = source.FirstOrNone(x => x > 1);

            // Assert
            Assert.True(option.TryGetValue(out var value));
            Assert.Equal(2, value);
        }

        [Fact]
        public void FirstOrNonePredicate_OnValueTypeWithNoMatch_ReturnsNone()
        {
            // Regression test: same default-value trap as the no-predicate overload,
            // triggered when the predicate matches nothing.

            // Arrange
            var source = new[] { 1, 2, 3 }.AsQueryable();

            // Act
            var option = source.FirstOrNone(x => x > 100);

            // Assert
            Assert.False(option.TryGetValue(out _));
        }

        [Fact]
        public void SingleOrNone_WithExactlyOneMatch_ReturnsThatElement()
        {
            // Arrange
            var source = new[] { 1, 2, 3 }.AsQueryable();

            // Act
            var option = source.SingleOrNone(x => x == 2);

            // Assert
            Assert.True(option.TryGetValue(out var value));
            Assert.Equal(2, value);
        }

        [Fact]
        public void SingleOrNone_WithNoMatch_ReturnsNone()
        {
            // Arrange
            var source = new[] { 1, 2, 3 }.AsQueryable();

            // Act
            var option = source.SingleOrNone(x => x == 5);

            // Assert
            Assert.False(option.TryGetValue(out _));
        }

        [Fact]
        public void SingleOrNone_WithMultipleMatches_ReturnsNone()
        {
            // Regression test: must not silently return the first of several matches.

            // Arrange
            var source = new[] { 1, 2, 2, 3 }.AsQueryable();

            // Act
            var option = source.SingleOrNone(x => x == 2);

            // Assert
            Assert.False(option.TryGetValue(out _));
        }
    }
}
