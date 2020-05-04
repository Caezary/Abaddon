using Abaddon;
using Abaddon.Data;
using Abaddon.Exceptions;
using Shouldly;
using Xunit;

namespace AbaddonTests.Instructions
{
    public class MoveInstructionTests
    {
        private const string ExampleInitialStateValues = "3FC21A54B";
        private readonly InstructionFactory _factory = new InstructionFactory();

        [Theory]
        [InlineData(0, 0xF)]
        [InlineData(1, 0x1)]
        [InlineData(2, 0x4)]
        public void ExecuteMoveRightInstruction_CanMoveRight_ExecutesProperly(int row, int expectedValue)
        {
            var context = CreateContext(new MemoryPosition(row, 0));
            var sut = _factory.CreateInstruction<int>("R");

            sut.Execute(context);

            context.Position.Row.ShouldBe(row);
            context.Position.Column.ShouldBe(1);
            context.MarkedEntry.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ExecuteMoveRightInstruction_CanNotMoveRight_Throws(int row)
        {
            var context = CreateContext(new MemoryPosition(row, 2));
            var sut = _factory.CreateInstruction<int>("R");

            Assert.Throws<IllegalMovementException>(() =>
                sut.Execute(context));
            
            context.Position.Row.ShouldBe(row);
            context.Position.Column.ShouldBe(2);
        }

        [Theory]
        [InlineData(0, 0xF)]
        [InlineData(1, 0x1)]
        [InlineData(2, 0x4)]
        public void ExecuteMoveLeftInstruction_CanMoveLeft_ExecutesProperly(int row, int expectedValue)
        {
            var context = CreateContext(new MemoryPosition(row, 2));
            var sut = _factory.CreateInstruction<int>("L");

            sut.Execute(context);

            context.Position.Row.ShouldBe(row);
            context.Position.Column.ShouldBe(1);
            context.MarkedEntry.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ExecuteMoveLeftInstruction_CanNotMoveLeft_Throws(int row)
        {
            var context = CreateContext(new MemoryPosition(row, 0));
            var sut = _factory.CreateInstruction<int>("L");

            Assert.Throws<IllegalMovementException>(() =>
                sut.Execute(context));
            
            context.Position.Row.ShouldBe(row);
            context.Position.Column.ShouldBe(0);
        }

        [Theory]
        [InlineData(0, 0x3)]
        [InlineData(1, 0xF)]
        [InlineData(2, 0xC)]
        public void ExecuteMoveUpInstruction_CanMoveUp_ExecutesProperly(int column, int expectedValue)
        {
            var context = CreateContext(new MemoryPosition(1, column));
            var sut = _factory.CreateInstruction<int>("U");

            sut.Execute(context);
            
            context.Position.Column.ShouldBe(column);
            context.Position.Row.ShouldBe(0);
            context.MarkedEntry.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ExecuteMoveUpInstruction_CanNotMoveUp_Throws(int column)
        {
            var context = CreateContext(new MemoryPosition(0, column));
            var sut = _factory.CreateInstruction<int>("U");

            Assert.Throws<IllegalMovementException>(() =>
                sut.Execute(context));
            
            context.Position.Row.ShouldBe(0);
            context.Position.Column.ShouldBe(column);
        }

        [Theory]
        [InlineData(0, 0x5)]
        [InlineData(1, 0x4)]
        [InlineData(2, 0xB)]
        public void ExecuteMoveDownInstruction_CanMoveDown_ExecutesProperly(int column, int expectedValue)
        {
            var context = CreateContext(new MemoryPosition(1, column));
            var sut = _factory.CreateInstruction<int>("D");

            sut.Execute(context);
            
            context.Position.Column.ShouldBe(column);
            context.Position.Row.ShouldBe(2);
            context.MarkedEntry.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ExecuteMoveDownInstruction_CanNotMoveDown_Throws(int column)
        {
            var context = CreateContext(new MemoryPosition(2, column));
            var sut = _factory.CreateInstruction<int>("D");

            Assert.Throws<IllegalMovementException>(() =>
                sut.Execute(context));
            
            context.Position.Row.ShouldBe(2);
            context.Position.Column.ShouldBe(column);
        }

        private static CurrentState<int> CreateContext(MemoryPosition startPosition = null)
        {
            return new ContextFactory().CreateInitialState(
                3, 3, ExampleInitialStateValues, Conversions.AsHex, startPosition);
        }
    }
}
