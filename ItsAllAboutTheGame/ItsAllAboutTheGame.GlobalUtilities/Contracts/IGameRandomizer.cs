namespace ItsAllAboutTheGame.GlobalUtilities.Contracts
{
    public interface IGameRandomizer
    {
        int Next(int minValue, int maxValue);
    }
}