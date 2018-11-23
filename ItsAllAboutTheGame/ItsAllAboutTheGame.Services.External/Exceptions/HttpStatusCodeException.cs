using System;

namespace ItsAllAboutTheGame.Services.External.Exceptions
{
    public class HttpStatusCodeException : Exception
    {
        public HttpStatusCodeException(string message, Exception ex = null) : base(message, ex)
        {

        }
    }
}
