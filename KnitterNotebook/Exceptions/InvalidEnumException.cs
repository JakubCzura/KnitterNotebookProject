using System;

namespace KnitterNotebook.Exceptions;

public class InvalidEnumException : Exception
{
    public InvalidEnumException(string? message) : base(message)
    {
    }
}