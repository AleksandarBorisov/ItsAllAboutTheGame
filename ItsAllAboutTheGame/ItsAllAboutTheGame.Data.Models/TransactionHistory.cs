using ItsAllAboutTheGame.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ItsAllAboutTheGame.Data.Models
{
    public class TransactionHistory : IAuditable, IDeletable
    {
        public TransactionType Type { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }

        public string Description { get; set; }

        public User User { get; set; }
    }

    public enum TransactionType
    {
        Withdraw,
        Stake,
        Win,
        Deposit
    }
}
