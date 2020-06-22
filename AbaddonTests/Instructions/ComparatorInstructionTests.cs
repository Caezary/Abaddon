using System.Collections.Generic;
using Abaddon;
using Abaddon.Data;
using Abaddon.Exceptions;
using Abaddon.Execution;
using Moq;
using Shouldly;
using Xunit;

namespace AbaddonTests.Instructions
{
    public class ComparatorInstructionTests
    {
        private const string ExampleInitialStateValues = "7";
        private readonly InstructionFactory<int> _factory;

        public ComparatorInstructionTests()
        {
            var comparer = new Mock<IComparer<int>>();
            comparer.Setup(c => c.Compare(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int a, int b) => a - b);
            
            _factory = new InstructionFactory<int>(
                new Mock<IPerformEntryOperations<int>>().Object,
                comparer.Object);
        }

        [Fact]
        public void Execute_PointsToSelf_Throws()
        {
            var context = CreateContext(5, 2);
            var sut = _factory.CreateInstruction("C0");

            Assert.Throws<InvalidComparatorJumpError>(
                () => sut.Execute(context));
            
            context.ExecutionStackPointer.Value.ShouldBe(2);
            context.ExecutionStackPointer.Step.ShouldBe(1);
            context.Accumulator.ShouldBe(5);
        }

        [Fact]
        public void Execute_PositiveInstructionCountAndAccumulatorLowerThanCurrentMemoryValue_JumpsForward()
        {
            var context = CreateContext(5, 2);
            var sut = _factory.CreateInstruction("C2");

            sut.Execute(context);
            
            context.ExecutionStackPointer.Value.ShouldBe(2);
            context.ExecutionStackPointer.Step.ShouldBe(2);
            context.Accumulator.ShouldBe(5);
        }

        [Fact]
        public void Execute_NegativeInstructionCountAndAccumulatorLowerThanCurrentMemoryValue_JumpsBackward()
        {
            var context = CreateContext(5, 2);
            var sut = _factory.CreateInstruction("C-2");

            sut.Execute(context);
            
            context.ExecutionStackPointer.Value.ShouldBe(2);
            context.ExecutionStackPointer.Step.ShouldBe(-2);
            context.Accumulator.ShouldBe(5);
        }

        [Fact]
        public void Execute_PositiveInstructionCountAndAccumulatorHigherThanCurrentMemoryValue_WillNotJump()
        {
            var context = CreateContext(9, 2);
            var sut = _factory.CreateInstruction("C2");

            sut.Execute(context);
            
            context.ExecutionStackPointer.Value.ShouldBe(2);
            context.ExecutionStackPointer.Step.ShouldBe(1);
            context.Accumulator.ShouldBe(9);
        }

        [Fact]
        public void Execute_NegativeInstructionCountAndAccumulatorHigherThanCurrentMemoryValue_WillNotJump()
        {
            var context = CreateContext(9, 2);
            var sut = _factory.CreateInstruction("C-2");

            sut.Execute(context);
            
            context.ExecutionStackPointer.Value.ShouldBe(2);
            context.ExecutionStackPointer.Step.ShouldBe(1);
            context.Accumulator.ShouldBe(9);
        }

        private static CurrentState<int> CreateContext(int accumulatorValue, int stackPointerValue)
        {
            var context = new ContextFactory().CreateInitialState(
                1, 1, ExampleInitialStateValues, Conversions.AsHex);
            context.Accumulator = accumulatorValue;
            context.ExecutionStackPointer.Value = stackPointerValue;
            return context;
        }
    }
}
