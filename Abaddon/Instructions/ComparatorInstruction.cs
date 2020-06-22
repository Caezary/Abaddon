using System.Collections.Generic;
using Abaddon.Data;
using Abaddon.Exceptions;

namespace Abaddon.Instructions
{
    public class ComparatorInstruction<TBoardEntry> : IInstruction<TBoardEntry>
    {
        private readonly int _instructionCount;
        private readonly IComparer<TBoardEntry> _entryComparer;

        public ComparatorInstruction(int instructionCount, IComparer<TBoardEntry> entryComparer)
        {
            _instructionCount = instructionCount;
            _entryComparer = entryComparer;
        }

        public void Execute(CurrentState<TBoardEntry> state)
        {
            if (_instructionCount == 0)
            {
                throw new InvalidComparatorJumpError();
            }

            var compareResult = _entryComparer.Compare(state.Accumulator, state.MarkedEntry);
            if (compareResult >= 0)
            {
                return;
            }

            state.ExecutionStackPointer.Step = _instructionCount;
        }
    }
}
