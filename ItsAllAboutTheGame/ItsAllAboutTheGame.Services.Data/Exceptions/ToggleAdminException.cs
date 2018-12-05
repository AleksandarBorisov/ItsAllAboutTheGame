using System;

namespace ItsAllAboutTheGame.Services.Data.Exceptions
{
    public class ToggleAdminException : Exception
    {
        public ToggleAdminException(string message, Exception ex = null) : base(message, ex)
        {

        }
    }
}
