using System;

namespace KnitterNotebook.Exceptions;

public class TokenExpirationDateException(string message) : Exception(message)
{
}