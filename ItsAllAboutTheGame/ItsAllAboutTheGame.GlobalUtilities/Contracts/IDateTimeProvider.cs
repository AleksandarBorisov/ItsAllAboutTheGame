using System;

namespace ItsAllAboutTheGame.GlobalUtilities.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}