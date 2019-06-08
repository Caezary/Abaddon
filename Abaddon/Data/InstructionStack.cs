using Abaddon.Exceptions;
using Abaddon.Instructions;
using System.Collections.Generic;

namespace Abaddon.Data
{
    public class InstructionStack<TBoardEntry>
    {
        private readonly List<IInstruction<TBoardEntry>> _instructions = new List<IInstruction<TBoardEntry>>();
        private readonly int _instructionLimit;

        public int Count => _instructions.Count;

        public InstructionStack(int instructionLimit)
        {
            _instructionLimit = instructionLimit;
        }

        public void Push(IInstruction<TBoardEntry> instruction)
        {
            if (Count >= _instructionLimit)
            {
                throw new InstructionLimitReachedError();
            }

            _instructions.Add(instruction);
        }

        public IEnumerable<IInstruction<TBoardEntry>> Peek(int count)
        {
            if (count > Count)
            {
                throw new InstructionsMissingError();
            }

            return _instructions.GetRange(Count - count, count);
        }
    }
}
