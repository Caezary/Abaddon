﻿using Abaddon.Data;

namespace Abaddon.Instructions
{
    public class CopyToAccumulatorInstruction<TBoardEntry> : IInstruction<TBoardEntry>
    {
        public void Execute(CurrentState<TBoardEntry> state)
        {
            throw new System.NotImplementedException();
        }
    }
}
