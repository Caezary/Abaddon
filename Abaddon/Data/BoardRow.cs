using System.Collections.Generic;

namespace Abaddon.Data
{
    public class BoardRow<TBoardEntry>
    {
        private readonly List<TBoardEntry> _entries;

        public TBoardEntry this[int index]
        {
            get => _entries[index];
            set => _entries[index] = value;
        }

        public int Width => _entries.Count;

        public BoardRow(List<TBoardEntry> entries) => _entries = entries;
    }
}
