using Abaddon;
using Abaddon.Exceptions;
using Shouldly;
using System;
using Xunit;

namespace AbaddonTests
{
    public class ContextFactoryTests
    {
        private readonly ContextFactory _sut;

        public ContextFactoryTests()
        {
            _sut = new ContextFactory();
        }

        [Fact]
        public void CreateInitialStateCalled_EmptyValues_Throws()
        {
            var values = "";
            Assert.Throws<StateInitializationError>(() => _sut.CreateInitialState<int>(2, 2, values, c => 0));
        }

        [Fact]
        public void CreateInitialStateCalled_ValuesGiven_CreatesExpectedInitialState()
        {
            var values = "A16B59";
            var result = _sut.CreateInitialState<int>(3, 2, values, c => ConvertAsHex(c));

            result.Board.Width.ShouldBe(3);
            result.Board.Height.ShouldBe(2);
            result.Board[0][0].ShouldBe(0xA);
            result.Board[0][1].ShouldBe(0x1);
            result.Board[0][2].ShouldBe(0x6);
            result.Board[1][0].ShouldBe(0xB);
            result.Board[1][1].ShouldBe(0x5);
            result.Board[1][2].ShouldBe(0x9);
        }

        private static int ConvertAsHex(char c)
        {
            return int.Parse($"{c}", System.Globalization.NumberStyles.HexNumber);
        }
    }
}
