using Abaddon;
using Abaddon.Data;
using Abaddon.Exceptions;
using Abaddon.Execution;
using Moq;
using Shouldly;
using Xunit;

namespace AbaddonTests.Instructions
{
    public class JumpInstructionTests
    {
        private const string ExampleInitialStateValues = "E";
        private readonly Mock<IPerformEntryOperations<int>> _entryOperatorMock =
            new Mock<IPerformEntryOperations<int>>();
        private readonly InstructionFactory<int> _factory;

        public JumpInstructionTests()
        {
            _entryOperatorMock
                .Setup(o => o.Decrease(It.IsAny<int>()))
                .Returns<int>(e => e - 1);
            _entryOperatorMock
                .Setup(o => o.IsZero(It.IsAny<int>()))
                .Returns<int>(e => e == 0);
            _factory = new InstructionFactory<int>(_entryOperatorMock.Object);
        }

        [Fact]
        public void ExecuteBackwardJump_ExecutionStackPointerPointsAtStart_Throws()
        {
            var context = CreateContext(5, 0);
            var sut = _factory.CreateInstruction("J5");

            Assert.Throws<InvalidJumpError>(
                () => sut.Execute(context));
            
            context.ExecutionStackPointer.Value.ShouldBe(0);
            context.ExecutionStackPointer.Step.ShouldBe(1);
            context.Accumulator.ShouldBe(5);
        }

        [Fact]
        public void ExecuteBackwardJump_PointsToSelf_Throws()
        {
            var context = CreateContext(5, 2);
            var sut = _factory.CreateInstruction("J0");

            Assert.Throws<InvalidJumpError>(
                () => sut.Execute(context));
            
            context.ExecutionStackPointer.Value.ShouldBe(2);
            context.ExecutionStackPointer.Step.ShouldBe(1);
            context.Accumulator.ShouldBe(5);
        }

        [Fact]
        public void ExecuteBackwardJump_PointsThreeInstructionsBackAndAccumulatorIsNonzero_MarksStackPointerToBeDecreasedAndAccumulatorIsDecreased()
        {
            var context = CreateContext(5, 7);
            var sut = _factory.CreateInstruction("J3");

            sut.Execute(context);
            
            context.ExecutionStackPointer.Value.ShouldBe(7);
            context.ExecutionStackPointer.Step.ShouldBe(-3);
            context.Accumulator.ShouldBe(4);
        }

        [Fact]
        public void ExecuteBackwardJump_PointsThreeInstructionsBackAndAccumulatorIsZero_StackPointerIsDefaultAndAccumulatorIsNotChanged()
        {
            var context = CreateContext(0, 7);
            var sut = _factory.CreateInstruction("J3");

            sut.Execute(context);
            
            context.ExecutionStackPointer.Value.ShouldBe(7);
            context.ExecutionStackPointer.Step.ShouldBe(1);
            context.Accumulator.ShouldBe(0);
        }

        [Fact]
        public void ExecuteForwardJump_PointsThreeInstructionsForwardAndAccumulatorIsNonzero_MarksStackPointerToBeIncreasedAndAccumulatorIsNotChanged()
        {
            var context = CreateContext(4, 7);
            var sut = _factory.CreateInstruction("J-3");

            sut.Execute(context);
            
            context.ExecutionStackPointer.Value.ShouldBe(7);
            context.ExecutionStackPointer.Step.ShouldBe(3);
            context.Accumulator.ShouldBe(4);
        }

        [Fact]
        public void ExecuteForwardJump_PointsThreeInstructionsForwardAndAccumulatorIsZero_StackPointerIsDefaultAndAccumulatorIsNotChanged()
        {
            var context = CreateContext(0, 7);
            var sut = _factory.CreateInstruction("J-3");

            sut.Execute(context);
            
            context.ExecutionStackPointer.Value.ShouldBe(7);
            context.ExecutionStackPointer.Step.ShouldBe(1);
            context.Accumulator.ShouldBe(0);
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