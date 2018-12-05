using System;

namespace ItsAllAboutTheGame.Services.Data.Exceptions
{
    public class LockoutUserException : Exception
    {
        public LockoutUserException(string message, Exception ex = null) : base(message, ex)
        {

        }
    }
}
