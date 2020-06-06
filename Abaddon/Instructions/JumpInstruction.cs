using Abaddon.Data;
using Abaddon.Exceptions;
using Abaddon.Execution;

namespace Abaddon.Instructions
{
    public class JumpInstruction<TBoardEntry> : IInstruction<TBoardEntry>
    {
        private readonly int _instructionCount;
        private readonly IPerformEntryOperations<TBoardEntry> _entryOperator;

        public JumpInstruction(
            int instructionCount, IPerformEntryOperations<TBoardEntry> entryOperator)
        {
            _instructionCount = instructionCount;
            _entryOperator = entryOperator;
        }

        public void Execute(CurrentState<TBoardEntry> state)
        {
            if (state.ExecutionStackPointer.Value < _instructionCount || _instructionCount == 0)
            {
                throw new InvalidJumpException();
            }

            if (_entryOperator.IsZero(state.Accumulator))
            {
                return;
            }
            
            state.ExecutionStackPointer.Direction = StackChangeDirection.Decreasing;
            state.ExecutionStackPointer.Step = _instructionCount;
            state.Accumulator = _entryOperator.Decrease(state.Accumulator);
        }
    }
}
