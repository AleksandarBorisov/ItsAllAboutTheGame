using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.Contracts.ForeignExchangeApiService;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.External.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.ForeignExchangeApiService
{
    public class ForeignExchangeService : IForeignExchangeService
    {
        private IJsonDeserializer jsonDeserializer;
        private readonly IMemoryCache cache;
        private IForeignExchangeApiCaller foreignExchangeApiCaller;
        private readonly UserManager<User> userManager;

        public ForeignExchangeService(IJsonDeserializer jsonDeserializer, IForeignExchangeApiCaller foreignExchangeApiCaller
            , IMemoryCache cache, UserManager<User> userManager)
        {
            this.cache = cache;
            this.jsonDeserializer = jsonDeserializer;
            this.foreignExchangeApiCaller = foreignExchangeApiCaller;
            this.userManager = userManager;
        }

        public async Task<ForeignExchangeDTO> GetConvertionRates()
        {
            var currencies = await this.cache.GetOrCreateAsync("ConvertionRates", async entry =>
            {
                entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
                string resourceUrl = $"https://api.exchangeratesapi.io/latest?base={GlobalConstants.BaseCurrency}&symbols={GlobalConstants.Currencies}";
                string currenciesString = await foreignExchangeApiCaller.GetCurrencyData(resourceUrl);
                var convertionRates = jsonDeserializer.Deserialize<ForeignExchangeDTO>(currenciesString);

                //throw new HttpRequestException();
                return convertionRates;
            });

            return currencies;
        }
    }
}
