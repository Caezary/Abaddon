﻿using Abaddon;
using Abaddon.Data;
using Abaddon.Exceptions;
using Shouldly;
using Xunit;

namespace AbaddonTests.Instructions
{
    public class MoveInstructionTests
    {
        private const string ExampleInitialStateValues = "3FC21A55B";
        private readonly InstructionFactory _factory;

        public MoveInstructionTests()
        {
            _factory = new InstructionFactory();
        }

        [Fact]
        public void ExecuteMoveRightInstruction_CanMoveRight_ExecutesProperly()
        {
            var context = CreateContext();
            var sut = _factory.CreateInstruction<int>("R");

            sut.Execute(context);

            context.Position.Column.ShouldBe(1);
            context.Position.Row.ShouldBe(0);
            context.MarkedEntry.ShouldBe(0xF);
        }

        [Fact]
        public void ExecuteMoveRightInstruction_CanNotMoveRight_Throws()
        {
            var context = CreateContext(new MemoryPosition(0, 2));
            var sut = _factory.CreateInstruction<int>("R");

            Assert.Throws<IllegalMovementException>(() =>
                sut.Execute(context));
            
            context.Position.Column.ShouldBe(2);
        }

        private static CurrentState<int> CreateContext(MemoryPosition startPosition = null)
        {
            return new ContextFactory().CreateInitialState(
                3, 3, ExampleInitialStateValues, Conversions.AsHex, startPosition);
        }
    }
}
