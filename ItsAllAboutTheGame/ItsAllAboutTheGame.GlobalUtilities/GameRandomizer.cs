using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using System;

namespace ItsAllAboutTheGame.GlobalUtilities
{
    public class GameRandomizer : IGameRandomizer
    {
        private Random randomzer;

        public GameRandomizer()
        {
            //We are seeding a random Hash for maximum randomness
            this.randomzer = new Random(Guid.NewGuid().GetHashCode());
        }

        public int Next(int minValue, int maxValue)
        {
            return this.randomzer.Next(minValue, maxValue);
        }
    }
}
