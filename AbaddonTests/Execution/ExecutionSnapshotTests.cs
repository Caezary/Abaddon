using System.Collections.Generic;
using System.Linq;
using Abaddon;
using Abaddon.Data;
using Abaddon.Exceptions;
using Abaddon.Execution;
using Abaddon.Instructions;
using Moq;
using Shouldly;
using Xunit;

namespace AbaddonTests.Execution
{
    public class ExecutionSnapshotTests
    {
        private const string ExampleInitialStateValues = "3";

        [Fact]
        public void ExecuteStep_NothingToExecute_Throws()
        {
            var sut = new ExecutionSnapshot<int>(new IInstruction<int>[0]);
            var context = CreateContext(InstructionExecutionCounter.Default);
            
            Assert.Throws<IllegalExecutionException>(
                () => sut.ExecuteStep(context));
        }

        [Fact]
        public void ExecuteStep_ExecutionLimitReached_Throws()
        {
            var sut = new ExecutionSnapshot<int>(new IInstruction<int>[0]);
            var executionCounter = new InstructionExecutionCounter(5, 5);
            var context = CreateContext(executionCounter);
            
            Assert.Throws<ExecutionLimitReachedException>(
                () => sut.ExecuteStep(context));
        }

        [Fact]
        public void ExecuteStep_OneInstructionToExecute_CallsExecuteAndIncreasesExecutionCounterAndIncreasesExecutionStackPointer()
        {
            var context = CreateContext(InstructionExecutionCounter.Default);
            var instructionMocks = CreateInstructionMocks(1);
            var sut = CreateSut(instructionMocks);

            sut.ExecuteStep(context);
            
            instructionMocks[0].Verify(x => x.Execute(context), Times.Once);
            context.ExecutionCounter.Current.ShouldBe(1);
            context.ExecutionStackPointer.ShouldBe(1);
        }

        [Fact]
        public void ExecuteStep_ManyInstructionsToExecuteAndMiddleOneIsNext_CallsExecuteAndIncreasesExecutionCounterAndIncreasesExecutionStackPointer()
        {
            var context = CreateContext(new InstructionExecutionCounter(3, 5), 3);
            var instructionMocks = CreateInstructionMocks(5);
            var sut = CreateSut(instructionMocks);

            sut.ExecuteStep(context);
            
            instructionMocks[3].Verify(x => x.Execute(context), Times.Once);
            context.ExecutionCounter.Current.ShouldBe(4);
            context.ExecutionStackPointer.ShouldBe(4);
        }

        [Fact]
        public void ExecuteStep_ManyInstructionsToExecuteAndLastAlreadyExecuted_ThrowsAndNoCounterIncremented()
        {
            var context = CreateContext(new InstructionExecutionCounter(3, 5), 3);
            var instructionMocks = CreateInstructionMocks(3);
            var sut = CreateSut(instructionMocks);

            Assert.Throws<IllegalExecutionException>(
                () => sut.ExecuteStep(context));

            instructionMocks.ForEach(m => m.Verify(x => x.Execute(context), Times.Never));
            context.ExecutionCounter.Current.ShouldBe(3);
            context.ExecutionStackPointer.ShouldBe(3);
        }

        [Fact]
        public void ExecuteStep_ManyInstructionsToExecuteAndLimitReached_ThrowsAndNoCounterIncremented()
        {
            var context = CreateContext(new InstructionExecutionCounter(3, 3), 3);
            var instructionMocks = CreateInstructionMocks(5);
            var sut = CreateSut(instructionMocks);

            Assert.Throws<ExecutionLimitReachedException>(
                () => sut.ExecuteStep(context));

            instructionMocks.ForEach(m => m.Verify(x => x.Execute(context), Times.Never));
            context.ExecutionCounter.Current.ShouldBe(3);
            context.ExecutionStackPointer.ShouldBe(3);
        }

        [Fact]
        public void ExecutionFinished_NothingLeftToExecute_ReturnsTrue()
        {
            var context = CreateContext(new InstructionExecutionCounter(3, 5), 3);
            var instructionMocks = CreateInstructionMocks(3);
            var sut = CreateSut(instructionMocks);
            
            var result = sut.ExecutionFinished(context);

            result.ShouldBeTrue();
        }

        [Fact]
        public void ExecutionFinished_ExecutionLimitReached_ReturnsTrue()
        {
            var context = CreateContext(new InstructionExecutionCounter(3, 3), 3);
            var instructionMocks = CreateInstructionMocks(5);
            var sut = CreateSut(instructionMocks);
            
            var result = sut.ExecutionFinished(context);

            result.ShouldBeTrue();
        }

        [Fact]
        public void ExecutionFinished_InstructionsLeftToExecute_ReturnsFalse()
        {
            var context = CreateContext(new InstructionExecutionCounter(3, 5), 3);
            var instructionMocks = CreateInstructionMocks(5);
            var sut = CreateSut(instructionMocks);
            
            var result = sut.ExecutionFinished(context);

            result.ShouldBeFalse();
        }

        private static ExecutionSnapshot<int> CreateSut(IEnumerable<Mock<IInstruction<int>>> instructionMocks)
        {
            var instructions = instructionMocks.Select(m => m.Object);
            return new ExecutionSnapshot<int>(instructions);
        }

        private static List<Mock<IInstruction<int>>> CreateInstructionMocks(int count)
        {
            return Enumerable.Range(0, count)
                .Select(_ => new Mock<IInstruction<int>>())
                .ToList();
        }

        private static CurrentState<int> CreateContext(
            InstructionExecutionCounter executionCounter, int currentExecutionPointer = 0)
        {
            var contextFactory = new ContextFactory();
            var initialState = contextFactory.CreateInitialState(
                1, 1, ExampleInitialStateValues, Conversions.AsHex, executionCounter: executionCounter);
            initialState.ExecutionStackPointer = currentExecutionPointer;
            return initialState;
        }
    }
}