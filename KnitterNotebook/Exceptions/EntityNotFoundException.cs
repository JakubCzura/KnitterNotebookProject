using System;

namespace KnitterNotebook.Exceptions;

public class EntityNotFoundException(string message) : Exception(message)
{
}