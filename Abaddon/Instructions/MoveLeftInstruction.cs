using Abaddon.Data;

namespace Abaddon.Instructions
{
    public class MoveLeftInstruction<TBoardEntry> : MoveInstructionBase<TBoardEntry>
    {
        protected override bool MovementBoundaryReached(CurrentState<TBoardEntry> state) =>
            state.Position.Column == 0;

        protected override void ChangePosition(CurrentState<TBoardEntry> state) =>
            state.Position.Column--;
    }
}