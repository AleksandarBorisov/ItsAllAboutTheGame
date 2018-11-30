using ItsAllAboutTheGame.Services.Data.Constants;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.Contracts.ForeignExchangeApiService;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.External.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.ForeignExchangeApiService
{
    public class ForeignExchangeService : IForeignExchangeService
    {
        private ServicesDataConstants constants;
        private IJsonDeserializer jsonDeserializer;
        private readonly IMemoryCache cache;
        private IForeignExchangeApiCaller foreignExchangeApiCaller;

        public ForeignExchangeService(IJsonDeserializer jsonDeserializer, IForeignExchangeApiCaller foreignExchangeApiCaller, ServicesDataConstants constants, IMemoryCache cache)
        {
            this.cache = cache;
            this.constants = constants;
            this.jsonDeserializer = jsonDeserializer;
            this.foreignExchangeApiCaller = foreignExchangeApiCaller;
        }

        public async Task<ForeignExchangeDTO> GetConvertionRates()
        {
            var currencies = await this.cache.GetOrCreateAsync("ConvertionRates", async entry =>
            {
                entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
                string resourceUrl = $"https://api.exchangeratesapi.io/latest?base={constants.BaseCurrency}&symbols={constants.Currencies}";
                string currenciesString = await foreignExchangeApiCaller.GetCurrencyData(resourceUrl);
                var convertionRates = jsonDeserializer.Deserialize<ForeignExchangeDTO>(currenciesString);

                return convertionRates;
            });

            return currencies;

        }
    }
}
