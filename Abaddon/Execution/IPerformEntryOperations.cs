namespace Abaddon.Execution
{
    public interface IPerformEntryOperations<TBoardEntry>
    {
        TBoardEntry Decrease(TBoardEntry entry);
        bool IsZero(TBoardEntry entry);
    }
}