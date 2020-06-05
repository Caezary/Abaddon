namespace Abaddon.Data
{
    public class ExecutionStackPointer
    {
        public int Value { get; set; } = 0;
        public StackChangeDirection Direction { get; set; } = StackChangeDirection.Increasing;
        public int Step { get; set; } = 1;
    }
}