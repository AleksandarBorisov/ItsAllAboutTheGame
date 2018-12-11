using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Areas.Administration.Models
{
    public class UserTransactionsViewModel
    {
        public UserTransactionsViewModel()
        {

        }

        public UserTransactionsViewModel(TransactionListDTO transactionsList, Dictionary<string, decimal> amounts)
        {
            this.Transactions = transactionsList.Transactions;
            this.HasNextPage = transactionsList.HasNextPage;
            this.HasPreviousPage = transactionsList.HasPreviousPage;
            this.PageCount = transactionsList.PageCount;
            this.PageNumber = transactionsList.PageNumber;
            this.PageSize = transactionsList.PageSize;
            this.TotalItemCount = transactionsList.TotalItemCount;
            this.IsFirstPage = transactionsList.IsFirstPage;
            this.IsLastPage = transactionsList.IsLastPage;
            this.CurrencySymbol = transactionsList.CurrencySymbol;
            SetDisplayPages();
            this.Amounts = amounts;
        }

        private void SetDisplayPages()
        {
            int pages = Math.Min(GlobalConstants.MaxPageCount, this.PageCount);

            FirstDisplayPage = this.PageNumber;
            LastDisplayPage = this.PageNumber;

            while (pages > 1)
            {
                if (this.FirstDisplayPage > 1)
                {
                    FirstDisplayPage--;
                    pages--;
                }
                if (this.LastDisplayPage < PageCount)
                {
                    LastDisplayPage++;
                    pages--;
                }
            }
        }

        public int FirstDisplayPage { get; set; }

        public int LastDisplayPage { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int PageCount { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid positive Page Number.")]
        public int PageNumber { get; set; }

        [RegularExpression(@"^(0?[1-9]|[1-9][0-9])$", ErrorMessage = "Please enter valid Page Size up to 99.")]
        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

        public bool IsFirstPage { get; set; }

        public IEnumerable<TransactionDTO> Transactions { get; set; }

        public string Username { get; set; }

        public string SearchString { get; set; }

        public string SortOrder { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }

        public bool IsLastPage { get; set; }

        public string CurrencySymbol { get; set; }

        public Dictionary<string, decimal> Amounts { get; set; }
    }
}

