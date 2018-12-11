using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using System;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {

        }

        public UserDTO(User user)
        {
            this.UserId = user.Id;
            this.LockoutFor = GetLockoutDays(user);
            this.Username = user.UserName;
            this.Deleted = user.IsDeleted;
            this.Admin = user.Role.Equals(UserRole.Administrator);
        }

        public string UserId { get; set; }

        public int LockoutFor { get; set; }

        public string Username { get; set; }

        public bool Deleted { get; set; }

        public bool Admin { get; set; }

        private int GetLockoutDays(User user)
        {
            if (user.LockoutEnd == null)
            {
                return 0;
            }

            var lockOutEndDate = user.LockoutEnd.Value.DateTime;

            var currentDate = DateTime.Now;

            var difference = (lockOutEndDate - currentDate).TotalDays;

            return (int)(difference > 0 ? Math.Round(difference, 1) : 0);
        }
    }
}
