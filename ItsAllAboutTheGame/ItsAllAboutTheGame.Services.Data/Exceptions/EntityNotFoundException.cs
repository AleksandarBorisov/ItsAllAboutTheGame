using System;
using System.Collections.Generic;
using System.Text;

namespace ItsAllAboutTheGame.Services.Data.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {

        }
    }
}
