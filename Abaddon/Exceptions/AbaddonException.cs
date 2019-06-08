using System;

namespace Abaddon.Exceptions
{
    public class AbaddonException : Exception { }
    public class InstructionLimitReachedError : AbaddonException { }
    public class InstructionsMissingError : AbaddonException { }

    public class StateInitializationError : AbaddonException { }
}
