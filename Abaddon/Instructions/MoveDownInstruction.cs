using Abaddon.Data;

namespace Abaddon.Instructions
{
    public class MoveDownInstruction<TBoardEntry> : MoveInstructionBase<TBoardEntry>
    {
        protected override bool MovementBoundaryReached(CurrentState<TBoardEntry> state) =>
            state.Board.Height <= state.Position.Row + 1;

        protected override void ChangePosition(CurrentState<TBoardEntry> state) =>
            state.Position.Row++;
    }
}