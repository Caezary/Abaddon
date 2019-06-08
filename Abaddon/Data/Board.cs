﻿using System.Collections.Generic;

namespace Abaddon.Data
{
    public class Board<TBoardEntry>     // TODO: maybe Board<TBoardRow> to make it more flexible?
    {
        private readonly List<BoardRow<TBoardEntry>> _rows;

        public BoardRow<TBoardEntry> this[int index] => _rows[index];
        public int Height => _rows.Count;
        public int Width => _rows.Count > 0 ? _rows[0].Width : 0;

        public Board(List<BoardRow<TBoardEntry>> rows) => _rows = rows;
    }
}
