using System.Collections.Generic;
using Abaddon.Data;
using Abaddon.Exceptions;

namespace Abaddon.Verification
{
    public class BoardVerifier<TBoardEntry>
    {
        private readonly Board<Ignorable<TBoardEntry>> _expected;
        private readonly IEqualityComparer<TBoardEntry> _equalityComparer;

        public BoardVerifier(Board<Ignorable<TBoardEntry>> expected, IEqualityComparer<TBoardEntry> equalityComparer)
        {
            _expected = expected;
            _equalityComparer = equalityComparer;
        }

        public bool Verify(Board<TBoardEntry> actual)
        {
            if(_expected.Width != actual.Width || _expected.Height != actual.Height)
            {
                throw new BoardSizesMismatchError();
            }

            for (var row = 0; row < _expected.Height; row++)
            {
                for (var column = 0; column < _expected.Width; column++)
                {
                    if (!IsExpectedEntry(actual, row, column))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsExpectedEntry(Board<TBoardEntry> actual, int row, int column)
        {
            return _expected[row][column].Ignore ||
                   _equalityComparer.Equals(actual[row][column], _expected[row][column].Value);
        }
    }
}