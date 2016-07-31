using System;

namespace PokemonGo.API.Exceptions
{
    public class AccountNotVerifiedException : Exception
    {
        public AccountNotVerifiedException(string message) : base(message)
        {
            
        }
    }
}