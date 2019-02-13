using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using System;

namespace ItsAllAboutTheGame.GlobalUtilities
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeProvider()
        {

        }

        public DateTime Now
        {
            get => DateTime.Now;
        }

        public DateTime UtcNow
        {
            get => DateTime.UtcNow;
        }
    }
}
