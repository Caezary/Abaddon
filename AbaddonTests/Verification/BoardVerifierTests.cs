using System.Collections.Generic;
using Abaddon;
using Abaddon.Data;
using Abaddon.Exceptions;
using Abaddon.Verification;
using Moq;
using Shouldly;
using Xunit;

namespace AbaddonTests.Verification
{
    public class BoardVerifierTests
    {
        private readonly ContextFactory _contextFactory = new ContextFactory();
        private readonly Mock<IEqualityComparer<int>> _equalityComparer = new Mock<IEqualityComparer<int>>();

        public BoardVerifierTests()
        {
            _equalityComparer.Setup(c => c.Equals(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int x, int y) => x == y);
        }
        
        [Fact]
        public void VerifyCalled_ExpectedBoardDifferentSizeThanActual_Throws()
        {
            var expected = CreateExpectedBoard(2, "ABCD");
            var actual = CreateBoard(1, "A");
            var sut = CreateSut(expected);

            void Act() => sut.Verify(actual);

            Assert.Throws<BoardSizesMismatchError>(Act);
        }

        [Fact]
        public void VerifyCalled_OneCellBoardWithValueDifferentThanExpected_ReturnsFalse()
        {
            var expected = CreateExpectedBoard(1, "D");
            var actual = CreateBoard(1, "A");
            var sut = CreateSut(expected);

            var result = sut.Verify(actual);

            result.ShouldBeFalse();
        }

        [Fact]
        public void VerifyCalled_OneCellBoardWithValueSameAsExpected_ReturnsTrue()
        {
            var expected = CreateExpectedBoard(1, "C");
            var actual = CreateBoard(1, "C");
            var sut = CreateSut(expected);

            var result = sut.Verify(actual);

            result.ShouldBeTrue();
        }
        
        [Fact]
        public void VerifyCalled_TwoByTwoBoardWithValuesDifferentThanExpected_ReturnsFalse()
        {
            var expected = CreateExpectedBoard(2, "ABCD");
            var actual = CreateBoard(2, "ABCE");
            var sut = CreateSut(expected);

            var result = sut.Verify(actual);

            result.ShouldBeFalse();
        }
        
        [Fact]
        public void VerifyCalled_TwoByTwoBoardWithValuesSameAsExpected_ReturnsTrue()
        {
            var expected = CreateExpectedBoard(2, "ABCD");
            var actual = CreateBoard(2, "ABCD");
            var sut = CreateSut(expected);

            var result = sut.Verify(actual);

            result.ShouldBeTrue();
        }
        
        [Fact]
        public void VerifyCalled_TwoByThreeBoardWithValuesDifferentThanExpected_ReturnsFalse()
        {
            var expected = CreateExpectedBoard(2, "123456");
            var actual = CreateBoard(2, "1234E6");
            var sut = CreateSut(expected);

            var result = sut.Verify(actual);

            result.ShouldBeFalse();
        }
        
        [Fact]
        public void VerifyCalled_TwoByThreeBoardWithValuesSameAsExpected_ReturnsTrue()
        {
            var expected = CreateExpectedBoard(2, "123456");
            var actual = CreateBoard(2, "123456");
            var sut = CreateSut(expected);

            var result = sut.Verify(actual);

            result.ShouldBeTrue();
        }
        
        [Fact]
        public void VerifyCalled_ThreeByThreeBoardWithValuesDifferentThanExpectedAndSomeValuesIgnored_ReturnsFalse()
        {
            var expected = CreateExpectedBoard(3, "1XXX5XXX9");
            var actual = CreateBoard(3, "1234E6789");
            var sut = CreateSut(expected);

            var result = sut.Verify(actual);

            result.ShouldBeFalse();
        }
        
        [Fact]
        public void VerifyCalled_ThreeByThreeBoardWithValuesSameAsExpectedAndSomeValuesIgnored_ReturnsTrue()
        {
            var expected = CreateExpectedBoard(3, "1XXX5XXX9");
            var actual = CreateBoard(3, "123456789");
            var sut = CreateSut(expected);

            var result = sut.Verify(actual);

            result.ShouldBeTrue();
        }

        private BoardVerifier<int> CreateSut(Board<Ignorable<int>> expected)
        {
            return new BoardVerifier<int>(expected, _equalityComparer.Object);
        }

        private Board<int> CreateBoard(int width, string values) =>
            _contextFactory.CreateBoard(width, values, Conversions.AsHex);
        
        private Board<Ignorable<int>> CreateExpectedBoard(int width, string values) =>
            _contextFactory.CreateBoard(width, values, ToIgnorableEntry);

        private static Ignorable<int> ToIgnorableEntry(char c)
        {
            return c == 'X'
                ? new Ignorable<int>()
                : new Ignorable<int>(Conversions.AsHex(c));
        }
    }
}
