using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Services.Game.Contracts.GameOne;
using ItsAllAboutTheGame.Services.Game.DTO;
using ItsAllAboutTheGame.Services.Game.Enums;
using System.Linq;

namespace ItsAllAboutTheGame.Services.Core.Game
{
    public class Game : IGame
    {
        private IGameRandomizer gameRadomizer;

        public Game(IGameRandomizer gameRadomizer)
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
            //We create matrix with rows equal to the rows of the grid + 1
            for (int row = 0; row < grid.GetLength(0); row++)
            {//We take the value of the first item by dividing its enum value to 10
                var rowValue = (decimal)grid[row, 0] / 10.0m;
                //Our prime value to which we will compare all other values in the same row
                var rowPrime = grid[row, 0];
                //We always assume at the begining that the row is winning
                var rowWins = true;

                for (int col = 1; col < grid.GetLength(1); col++)
                {//We check if the prime value is Wildcard and Current Iterated Value is not!
                    if (rowPrime == GameResults.Wildcard && grid[row, col] != GameResults.Wildcard)
                    {//The prime value becomes equal to the Current Iterated Value!
                        rowPrime = grid[row, col];
                    }

                    //Then we check if the Current Iterated Value is equal to the prime or to Wildcard
                    if (grid[row, col] == rowPrime || grid[row, col] == GameResults.Wildcard)
                    {//If it is we add its value to row value
                        rowValue += (decimal)grid[row, col] / 10.0m;
                    }
                    else
                    {//If it is not we evaluate the entire row as Not Winning and break the itaretion
                        rowWins = false;
                        break;
                    }
                }
                if (rowWins)
                {//If the row is still winning we set its index to 1 and add its sum of coefficients
                    //to the last extra row which we added at the creation of the result array
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
