using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using System;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class WalletDTO
    {
        public WalletDTO(Wallet wallet, ForeignExchangeDTO currencies)
        {
            this.Currency = wallet.Currency;
            this.Balance = Math.Round(wallet.Balance * currencies.Rates[wallet.Currency.ToString()], 2);
        }

        public Currency Currency { get; set; }

        public decimal Balance { get; set; }

        public string CurrencySymbol { get; set; }
    }
}
