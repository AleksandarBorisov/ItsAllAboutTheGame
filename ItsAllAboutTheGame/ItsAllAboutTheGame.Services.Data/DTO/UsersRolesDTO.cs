using ItsAllAboutTheGame.Data.Models;
using System.Collections.Generic;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class UsersRolesDTO
    {
        public Dictionary<string, List<User>> Roles { get; set; }
    }
}
