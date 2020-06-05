using Abaddon.Data;
using Abaddon.Exceptions;

namespace Abaddon.Instructions
{
    public class JumpInstruction<TBoardEntry> : IInstruction<TBoardEntry>
    {
        private readonly int _instructionCount;

        public JumpInstruction(int instructionCount)
        {
            _instructionCount = instructionCount;
        }

        public void Execute(CurrentState<TBoardEntry> state)
        {
            if (state.ExecutionStackPointer.Value < _instructionCount || _instructionCount == 0)
            {
                throw new InvalidJumpException();
            }
            
            state.ExecutionStackPointer.Direction = StackChangeDirection.Decreasing;
            state.ExecutionStackPointer.Step = _instructionCount;
            // state.Accumulator--; // TODO: decrease
        }
    }
}
