using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using X.PagedList;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class TransactionListDTO
    {
        public TransactionListDTO(IPagedList<Transaction> transactions, string sortOrder)
        {
            this.Amounts = Enum.GetNames(typeof(TransactionType)).ToDictionary(name => name, value => 0m);
            this.Transactions = OrderTransactions(transactions.AsEnumerable(), sortOrder);
            this.HasNextPage = transactions.HasNextPage;
            this.HasPreviousPage = transactions.HasPreviousPage;
            this.PageCount = transactions.PageCount;
            this.PageNumber = transactions.PageNumber;
            this.PageSize = transactions.PageSize;
            this.IsFirstPage = transactions.IsFirstPage;
            this.IsLastPage = transactions.IsLastPage;
            this.TotalItemCount = transactions.TotalItemCount;
        }

        private IEnumerable<TransactionDTO> OrderTransactions(IEnumerable<Transaction> transactions, string sortOrder)
        {

            var property = sortOrder.Remove(sortOrder.IndexOf("_"));
            PropertyInfo prop = typeof(Transaction).GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (!sortOrder.Contains("_desc"))
            {
                return transactions
                    .OrderBy(transaction => prop.GetValue(transaction))
                    .Select(transaction => {
                        Amounts[transaction.Type.ToString()] = transaction.Amount;
                        return new TransactionDTO(transaction);
                        });
            }
            else
            {
                return transactions
                    .OrderByDescending(user => prop.GetValue(user))
                    .Select(transaction => {
                        Amounts[transaction.Type.ToString()] += transaction.Amount;
                        return new TransactionDTO(transaction);
                    });
            }
        }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int PageCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public int TotalItemCount { get; set; }

        public IEnumerable<TransactionDTO> Transactions { get; set; }

        public Dictionary<string,decimal> Amounts { get; set; }
    }
}
