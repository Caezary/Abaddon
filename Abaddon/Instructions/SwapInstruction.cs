using Abaddon.Data;

namespace Abaddon.Instructions
{
    public class SwapInstruction<TBoardEntry> : IInstruction<TBoardEntry>
    {
        public void Execute(CurrentState<TBoardEntry> state)
        {
            var current = state.MarkedEntry;
            state.MarkedEntry = state.Accumulator;
            state.Accumulator = current;
        }
    }
}
