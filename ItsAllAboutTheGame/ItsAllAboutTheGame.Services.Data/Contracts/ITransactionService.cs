using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.DTO;
using System.Threading.Tasks;
using X.PagedList;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface ITransactionService
    {
        Task<Transaction> MakeDeposit(User user, int cardId, decimal amount);

        IPagedList<TransactionDTO> GetAllTransactions(string searchByUsername = null, int page = 1, int size = GlobalConstants.DefultPageSize, string sortOrder = GlobalConstants.DefultTransactionSorting);

        Task<TransactionDTO> MakeStake(User user, int amount, string game);
    }
}
