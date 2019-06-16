using System;

namespace Abaddon.Exceptions
{
    public class AbaddonException : Exception { }
    public class InstructionLimitReachedError : AbaddonException { }
    public class InstructionsMissingError : AbaddonException { }
    public class UnknownInstructionError : AbaddonException { }
    public class MalformedInstructionError : AbaddonException { }

    public class StateInitializationError : AbaddonException { }
}
