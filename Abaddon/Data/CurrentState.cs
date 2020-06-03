namespace Abaddon.Data
{
    public class CurrentState<TBoardEntry>
    {
        public Board<TBoardEntry> Board { get; }
        public MemoryPosition Position { get; }
        public TBoardEntry Accumulator { get; set; }

        public InstructionExecutionCounter ExecutionCounter { get; }
        public int ExecutionStackPointer { get; set; } = 0;


        public InstructionStack<TBoardEntry> ExecutedInstructions { get; } =
            new InstructionStack<TBoardEntry>(Defaults.InstructionExecutionCountLimit);

        public int InstructionExecutionCount { get; set; } = 0;


        public TBoardEntry MarkedEntry
        {
            get => Board[Position.Row][Position.Column];
            set => Board[Position.Row][Position.Column] = value;
        }

        public CurrentState(Board<TBoardEntry> board, MemoryPosition startPosition = null, InstructionExecutionCounter executionCounter = null)
        {
            Board = board;
            Position = startPosition ?? new MemoryPosition(0, 0);
            ExecutionCounter = executionCounter ?? InstructionExecutionCounter.Default;
        }
    }
    
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
