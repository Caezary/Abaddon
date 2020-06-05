namespace Abaddon.Data
{
    public class CurrentState<TBoardEntry>
    {
        public Board<TBoardEntry> Board { get; }
        public MemoryPosition Position { get; }
        public TBoardEntry Accumulator { get; set; }

        public InstructionExecutionCounter ExecutionCounter { get; }


        public InstructionStack<TBoardEntry> ExecutedInstructions { get; } =
            new InstructionStack<TBoardEntry>(Defaults.InstructionExecutionCountLimit);

        public int InstructionExecutionCount { get; set; } = 0;


        public TBoardEntry MarkedEntry
        {
            get => Board[Position.Row][Position.Column];
            set => Board[Position.Row][Position.Column] = value;
        }

        public ExecutionStackPointer ExecutionStackPointer { get; }

        public CurrentState(Board<TBoardEntry> board, MemoryPosition startPosition = null, InstructionExecutionCounter executionCounter = null)
        {
            Board = board;
            Position = startPosition ?? new MemoryPosition(0, 0);
            ExecutionCounter = executionCounter ?? InstructionExecutionCounter.Default;
            ExecutionStackPointer = new ExecutionStackPointer();
        }
    }
}
