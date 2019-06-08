using Abaddon.Data;

namespace Abaddon.Instructions
{
    public interface IInstruction<TBoardEntry>
    {
        void Execute(CurrentState<TBoardEntry> state);
    }
}
