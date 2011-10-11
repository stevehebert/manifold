using System;

namespace Wormhole.Configuration
{
    public class MismatchedClosingTypeDeclarationException : Exception 
    {
        public MismatchedClosingTypeDeclarationException() { }

        public MismatchedClosingTypeDeclarationException(string message) : base(message){}

        public MismatchedClosingTypeDeclarationException(string message, Exception inner) : base(message, inner) { }
    }
}
