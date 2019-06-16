using Abaddon.Data;

namespace Abaddon.Instructions
{
    public class MoveInstruction<TBoardEntry> : IInstruction<TBoardEntry>
    {
        public void Execute(CurrentState<TBoardEntry> state)
        {
            throw new System.NotImplementedException();
        }
    }
}
