using System;
using System.Collections.Generic;

namespace Abaddon
{
    public class Board<TBoardEntry>
    {
        private readonly List<BoardRow<TBoardEntry>> _rows;

        public BoardRow<TBoardEntry> this[int index] => _rows[index];
        public int Height => _rows.Count;
        public int Width => _rows.Count > 0 ? _rows[0].Width : 0;

        public Board(List<BoardRow<TBoardEntry>> rows) => _rows = rows;
    }

    public class BoardRow<TBoardEntry>
    {
        private readonly List<TBoardEntry> _entries;

        public int Width => _entries.Count;

        public BoardRow(List<TBoardEntry> entries) => _entries = entries;
    }

    public interface IInstruction { }

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

    public class CurrentState
    {
        public int InstructionCount { get; } = 0;       // TODO: IC hard limit (to prevent infinite loops)?
        public MemoryPosition Position { get; } = new MemoryPosition(0, 0);
    }
}
