using ItsAllAboutTheGame.Services.Data.DTO;
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

        public UsersViewModel(IPagedList<UserDTO> users)
        {
            this.Users = users;
            this.UsersCount = users.Count;
            this.HasNextPage = users.HasNextPage;
            this.HasPreviousPage = users.HasPreviousPage;
            this.IsFirstPage = users.IsFirstPage;
            this.IsLastPage = users.IsLastPage;
            this.PageCount = users.PageCount;
            this.PageNumber = users.PageNumber;
            this.PageSize = users.PageSize;
            this.TotalItemCount = users.TotalItemCount;
        }

        public int UsersCount { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public int PageCount { get; set; }

        public int PageNumber { get; set; }

        [RegularExpression(@"^[0-9]{2}$", ErrorMessage = "Please enter valid Page Size up to 99.")]
        public int PageSize { get;  set; }

        public int TotalItemCount { get; set; }

        public IEnumerable<UserDTO> Users { get; set; }

        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "Please enter valid days up to 999.")]
        public int LockoutFor { get; set; }

        public string Username { get; set; }

        public bool Deleted { get; set; }

        public bool Admin { get; set; }

        public string UserId { get; set; }

        public string  SearchString { get; set; }

        public string SortOrder { get; set; }
    }
}
