using ItsAllAboutTheGame.Services.Game.Enums;
using System.Collections.Generic;

namespace ItsAllAboutTheGame.Services.Game.DTO
{
    public class GameResultDTO
    {
        public GameResultDTO()
        {

        }

        public decimal WonAmount { get; set; }

        public IEnumerable<bool> WinningRows { get; set; }

        public GameResults[,] Grid { get; set; }
    }
}
