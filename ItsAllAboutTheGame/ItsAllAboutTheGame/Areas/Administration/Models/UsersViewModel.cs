using ItsAllAboutTheGame.Services.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            this.Users = users.ToList();
            this.UsersCount = users.Count;
            this.HasNextPage = users.HasNextPage;
            this.IsFirstPage = users.IsFirstPage;
            this.IsLastPage = users.IsLastPage;
            this.PageCount = users.PageCount;
            this.PageNumber = users.PageNumber;
            this.PageSize = users.PageSize;
            this.TotalItemCount = users.TotalItemCount;
        }

        public int UsersCount { get; private set; }

        public bool HasNextPage { get; private set; }

        public bool IsFirstPage { get; private set; }

        public bool IsLastPage { get; private set; }

        public int PageCount { get; private set; }

        public int PageNumber { get; }

        public int PageSize { get; private set; }

        public int TotalItemCount { get; private set; }

        public IEnumerable<UserDTO> Users { get; private set; }

        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "Please enter valid days up to 999.")]
        public int LockoutFor { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool Deleted { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string DateOfBirth { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public int RegisteredCards { get; set; }

        public bool Admin { get; set; }

        public string UserId { get; set; }
    }
}
