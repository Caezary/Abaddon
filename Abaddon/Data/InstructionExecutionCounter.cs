namespace Abaddon.Data
{
    public class InstructionExecutionCounter
    {
        public static readonly InstructionExecutionCounter Default =
            new InstructionExecutionCounter(0, Defaults.InstructionExecutionCountLimit);

        public int Current { get; private set; }
        public int Limit { get; }
        public bool LimitReached => Current == Limit;

        public InstructionExecutionCounter(int current, int limit)
        {
            Current = current;
            Limit = limit;
        }

        public void Increase()
        {
            Current++;
        }
    }
}