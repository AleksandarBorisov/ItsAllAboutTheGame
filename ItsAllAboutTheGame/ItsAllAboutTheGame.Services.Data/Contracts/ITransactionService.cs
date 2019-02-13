using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface ITransactionService
    {
        Task<TransactionDTO> MakeDeposit(User user, int cardId, decimal amount);

        Task<IPagedList<TransactionDTO>> GetAllTransactions(string searchByUsername = null, int page = 1, int size = GlobalConstants.DefultPageSize, string sortOrder = GlobalConstants.DefaultTransactionSorting);

        Task<TransactionDTO> GameTransaction(User user, int amount, string game, string description, TransactionType type);

        Task<Dictionary<string, decimal>> GetAllAmounts(string searchByUsername = null, string currency = GlobalConstants.BaseCurrency);

        Task<TransactionListDTO> GetUserTransactions(string searchByUsername = null, int page = 1, int size = GlobalConstants.DefultPageSize, string sortOrder = GlobalConstants.DefaultTransactionSorting);
    }
}
