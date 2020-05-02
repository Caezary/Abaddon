using Abaddon.Data;
using Abaddon.Exceptions;

namespace Abaddon.Instructions
{
    public class MoveRightInstruction<TBoardEntry> : MoveInstructionBase<TBoardEntry>
    {
        protected override bool MovementBoundaryReached(CurrentState<TBoardEntry> state)
        {
            return state.Board.Width <= state.Position.Column + 1;
        }

        protected override void ChangePosition(CurrentState<TBoardEntry> state)
        {
            state.Position.Column++;
        }
    }
}
