using Abaddon;
using Abaddon.Data;
using Abaddon.Exceptions;
using Shouldly;
using Xunit;

namespace AbaddonTests
{
    public class ContextFactoryTests
    {
        private const string ExampleValues = "A16B59";
        private readonly ContextFactory _sut = new ContextFactory();

        [Fact]
        public void CreateInitialStateCalled_EmptyValues_Throws()
        {
            var values = "";
            
            Assert.Throws<StateInitializationError>(
                () => _sut.CreateInitialState(2, 2, values, c => 0));
        }

        [Fact]
        public void CreateInitialStateCalled_ValuesGiven_CreatesExpectedInitialState()
        {
            var result = _sut.CreateInitialState(3, 2, ExampleValues, Conversions.AsHex);

            result.Board.Width.ShouldBe(3);
            result.Board.Height.ShouldBe(2);
            result.Board[0][0].ShouldBe(0xA);
            result.Board[0][1].ShouldBe(0x1);
            result.Board[0][2].ShouldBe(0x6);
            result.Board[1][0].ShouldBe(0xB);
            result.Board[1][1].ShouldBe(0x5);
            result.Board[1][2].ShouldBe(0x9);
        }

        [Fact]
        public void CreateInitialStateCalled_InitialPositionGiven_CreatesStateWithSetPosition()
        {
            var position = new MemoryPosition(1, 2);
            
            var result = _sut.CreateInitialState(
                3, 2, ExampleValues, Conversions.AsHex, position);

            result.Position.Row.ShouldBe(1);
            result.Position.Column.ShouldBe(2);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, 3)]
        [InlineData(4, 4)]
        [InlineData(0, -1)]
        public void CreateInitialStateCalled_FaultyInitialPositionGiven_Throws(int row, int column)
        {
            var position = new MemoryPosition(row, column);

            Assert.Throws<StateInitializationError>(
                () => _sut.CreateInitialState(3, 2, ExampleValues, Conversions.AsHex, position));
        }
    }
}
