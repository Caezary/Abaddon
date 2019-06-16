using Abaddon;
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
            Assert.Throws<UnknownInstructionError>(() => _sut.CreateInstruction<int>("ZZZ"));
        }

        [Theory]
        [InlineData("U")]
        [InlineData("D")]
        [InlineData("L")]
        [InlineData("R")]
        public void CreateInstructionCalled_MoveMnemonic_CreatesMoveInstruction(string mnemonic)
        {
            VerifyInstructionCreationOfType<MoveInstruction<int>>(mnemonic);
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

        [Fact]
        public void CreateInstructionCalled_JumpMnemonic_CreatesJumpInstruction()
        {
            VerifyInstructionCreationOfType<JumpInstruction<int>>("J5");
        }

        [Theory]
        [InlineData("J")]
        [InlineData("JZZZ")]
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
