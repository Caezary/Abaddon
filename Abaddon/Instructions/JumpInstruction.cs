using Abaddon.Data;

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
            throw new System.NotImplementedException();
        }
    }
}
