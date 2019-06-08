using Abaddon.Data;
using Abaddon.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abaddon
{
    public class ContextFactory
    {
        public CurrentState<TEntry> CreateInitialState<TEntry>(
            int width, int height, string values, Func<char, TEntry> converter)
        {
            if (width * height != values.Length)
            {
                throw new StateInitializationError();
            }
            var rows = values.Select((c, index) => new { index, value = converter(c) })
                .GroupBy(x => x.index / width, x => x.value)
                .Select(g => new BoardRow<TEntry>(g.ToList()))
                .ToList();

            var board = new Board<TEntry>(rows);

            return new CurrentState<TEntry>(board);
        }
    }
}
