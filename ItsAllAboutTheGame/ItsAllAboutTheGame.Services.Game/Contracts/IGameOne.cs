using ItsAllAboutTheGame.Services.Game.DTO;

namespace ItsAllAboutTheGame.Services.Game.Contracts.GameOne
{
    public interface IGameOne
    {
        GameResultDTO Play(int stake);

        GameResultDTO GenerateGrid();
    }
}