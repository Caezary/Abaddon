using System.Collections.Generic;
using System.Linq;
using Abaddon.Exceptions;
using Abaddon.Execution;
using Abaddon.Instructions;

namespace Abaddon
{
    public class InstructionFactory<TEntry>
    {
        private readonly IPerformEntryOperations<TEntry> _entryOperator;
        private readonly IComparer<TEntry> _entryComparer;

        public InstructionFactory(IPerformEntryOperations<TEntry> entryOperator, IComparer<TEntry> entryComparer)
        {
            _entryOperator = entryOperator;
            _entryComparer = entryComparer;
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
                var compare when compare != null && compare.StartsWith("C") =>
                    CreateComparatorInstruction(compare.Substring(1)),
                _ => throw new UnknownInstructionError()
            };

        public IEnumerable<IInstruction<TEntry>> CreateInstructionStack(string instructionSet)
        {
            if (string.IsNullOrEmpty(instructionSet))
            {
                throw new InstructionsMissingError();
            }

            return ExtractMnemonics(instructionSet)
                .Select(m => CreateInstruction($"{m}"));
        }

        private IInstruction<TEntry> CreateJumpInstruction(string value)
        {
            var instructionCount = ExtractInstructionCount(value);
            return new JumpInstruction<TEntry>(instructionCount, _entryOperator);
        }

        private IInstruction<TEntry> CreateComparatorInstruction(string value)
        {
            var instructionCount = ExtractInstructionCount(value);
            return new ComparatorInstruction<TEntry>(instructionCount, _entryComparer);
        }

        private static int ExtractInstructionCount(string value)
        {
            if (!int.TryParse(value, out var instructionCount))
            {
                throw new MalformedInstructionError();
            }

            return instructionCount;
        }

        private static IEnumerable<string> ExtractMnemonics(string instructionSet)
        {
            var mnemonics = new LinkedList<string>();

            foreach (var m in instructionSet)
            {
                if (BeginsNewMnemonic(m))
                {
                    mnemonics.AddLast($"{m}");
                    continue;
                }

                mnemonics.Last.Value += $"{m}";
            }

            return mnemonics;
        }

        private static bool BeginsNewMnemonic(char m)
        {
            return char.IsLetter(m) && char.IsUpper(m);
        }
    }
}
