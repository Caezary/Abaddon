using System;
using System.Collections.Generic;
using System.Linq;
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

        [Theory]
        [InlineData("ZZZ")]
        [InlineData("D0")]
        [InlineData("U5")]
        [InlineData("S-3")]
        public void CreateInstructionCalled_UnknownMnemonic_Throws(string mnemonic)
        {
            Assert.Throws<UnknownInstructionError>(
                () => _sut.CreateInstruction(mnemonic));
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

        [Fact]
        public void CreateInstructionStackCalled_EmptyInstructionSet_Throws()
        {
            var instructionSet = "";

            Assert.Throws<InstructionsMissingError>(
                () => _sut.CreateInstructionStack(instructionSet));
        }

        [Fact]
        public void CreateInstructionStackCalled_InstructionSetWithOneMnemonic_CreatesStackWithOneInstruction()
        {
            var instructionSet = "Q";

            var result = _sut.CreateInstructionStack(instructionSet);

            result.ShouldHaveSingleItem()
                .ShouldBeOfType<CopyFromAccumulatorInstruction<int>>();
        }

        [Fact]
        public void CreateInstructionStackCalled_InstructionSetWithTwoMnemonics_CreatesStackWithTwoInstructions()
        {
            var instructionSet = "QS";

            var result = _sut.CreateInstructionStack(instructionSet).ToArray();

            result.Length.ShouldBe(2);
            result[0].ShouldBeOfType<CopyFromAccumulatorInstruction<int>>();
            result[1].ShouldBeOfType<SwapInstruction<int>>();
        }

        [Fact]
        public void CreateInstructionStackCalled_InstructionSetWithThreeMnemonics_CreatesStackWithThreeInstructions()
        {
            var instructionSet = "QUS";

            var result = _sut.CreateInstructionStack(instructionSet).ToArray();

            result.Length.ShouldBe(3);
            result[0].ShouldBeOfType<CopyFromAccumulatorInstruction<int>>();
            result[1].ShouldBeOfType<MoveUpInstruction<int>>();
            result[2].ShouldBeOfType<SwapInstruction<int>>();
        }

        [Theory]
        [InlineData("J3", typeof(JumpInstruction<int>))]
        [InlineData("C5", typeof(ComparatorInstruction<int>))]
        public void CreateInstructionStackCalled_InstructionSetWithTwoCharMnemonic_CreatesStackWithOneInstruction(
            string instructionSet, Type expectedType)
        {
            var result = _sut.CreateInstructionStack(instructionSet);

            result.ShouldHaveSingleItem()
                .ShouldBeOfType(expectedType);
        }

        [Theory]
        [InlineData("J35", typeof(JumpInstruction<int>))]
        [InlineData("J-6", typeof(JumpInstruction<int>))]
        [InlineData("C54", typeof(ComparatorInstruction<int>))]
        [InlineData("C-2", typeof(ComparatorInstruction<int>))]
        public void CreateInstructionStackCalled_InstructionSetWithThreeCharMnemonic_CreatesStackWithOneInstruction(
            string instructionSet, Type expectedType)
        {
            var result = _sut.CreateInstructionStack(instructionSet);

            result.ShouldHaveSingleItem()
                .ShouldBeOfType(expectedType);
        }

        [Fact]
        public void CreateInstructionStackCalled_InstructionSetWithManyMnemonics_CreatesStackWithAllDefinedInstructions()
        {
            var instructionSet = "QUSJ2C5LULAC-12345J3J246J-999";

            var result = _sut.CreateInstructionStack(instructionSet).ToArray();

            result.Length.ShouldBe(13);
        }

        private void VerifyInstructionCreationOfType<TInstructionType>(string mnemonic)
        {
            var result = _sut.CreateInstruction(mnemonic);
            result.ShouldBeOfType<TInstructionType>();
        }
    }
}
