using System;

namespace Abaddon.Exceptions
{
    public class AbaddonException : Exception
    {
        protected AbaddonException()
        {
        }
        
        protected AbaddonException(string message) : base(message)
        {
        }
    }
    public class InstructionLimitReachedError : AbaddonException { }
    public class InstructionsMissingError : AbaddonException { }
    public class UnknownInstructionError : AbaddonException { }
    public class MalformedInstructionError : AbaddonException { }
    
    public class BoardSizesMismatchError : AbaddonException { }

    public class StateInitializationError : AbaddonException
    {
        public StateInitializationError(string message) : base(message)
        {
        }
    }

    public class InstructionExecutionException : AbaddonException
    {
        protected InstructionExecutionException()
        {
        }

        protected InstructionExecutionException(string message) : base(message)
        {
        }
    }
    
    public class IllegalMovementError : InstructionExecutionException { }
    
    public class IllegalExecutionError : InstructionExecutionException { }
    
    public class ExecutionLimitReachedError : InstructionExecutionException { }
    
    public class InvalidJumpError : InstructionExecutionException { }
    
    public class InvalidComparatorJumpError : InstructionExecutionException { }
}
