using ItsAllAboutTheGame.Services.Game.DTO;

namespace ItsAllAboutTheGame.Services.Game.Contracts.GameOne
{
    public interface IGame
    {
        GameResultDTO Play(int stake, string gridDimensions);

        GameResultDTO GenerateGrid(string gridDimensions);
    }
}