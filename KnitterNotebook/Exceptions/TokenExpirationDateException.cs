using System;

namespace KnitterNotebook.Exceptions
{
    public class TokenExpirationDateException : Exception
    {
        public TokenExpirationDateException(string message) : base(message)
        {
        }
    }
}