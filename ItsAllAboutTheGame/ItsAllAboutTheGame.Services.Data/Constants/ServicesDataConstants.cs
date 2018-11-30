using ItsAllAboutTheGame.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ItsAllAboutTheGame.Services.Data.Constants
{
    public class ServicesDataConstants
    {
        private static string currencies;

        private static Dictionary<string, string> currencySymbols;

        private const string baseCurrency = "USD";

        public ServicesDataConstants()
        {
            SetCurrencySymbols();
            currencies = string.Join(",", Enum.GetNames(typeof(Currency)));
        }

        public static Dictionary<string, string> CurrencySymbols
        {
            get => currencySymbols;
        }

        public string Currencies {
            get => currencies;
        }

        public string BaseCurrency
        {
            get => baseCurrency;
        }

        private static void SetCurrencySymbols()
        {
            currencySymbols = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.LCID);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null)
                .GroupBy(ri => ri.ISOCurrencySymbol)
                .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);
        }

        public string DepositDescription = "Deposit with card ";
    }
}
