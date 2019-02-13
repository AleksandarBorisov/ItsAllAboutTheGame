using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ItsAllAboutTheGame.GlobalUtilities
{
    public class CultureReferences
    {
        private static Dictionary<string, string> currencySymbols;

        static CultureReferences()
        {
            SetCurrencySymbols();
        }

        public static Dictionary<string, string> CurrencySymbols
        {
            get => currencySymbols;
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
    }
}
