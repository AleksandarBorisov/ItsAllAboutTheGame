namespace ItsAllAboutTheGame.Contracts.GlobalUtilities
{
    public interface IGameRandomizer
    {
        int Next(int minValue, int maxValue);
    }
}