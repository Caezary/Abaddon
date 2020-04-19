using Abaddon.Data;
using Abaddon.Exceptions;

namespace Abaddon.Instructions
{
    public class MoveInstruction<TBoardEntry> : IInstruction<TBoardEntry>
    {
        public void Execute(CurrentState<TBoardEntry> state)
        {
            if (state.Board.Width <= state.Position.Column + 1)
            {
                throw new IllegalMovementException();
            }
            
            state.Position.Column++;
        }
    }
}
