using System;
using Abaddon.Exceptions;
using Abaddon.Instructions;

namespace Abaddon
{
    public class InstructionFactory
    {
        public IInstruction<TEntity> CreateInstruction<TEntity>(string mnemonic)
        {
            switch (mnemonic)
            {
                case "U":
                    return new MoveUpInstruction<TEntity>();
                case "D":
                    return new MoveDownInstruction<TEntity>();
                case "L":
                    return new MoveLeftInstruction<TEntity>();
                case "R":
                    return new MoveRightInstruction<TEntity>();
                case "A":
                    return new CopyToAccumulatorInstruction<TEntity>();
                case "Q":
                    return new CopyFromAccumulatorInstruction<TEntity>();
                case "S":
                    return new SwapInstruction<TEntity>();
            }

            if (mnemonic.StartsWith("J"))
            {
                return CreateJumpInstruction<TEntity>(mnemonic.Substring(1));
            }

            throw new UnknownInstructionError();
        }

        private IInstruction<TEntity> CreateJumpInstruction<TEntity>(string value)
        {
            if (!int.TryParse(value, out var instructionCount))
            {
                throw new MalformedInstructionError();
            }
            return new JumpInstruction<TEntity>(instructionCount);
        }
    }
}
