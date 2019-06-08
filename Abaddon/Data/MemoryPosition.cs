namespace Abaddon.Data
{
    public class MemoryPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public MemoryPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
