using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using System;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class TransactionDTO
    {
        public TransactionDTO()
        {

        }

        public TransactionDTO(Transaction transaction)
        {
            this.Type = transaction.Type;
            this.Username = transaction.User.UserName;
            this.Amount = transaction.Amount;
            this.Description = transaction.Description;
            this.CreatedOn = transaction.CreatedOn;
        }

        public TransactionType Type { get; set; }

        public string Username { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
