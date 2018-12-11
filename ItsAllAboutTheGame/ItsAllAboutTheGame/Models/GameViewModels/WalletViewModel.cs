using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Game.DTO;
using ItsAllAboutTheGame.Services.Game.Enums;
using ItsAllAboutTheGame.Utilities.CustomAttributes.GameAttributes;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ItsAllAboutTheGame.Models.GameViewModels
{
    public class WalletViewModel
    {
        public WalletViewModel()
        {

        }

        public WalletViewModel(WalletDTO wallet, GameResultDTO gameResult, string gridSize)
        {
            this.Win = gameResult.WonAmount;
            this.Balance = wallet.Balance;
            this.Currency = wallet.Currency;
            this.CurrencySymbol = wallet.CurrencySymbol;
            this.WinningRows = gameResult.WinningRows == null ? new bool[gameResult.Grid.GetLength(0)] : gameResult.WinningRows.ToArray();
            this.Grid = GridToString(gameResult.Grid);
            this.GridSize = gridSize;
        }

        private string[,] GridToString(GameResults[,] grid)
        {
            var result = new string[grid.GetLength(0),grid.GetLength(1)];

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    result[row, col] = grid[row, col].ToString();
                }
            }

            return result;
        }

        [RegularExpression(@"^\d{1,10}(\.\d{1,2})?$", ErrorMessage = "Balance of the wallet must be valid decimal number with 2 digits after the decimal point")]
        public decimal Balance { get; set; }

        public Currency Currency { get; set; }

        public string CurrencySymbol { get; set; }

        //[AssertThat("Stake < Balance", ErrorMessage = "You cannot stake more than you have in your deposit!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid positive Page Number.")]
        [ValidStake("Balance", ErrorMessage = "You cannot stake more than you have in your deposit!")]
        [Required]
        public int? Stake { get; set; }

        public decimal Win { get; set; }

        public bool[] WinningRows { get; set; }

        public string[,] Grid { get; set; }

        public string GridSize { get; set; }
    }
}
