namespace Abaddon.Data
{
    public class ExecutionStackPointer
    {
        private const int DefaultStep = 1;
        
        public int Value { get; set; } = 0;
        public int Step { get; set; } = DefaultStep;

        public void ResetStep() => Step = 1;
    }
}