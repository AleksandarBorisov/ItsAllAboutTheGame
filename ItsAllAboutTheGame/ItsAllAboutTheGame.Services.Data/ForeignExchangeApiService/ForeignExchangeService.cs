using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.Contracts.ForeignExchangeApiService;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.External.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.ForeignExchangeApiService
{
    public class ForeignExchangeService : IForeignExchangeService
    {
        private readonly IMemoryCache cache;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IJsonDeserializer jsonDeserializer;
        private readonly IForeignExchangeApiCaller foreignExchangeApiCaller;

        public ForeignExchangeService(IJsonDeserializer jsonDeserializer, IForeignExchangeApiCaller foreignExchangeApiCaller
            , IMemoryCache cache, IDateTimeProvider dateTimeProvider)
        {
            this.cache = cache;
            this.jsonDeserializer = jsonDeserializer;
            this.dateTimeProvider = dateTimeProvider;
            this.foreignExchangeApiCaller = foreignExchangeApiCaller;
        }

        public async Task<ForeignExchangeDTO> GetConvertionRates()
        {
            var currencies = await this.cache.GetOrCreateAsync("ConvertionRates", async entry =>
            {
                entry.AbsoluteExpiration = dateTimeProvider.UtcNow.AddDays(1);

                string resourceUrl = $"https://api.exchangeratesapi.io/latest?base={GlobalConstants.BaseCurrency}&symbols={GlobalConstants.Currencies}";

                string currenciesString = await foreignExchangeApiCaller.GetCurrencyData(resourceUrl);

                var convertionRates = jsonDeserializer.Deserialize<ForeignExchangeDTO>(currenciesString);

                return convertionRates;
            });

            return currencies;
        }
    }
}
