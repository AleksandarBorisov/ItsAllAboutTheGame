using System.Collections.Generic;
using X.PagedList;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class TransactionListDTO
    {
        public TransactionListDTO()
        {

        }

        public TransactionListDTO(IPagedList<TransactionDTO> transactions)
        {
            this.Transactions = transactions;
            this.HasNextPage = transactions.HasNextPage;
            this.HasPreviousPage = transactions.HasPreviousPage;
            this.PageCount = transactions.PageCount;
            this.PageNumber = transactions.PageNumber;
            this.PageSize = transactions.PageSize;
            this.IsFirstPage = transactions.IsFirstPage;
            this.IsLastPage = transactions.IsLastPage;
            this.TotalItemCount = transactions.TotalItemCount;
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

        public string Currency { get; set; }

        public string CurrencySymbol { get; set; }
    }
}
