using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Services.Game.Contracts.GameOne;
using ItsAllAboutTheGame.Services.Game.DTO;
using ItsAllAboutTheGame.Services.Game.Enums;
using System.Linq;

namespace ItsAllAboutTheGame.Services.Game.GameOne
{
    public class GameOne : IGameOne
    {
        private IGameRandomizer gameRadomizer;

        public GameOne(IGameRandomizer gameRadomizer)
        {
            this.gameRadomizer = gameRadomizer;
        }

        public GameResultDTO Play(int stake, string gridDimensions)
        {
            var result = GenerateGrid(gridDimensions);

            var evaluatedGrid = EvaluateGrid(result.Grid);

            result.WonAmount = evaluatedGrid[evaluatedGrid.Length - 1] * stake;

            result.WinningRows = evaluatedGrid.Take(evaluatedGrid.Length - 1).Select(r => r == 1);

            return result;
        }

        private decimal[] EvaluateGrid(GameResults[,] grid)
        {
            decimal[] results = new decimal[grid.GetLength(0) + 1];

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                var rowValue = (decimal)grid[row, 0] / 10.0m;//We take the first value

                var rowPrime = grid[row, 0];

                var rowWins = true;

                for (int col = 1; col < grid.GetLength(1); col++)
                {
                    if (rowPrime == GameResults.Wildcard && grid[row, col] != GameResults.Wildcard)
                    {
                        rowPrime = grid[row, col];
                    }
                    if (grid[row, col] == rowPrime || grid[row, col] == GameResults.Wildcard)
                    {//We evaluate the current value if it is equal to the previous or to the WildCard
                        rowValue += (decimal)grid[row, col] / 10.0m;
                    }
                    else
                    {
                        rowWins = false;
                        break;
                    }
                }
                if (rowWins)
                {
                    results[results.Length - 1] += rowValue;
                    results[row] = 1;
                }
            }
            return results;
        }

        public GameResultDTO GenerateGrid(string gridDimensions)
        {
            string[] dimensions = gridDimensions.Split('x');

            int rows = int.Parse(dimensions[0]);

            int cows = int.Parse(dimensions[1]);

            var grid = new GameResults[rows, cows];

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    grid[row, col] = GenerateValue();
                }
            }

            var result = new GameResultDTO();

            result.Grid = grid;

            return result;
        }

        private GameResults GenerateValue()
        {
            var value = gameRadomizer.Next(1, 101);

            if (value <= 5)
            {
                return GameResults.Wildcard;
            }
            else if (value <= 20)
            {
                return GameResults.Pineapple;
            }
            else if (value <= 55)
            {
                return GameResults.Banana;
            }
            else
            {
                return GameResults.Apple;
            }
        }
    }
}
