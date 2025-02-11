﻿using Abaddon.Data;

namespace Abaddon.Instructions
{
    public class CopyFromAccumulatorInstruction<TBoardEntry> : IInstruction<TBoardEntry>
    {
        public void Execute(CurrentState<TBoardEntry> state) =>
            state.MarkedEntry = state.Accumulator;
    }
}
