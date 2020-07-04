using Abaddon.Data;
using Abaddon.Exceptions;
using System;
using System.Linq;

namespace Abaddon
{
    public class ContextFactory
    {
        public CurrentState<TEntry> CreateInitialState<TEntry>(
            int width, int height,
            string values,
            Func<char, TEntry> converter,
            MemoryPosition startPosition = null,
            InstructionExecutionCounter executionCounter = null)
        {
            if (width * height != values.Length)
            {
                throw new StateInitializationError($"Values don't match given {nameof(width)} and {nameof(height)}");
            }

            if (startPosition?.Row < 0 || startPosition?.Row >= height)
            {
                throw new StateInitializationError("Faulty start position");
            }

            if (startPosition?.Column < 0 || startPosition?.Column >= width)
            {
                throw new StateInitializationError("Faulty start position");
            }

            var board = CreateBoard(width, values, converter);

            return new CurrentState<TEntry>(board, startPosition, executionCounter);
        }

        public Board<TEntry> CreateBoard<TEntry>(int width, string values, Func<char, TEntry> converter)
        {
            var rows = values.Select((c, index) => new {index, value = converter(c)})
                .GroupBy(x => x.index / width, x => x.value)
                .Select(g => new BoardRow<TEntry>(g.ToList()))
                .ToList();

            var board = new Board<TEntry>(rows);
            return board;
        }
    }
}
