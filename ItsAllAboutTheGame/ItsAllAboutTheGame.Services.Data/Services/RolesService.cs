//using ItsAllAboutTheGame.Data;
//using ItsAllAboutTheGame.Data.Models;
//using ItsAllAboutTheGame.GlobalUtilities.Constants;
//using ItsAllAboutTheGame.Services.Data.DTO;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ItsAllAboutTheGame.Services.Data.Services
//{

//    public class RolesService
//    {
//        private readonly ItsAllAboutTheGameDbContext context;
//        private readonly RoleManager<User> roleManager;

//        public RolesService(ItsAllAboutTheGameDbContext context, RoleManager<User> roleManager)
//        {
//            this.context = context;
//            this.roleManager = roleManager;
//        }

//        public async Task<UsersRolesDTO> GetUserRoles()
//        {
//            var usersRoles = this.context.Roles
//                .OrderBy(r => r.Name)

//            var roles = this.
//            await roleManager.FindByNameAsync(GlobalConstants.AdminRole).Users
//        }
//    }
//}
