using Abaddon;
using Abaddon.Data;
using Abaddon.Exceptions;
using Shouldly;
using Xunit;

namespace AbaddonTests.Instructions
{
    public class JumpInstructionTests
    {
        private const string ExampleInitialStateValues = "E";
        private readonly InstructionFactory _factory = new InstructionFactory();

        [Fact]
        public void ExecuteBackwardJump_ExecutionStackPointerPointsAtStart_Throws()
        {
            var context = CreateContext();
            context.Accumulator = 5;
            var sut = _factory.CreateInstruction<int>("J5");

            Assert.Throws<InvalidJumpException>(
                () => sut.Execute(context));
            
            context.ExecutionStackPointer.Value.ShouldBe(0);
            context.ExecutionStackPointer.Direction.ShouldBe(StackChangeDirection.Increasing);
            context.ExecutionStackPointer.Step.ShouldBe(1);
            context.Accumulator.ShouldBe(5);
        }

        [Fact]
        public void ExecuteBackwardJump_PointsToSelf_Throws()
        {
            var context = CreateContext();
            context.ExecutionStackPointer.Value = 2;
            context.Accumulator = 5;
            var sut = _factory.CreateInstruction<int>("J0");

            Assert.Throws<InvalidJumpException>(
                () => sut.Execute(context));
            
            context.ExecutionStackPointer.Value.ShouldBe(2);
            context.ExecutionStackPointer.Direction.ShouldBe(StackChangeDirection.Increasing);
            context.ExecutionStackPointer.Step.ShouldBe(1);
            context.Accumulator.ShouldBe(5);
        }

        [Fact]
        public void ExecuteBackwardJump_PointsThreeInstructionsBackAndAccumulatorIsNonzero_MarksStackPointerToBeDecreasedAndAccumulatorIsDecreased()
        {
            var context = CreateContext();
            context.ExecutionStackPointer.Value = 7;
            context.Accumulator = 5;
            var sut = _factory.CreateInstruction<int>("J3");

            sut.Execute(context);
            
            context.ExecutionStackPointer.Value.ShouldBe(7);
            context.ExecutionStackPointer.Direction.ShouldBe(StackChangeDirection.Decreasing);
            context.ExecutionStackPointer.Step.ShouldBe(3);
            // context.Accumulator.ShouldBe(4); // TODO: decrease acc, use IPerformEntryOperations - add to InstructionFactory ctor param
        }

        private static CurrentState<int> CreateContext()
        {
            return new ContextFactory().CreateInitialState(
                1, 1, ExampleInitialStateValues, Conversions.AsHex);
        }
    }
}