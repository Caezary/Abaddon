using System.Collections.Generic;
using System.Linq;
using Abaddon;
using Abaddon.Execution;
using Abaddon.Verification;
using Shouldly;
using Xunit;

namespace AbaddonTests.Scenarios
{
    public class SimpleThreeByThreeScenario
    {
        private readonly string[] _expected = new[]
        {
            "1xx",
            "x2x",
            "xx3"
        };
        
        private readonly string[] _startState = new[]
        {
            "6AC",
            "45D",
            "BE4"
        };

        private const string Program = "DSDRRC2J1QC2J1ULQC2J1ULQ";

        private static readonly CharComparer EntryComparer = new CharComparer();
        
        private readonly ContextFactory _contextFactory = new ContextFactory();
        private readonly InstructionFactory<char> _instructionFactory =
            new InstructionFactory<char>(new CharEntryOperator(), EntryComparer);

        [Fact]
        public void AfterProgramExecution_BoardIsAsExpected()
        {
            var startState = string.Join(null, _startState);
            var currentState = _contextFactory.CreateInitialState(3, 3, startState, c => c);
            var instructions = _instructionFactory.CreateInstructionStack(Program);
            var snapshot = new ExecutionSnapshot<char>(instructions);
            var expectedState = string.Join(null, _expected);
            var expectedBoard = _contextFactory.CreateBoard(3, expectedState, EntryConverter);
            var verifier = new BoardVerifier<char>(expectedBoard, EntryComparer);

            while (!snapshot.ExecutionFinished(currentState))
            {
                snapshot.ExecuteStep(currentState);
            }

            var result = verifier.Verify(currentState.Board);
            
            result.ShouldBeTrue();
        }

        private static Ignorable<char> EntryConverter(char c)
        {
            return c == 'x' ? new Ignorable<char>() : new Ignorable<char>(c);
        }
    }
    
    public class CharEntryOperator : IPerformEntryOperations<char>
    {
        public char Decrease(char entry) => --entry;
        public bool IsZero(char entry) => entry == '0';
    }
    
    public class CharComparer : IComparer<char>, IEqualityComparer<char>
    {
        public int Compare(char x, char y) => x.CompareTo(y);
        public bool Equals(char x, char y) => x.Equals(y);
        public int GetHashCode(char obj) => obj.GetHashCode();
    }
}