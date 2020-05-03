using System;
using Abaddon.Exceptions;
using Abaddon.Instructions;

namespace Abaddon
{
    public class InstructionFactory
    {
        public IInstruction<TEntity> CreateInstruction<TEntity>(string mnemonic) =>
            mnemonic switch
            {
                "U" => new MoveUpInstruction<TEntity>(),
                "D" => new MoveDownInstruction<TEntity>(),
                "L" => new MoveLeftInstruction<TEntity>(),
                "R" => new MoveRightInstruction<TEntity>(),
                "A" => new CopyToAccumulatorInstruction<TEntity>(),
                "Q" => new CopyFromAccumulatorInstruction<TEntity>(),
                "S" => new SwapInstruction<TEntity>(),
                var jump when jump != null && jump.StartsWith("J") =>
                    CreateJumpInstruction<TEntity>(jump.Substring(1)),
                _ => throw new UnknownInstructionError()
            };

        private static IInstruction<TEntity> CreateJumpInstruction<TEntity>(string value)
        {
            if (!int.TryParse(value, out var instructionCount))
            {
                throw new MalformedInstructionError();
            }
            return new JumpInstruction<TEntity>(instructionCount);
        }
    }
}
