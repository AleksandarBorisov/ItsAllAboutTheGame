using ItsAllAboutTheGame.Data.Models;
using System.Collections.Generic;
using X.PagedList;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class UsersListDTO
    {
        public UsersListDTO(IPagedList<UserDTO> users)
        {
            this.UsersCount = users.Count;
            this.Users = users;
            this.HasNextPage = users.HasNextPage;
            this.HasPreviousPage = users.HasPreviousPage;
            this.PageCount = users.PageCount;
            this.PageNumber = users.PageNumber;
            this.PageSize = users.PageSize;
            this.TotalItemCount = users.TotalItemCount;
        }

        public int UsersCount { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int PageCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

        public IEnumerable<UserDTO> Users { get; set; }
    }
}
