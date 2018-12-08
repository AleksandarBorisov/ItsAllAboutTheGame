using ItsAllAboutTheGame.Contracts.GlobalUtilities;
using ItsAllAboutTheGame.Services.Game.Contracts.GameOne;
using ItsAllAboutTheGame.Services.Game.Enums;

namespace ItsAllAboutTheGame.Services.Game.GameOne
{
    public class GameOne : IGameOne
    {
        private IGameRandomizer gameRadomizer;

        public GameOne(IGameRandomizer gameRadomizer)
        {
            this.gameRadomizer = gameRadomizer;
        }

        public decimal Play(int stake)
        {
            var grid = GenerateGrid();

            var wonAmount = EvaluateGrid(grid);

            return wonAmount;
        }

        private decimal EvaluateGrid(string[,] grid)
        {
            decimal wonAmount = 0.0m;

            return wonAmount;
        }

        public string[,] GenerateGrid()
        {
            var grid = new string[4, 3];

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    grid[row, col] = GenerateValue();
                }
            }

            return grid;
        }

        private string GenerateValue()
        {
            var value = gameRadomizer.Next(1, 101);

            if (value <= 5)
            {
                return GameResults.Wildcard.ToString();
            }
            else if (value <= 20)
            {
                return GameResults.Pineapple.ToString();
            }
            else if( value <= 55)
            {
                return GameResults.Banana.ToString();
            }
            else
            {
                return GameResults.Apple.ToString();
            }
        }
    }
}
