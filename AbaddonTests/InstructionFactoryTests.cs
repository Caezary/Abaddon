﻿using Abaddon;
using Abaddon.Exceptions;
using Abaddon.Instructions;
using Shouldly;
using Xunit;

namespace AbaddonTests
{
    public class InstructionFactoryTests
    {
        private readonly InstructionFactory _sut;

        public InstructionFactoryTests()
        {
            _sut = new InstructionFactory();
        }

        [Fact]
        public void CreateInstructionCalled_UnknownMnemonic_Throws()
        {
            Assert.Throws<UnknownInstructionError>(
                () => _sut.CreateInstruction<int>("ZZZ"));
        }

        [Theory]
        [InlineData("U")]
        [InlineData("D")]
        [InlineData("L")]
        public void CreateInstructionCalled_MoveMnemonic_CreatesMoveInstruction(string mnemonic)
        {
            VerifyInstructionCreationOfType<MoveLeftInstruction<int>>(mnemonic);
        }
        
        [Fact]
        public void CreateInstructionCalled_MoveRightMnemonic_CreatesMoveRightInstruction()
        {
            VerifyInstructionCreationOfType<MoveRightInstruction<int>>("R");
        }

        [Fact]
        public void CreateInstructionCalled_CopyFromMnemonic_CreatesCopyFromAccumulatorInstruction()
        {
            VerifyInstructionCreationOfType<CopyFromAccumulatorInstruction<int>>("Q");
        }

        [Fact]
        public void CreateInstructionCalled_CopyToMnemonic_CreatesCopyToAccumulatorInstruction()
        {
            VerifyInstructionCreationOfType<CopyToAccumulatorInstruction<int>>("A");
        }

        [Fact]
        public void CreateInstructionCalled_SwapMnemonic_CreatesSwapInstruction()
        {
            VerifyInstructionCreationOfType<SwapInstruction<int>>("S");
        }

        [Theory]
        [InlineData("J5")]
        [InlineData("J123")]
        [InlineData("J-5")]
        public void CreateInstructionCalled_JumpMnemonic_CreatesJumpInstruction(string mnemonic)
        {
            VerifyInstructionCreationOfType<JumpInstruction<int>>(mnemonic);
        }

        [Theory]
        [InlineData("J")]
        [InlineData("JZZZ")]
        [InlineData("J0xB")]
        public void CreateInstructionCalled_JumpMnemonicWithMalformedCounter_Throws(string mnemonic)
        {
            Assert.Throws<MalformedInstructionError>(() => _sut.CreateInstruction<int>(mnemonic));
        }

        private void VerifyInstructionCreationOfType<TInstructionType>(string mnemonic)
        {
            var result = _sut.CreateInstruction<int>(mnemonic);
            result.ShouldBeOfType<TInstructionType>();
        }
    }
}
