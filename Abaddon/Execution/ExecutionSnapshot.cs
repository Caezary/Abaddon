using System.Collections.Generic;
using System.Linq;
using Abaddon.Data;
using Abaddon.Exceptions;
using Abaddon.Instructions;

namespace Abaddon.Execution
{
    public class ExecutionSnapshot<TBoardEntry>
    {
        private readonly List<IInstruction<TBoardEntry>> _instructions;

        public ExecutionSnapshot(IEnumerable<IInstruction<TBoardEntry>> instructions)
        {
            _instructions = instructions.ToList();
        }

        public void ExecuteStep(CurrentState<TBoardEntry> context)
        {
            if (context.ExecutionCounter.LimitReached)
            {
                throw new ExecutionLimitReachedException();
            }

            if(context.ExecutionStackPointer >= _instructions.Count)
            {
                throw new IllegalExecutionException();
            }
            
            _instructions[context.ExecutionStackPointer].Execute(context);
            context.ExecutionStackPointer++;
            context.ExecutionCounter.Increase();
        }

        public bool ExecutionFinished(CurrentState<TBoardEntry> context)
        {
            return context.ExecutionStackPointer == _instructions.Count
                   || context.ExecutionCounter.LimitReached;
        }
    }
}