using Abaddon.Data;
using Abaddon.Exceptions;

namespace Abaddon.Instructions
{
    public abstract class MoveInstructionBase<TBoardEntry> : IInstruction<TBoardEntry>
    {
        public void Execute(CurrentState<TBoardEntry> state)
        {
            if (MovementBoundaryReached(state))
            {
                throw new IllegalMovementException();
            }

            ChangePosition(state);
        }

        protected abstract bool MovementBoundaryReached(CurrentState<TBoardEntry> state);
        protected abstract void ChangePosition(CurrentState<TBoardEntry> state);
    }
}