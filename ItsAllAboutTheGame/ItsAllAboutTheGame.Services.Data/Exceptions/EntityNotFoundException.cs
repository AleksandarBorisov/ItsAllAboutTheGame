using System;

namespace ItsAllAboutTheGame.Services.Data.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message, Exception ex = null) : base(message, ex)
        {

        }
    }
}
