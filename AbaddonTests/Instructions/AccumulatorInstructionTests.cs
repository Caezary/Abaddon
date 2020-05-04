using Abaddon;
using Abaddon.Data;
using Shouldly;
using Xunit;

namespace AbaddonTests.Instructions
{
    public class AccumulatorInstructionTests
    {
        private const string ExampleInitialStateValues = "0123456789ABCDEF";
        private readonly InstructionFactory _factory = new InstructionFactory();

        [Fact]
        public void CopyToAccumulatorInstruction_TakesValueUnderCurrentPosition_CopiesValueToAccumulator()
        {
            var context = CreateContext(new MemoryPosition(3, 2));
            var sut = _factory.CreateInstruction<int>("A");

            sut.Execute(context);
            
            context.Accumulator.ShouldBe(0xE);
            context.MarkedEntry.ShouldBe(0xE);
        }

        [Fact]
        public void CopyFromAccumulatorInstruction_TakesValueFromAccumulator_CopiesValueToCurrentPosition()
        {
            var context = CreateContext(new MemoryPosition(1, 1));
            context.Accumulator = 0xF;
            var sut = _factory.CreateInstruction<int>("Q");

            sut.Execute(context);
            
            context.Accumulator.ShouldBe(0xF);
            context.MarkedEntry.ShouldBe(0xF);
        }

        [Fact]
        public void SwapInstruction_TakesValueUnderCurrentPosition_ExchangesWithAccumulator()
        {
            var context = CreateContext(new MemoryPosition(0, 2));
            context.Accumulator = 0xD;
            var sut = _factory.CreateInstruction<int>("S");

            sut.Execute(context);
            
            context.Accumulator.ShouldBe(0x2);
            context.MarkedEntry.ShouldBe(0xD);
        }

        private static CurrentState<int> CreateContext(MemoryPosition startPosition = null)
        {
            return new ContextFactory().CreateInitialState(
                4, 4, ExampleInitialStateValues, Conversions.AsHex, startPosition);
        }
    }
}