using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.External.Contracts
{
    public interface IForeignExchangeApiCaller
    {
        Task<string> GetCurrencyData(string resourceUrl);
    }
}
