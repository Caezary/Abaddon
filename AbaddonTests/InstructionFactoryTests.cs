using System.Collections.Generic;
using Abaddon;
using Abaddon.Exceptions;
using Abaddon.Execution;
using Abaddon.Instructions;
using Moq;
using Shouldly;
using Xunit;

namespace AbaddonTests
{
    public class InstructionFactoryTests
    {
        private readonly InstructionFactory<int> _sut = new InstructionFactory<int>(
            new Mock<IPerformEntryOperations<int>>().Object,
            new Mock<IComparer<int>>().Object);

        [Fact]
        public void CreateInstructionCalled_UnknownMnemonic_Throws()
        {
            Assert.Throws<UnknownInstructionError>(
                () => _sut.CreateInstruction("ZZZ"));
        }

        [Fact]
        public void CreateInstructionCalled_NullInsteadOfMnemonic_Throws()
        {
            Assert.Throws<UnknownInstructionError>(
                () => _sut.CreateInstruction(null));
        }

        [Fact]
        public void CreateInstructionCalled_MoveLeftMnemonic_CreatesMoveLeftInstruction()
        {
            VerifyInstructionCreationOfType<MoveLeftInstruction<int>>("L");
        }
        
        [Fact]
        public void CreateInstructionCalled_MoveRightMnemonic_CreatesMoveRightInstruction()
        {
            VerifyInstructionCreationOfType<MoveRightInstruction<int>>("R");
        }
        
        [Fact]
        public void CreateInstructionCalled_MoveUpMnemonic_CreatesMoveUpInstruction()
        {
            VerifyInstructionCreationOfType<MoveUpInstruction<int>>("U");
        }
        
        [Fact]
        public void CreateInstructionCalled_MoveDownMnemonic_CreatesMoveDownInstruction()
        {
            VerifyInstructionCreationOfType<MoveDownInstruction<int>>("D");
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
            Assert.Throws<MalformedInstructionError>(() => _sut.CreateInstruction(mnemonic));
        }

        [Theory]
        [InlineData("C5")]
        [InlineData("C123")]
        [InlineData("C-5")]
        public void CreateInstructionCalled_ComparatorMnemonic_CreatesComparatorInstruction(string mnemonic)
        {
            VerifyInstructionCreationOfType<ComparatorInstruction<int>>(mnemonic);
        }

        [Theory]
        [InlineData("C")]
        [InlineData("CZZZ")]
        [InlineData("C0xB")]
        public void CreateInstructionCalled_ComparatorMnemonicWithMalformedCounter_Throws(string mnemonic)
        {
            Assert.Throws<MalformedInstructionError>(() => _sut.CreateInstruction(mnemonic));
        }

        private void VerifyInstructionCreationOfType<TInstructionType>(string mnemonic)
        {
            var result = _sut.CreateInstruction(mnemonic);
            result.ShouldBeOfType<TInstructionType>();
        }
    }
}
