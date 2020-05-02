using Abaddon.Data;

namespace Abaddon.Instructions
{
    public class MoveUpInstruction<TBoardEntry> : MoveInstructionBase<TBoardEntry>
    {
        protected override bool MovementBoundaryReached(CurrentState<TBoardEntry> state) =>
            state.Position.Row == 0;

        protected override void ChangePosition(CurrentState<TBoardEntry> state) =>
            state.Position.Row--;
    }
}