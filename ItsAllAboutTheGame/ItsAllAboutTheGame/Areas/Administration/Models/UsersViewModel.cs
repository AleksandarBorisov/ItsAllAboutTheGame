using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace ItsAllAboutTheGame.Areas.Administration.Models
{
    public class UsersViewModel
    {
        public UsersViewModel()
        {

        }

        public UsersViewModel(IPagedList<UserDTO> usersList)
        {
            this.Users = usersList;
            this.HasNextPage = usersList.HasNextPage;
            this.HasPreviousPage = usersList.HasPreviousPage;
            this.PageCount = usersList.PageCount;
            this.PageNumber = usersList.PageNumber;
            this.PageSize = usersList.PageSize;
            this.TotalItemCount = usersList.TotalItemCount;
            this.IsFirstPage = usersList.IsFirstPage;
            this.IsLastPage = usersList.IsLastPage;
            SetDisplayPages();
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

        public bool IsLastPage { get; set; }

        public bool IsFirstPage { get; set; }

        public int FirstDisplayPage { get; set; }

        public int LastDisplayPage { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int PageCount { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid positive Page Number.")]
        public int PageNumber { get; set; }

        [RegularExpression(@"^(0?[1-9]|[1-9][0-9])$", ErrorMessage = "Please enter valid Page Size up to 99.")]
        public int PageSize { get;  set; }

        public int TotalItemCount { get; set; }

        public IEnumerable<UserDTO> Users { get; set; }

        [RegularExpression(@"^[0-9]?[0-9]?[0-9]?$", ErrorMessage = "Please enter valid days up to 999.")]
        public int LockoutFor { get; set; }

        public string Username { get; set; }

        public bool Deleted { get; set; }

        public bool Admin { get; set; }

        public string UserId { get; set; }

        public string  SearchString { get; set; }

        public string SortOrder { get; set; }
    }
}
