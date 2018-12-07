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

        public UsersViewModel(IPagedList<UserDTO> usersList)
        {
            this.Users = usersList;
            this.HasNextPage = usersList.HasNextPage;
            this.HasPreviousPage = usersList.HasPreviousPage;
            this.PageCount = usersList.PageCount;
            this.PageNumber = usersList.PageNumber;
            this.PageSize = usersList.PageSize;
            this.TotalItemCount = usersList.TotalItemCount;
        }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int PageCount { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid positive Page Number.")]
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
