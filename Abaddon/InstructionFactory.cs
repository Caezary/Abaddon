using Abaddon.Exceptions;
using Abaddon.Execution;
using Abaddon.Instructions;

namespace Abaddon
{
    public class InstructionFactory<TEntry>
    {
        private readonly IPerformEntryOperations<TEntry> _entryOperator;

        public InstructionFactory(IPerformEntryOperations<TEntry> entryOperator)
        {
            _entryOperator = entryOperator;
        }
        
        public IInstruction<TEntry> CreateInstruction(string mnemonic) =>
            mnemonic switch
            {
                "U" => new MoveUpInstruction<TEntry>(),
                "D" => new MoveDownInstruction<TEntry>(),
                "L" => new MoveLeftInstruction<TEntry>(),
                "R" => new MoveRightInstruction<TEntry>(),
                "A" => new CopyToAccumulatorInstruction<TEntry>(),
                "Q" => new CopyFromAccumulatorInstruction<TEntry>(),
                "S" => new SwapInstruction<TEntry>(),
                var jump when jump != null && jump.StartsWith("J") =>
                    CreateJumpInstruction(jump.Substring(1)),
                _ => throw new UnknownInstructionError()
            };

        private IInstruction<TEntry> CreateJumpInstruction(string value)
        {
            if (!int.TryParse(value, out var instructionCount))
            {
                throw new MalformedInstructionError();
            }
            return new JumpInstruction<TEntry>(instructionCount, _entryOperator);
        }
    }
}
