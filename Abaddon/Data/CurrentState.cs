namespace Abaddon.Data
{
    public class CurrentState<TBoardEntry>
    {
        public const int InstructionCountHardLimit = 1000;

        public Board<TBoardEntry> Board { get; }
        public TBoardEntry Accumulator { get; set; }
        public InstructionStack<TBoardEntry> ExecutedInstructions { get; } =
            new InstructionStack<TBoardEntry>(InstructionCountHardLimit);
        public MemoryPosition Position { get; }
        public int InstructionExecutionCount { get; set; } = 0;

        public TBoardEntry MarkedEntry
        {
            get => Board[Position.Row][Position.Column];
            set => Board[Position.Row][Position.Column] = value;
        }

        public CurrentState(Board<TBoardEntry> board, MemoryPosition startPosition = null)
        {
            Board = board;
            Position = startPosition ?? new MemoryPosition(0, 0);
        }
    }
}
