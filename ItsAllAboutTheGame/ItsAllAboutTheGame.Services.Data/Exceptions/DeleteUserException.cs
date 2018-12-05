using System;

namespace ItsAllAboutTheGame.Services.Data.Exceptions
{
    public class DeleteUserException : Exception
    {
        public DeleteUserException(string message, Exception ex = null) : base(message, ex)
        {

        }
    }
}
