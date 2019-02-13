using System;

namespace ItsAllAboutTheGame.Services.Data.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string message, Exception ex = null) : base(message, ex)
        {

        }
    }
}
