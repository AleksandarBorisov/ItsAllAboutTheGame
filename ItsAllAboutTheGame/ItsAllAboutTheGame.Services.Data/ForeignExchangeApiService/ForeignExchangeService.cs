using ItsAllAboutTheGame.Services.Data.Contracts.ForeignExchangeApiService;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.External.Contracts;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.ForeignExchangeApiService
{
    public class ForeignExchangeService
    {
        private IJsonDeserializer jsonDeserializer;
        private IForeignExchangeApiCaller foreignExchangeApiCaller;

        public ForeignExchangeService(IJsonDeserializer jsonDeserializer, IForeignExchangeApiCaller foreignExchangeApiCaller)
        {
            this.jsonDeserializer = jsonDeserializer;
            this.foreignExchangeApiCaller = foreignExchangeApiCaller;
        }

        public async Task<ForeignExchangeDTO> GetConvertionRates()
        {
            string resourceUrl = "https://api.exchangeratesapi.io/latest?base=USD&symbols=EUR,USD,BGN,GBP";
            string currenciesString = await foreignExchangeApiCaller.GetCurrencyData(resourceUrl);
            var convertionRates = jsonDeserializer.Deserialize<ForeignExchangeDTO>(currenciesString);

            return convertionRates;
        }
    }
}
