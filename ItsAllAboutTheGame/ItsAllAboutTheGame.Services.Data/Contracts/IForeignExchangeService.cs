using System.Threading.Tasks;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Services.Data.DTO;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface IForeignExchangeService
    {
        Task<ForeignExchangeDTO> GetConvertionRates();

        //Task<decimal> AJAXBalance(User user);
    }
}