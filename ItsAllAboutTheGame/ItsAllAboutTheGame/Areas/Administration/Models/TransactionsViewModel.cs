using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace ItsAllAboutTheGame.Areas.Administration.Models
{
    public class TransactionsViewModel
    {
        public TransactionsViewModel()
        {

        }

        public TransactionsViewModel(IPagedList<TransactionDTO> transactionsList)
        {
            this.Transactions = transactionsList;
            this.HasNextPage = transactionsList.HasNextPage;
            this.HasPreviousPage = transactionsList.HasPreviousPage;
            this.PageCount = transactionsList.PageCount;
            this.PageNumber = transactionsList.PageNumber;
            this.PageSize = transactionsList.PageSize;
            this.TotalItemCount = transactionsList.TotalItemCount;
        }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int PageCount { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid positive Page Number.")]
        public int PageNumber { get; set; }

        [RegularExpression(@"^[0-9]{2}$", ErrorMessage = "Please enter valid Page Size up to 99.")]
        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

        public IEnumerable<TransactionDTO> Transactions { get; set; }

        public string Username { get; set; }

        public string SearchString { get; set; }

        public string SortOrder { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
