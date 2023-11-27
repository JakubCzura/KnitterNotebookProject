using System;

namespace KnitterNotebook.Exceptions;

public class InvalidEnumException(string? message) : Exception(message)
{
}