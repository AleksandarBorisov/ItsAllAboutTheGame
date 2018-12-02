using ItsAllAboutTheGame.Services.Data.DTO;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace ItsAllAboutTheGame.Areas.Administration.Models
{
    public class UsersViewModel
    {
        public UsersViewModel(IPagedList<UserDTO> users)
        {
            this.Users = users.ToList();
            this.Properties = typeof(UserDTO).GetProperties().Select(prop => prop.Name).ToList();
            this.UsersCount = users.Count;
            this.HasNextPage = users.HasNextPage;
            this.IsFirstPage = users.IsFirstPage;
            this.IsLastPage = users.IsLastPage;
            this.PageCount = users.PageCount;
            this.PageNumber = users.PageNumber;
            this.PageSize = users.PageSize;
            this.TotalItemCount = users.TotalItemCount;
        }

        public IEnumerable<string> Properties { get; private set; }

        public int UsersCount { get; private set; }

        public bool HasNextPage { get; private set; }

        public bool IsFirstPage { get; private set; }

        public bool IsLastPage { get; private set; }

        public int PageCount { get; private set; }

        public int PageNumber { get; }

        public int PageSize { get; private set; }

        public int TotalItemCount { get; private set; }

        public IEnumerable<UserDTO> Users { get; private set; }
    }
}
