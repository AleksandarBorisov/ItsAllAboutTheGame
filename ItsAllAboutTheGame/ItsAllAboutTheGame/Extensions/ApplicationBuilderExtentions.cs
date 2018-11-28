using ItsAllAboutTheGame.Data.Constants;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Extensions
{
    public static class ApplicationBuilderExtentions
    {
        public static void UseNotFoundExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<EntityNotFoundMiddleware>();
        }

        public static void SeedAdmins(this IApplicationBuilder builder, UserManager<User> userManager)
        {
            Task.Run(async () =>
            {
                //Here we create the Admin account
                if (await userManager.FindByEmailAsync(DataConstants.AdminEmail) == null)
                {
                    User user = new User
                    {
                        UserName = DataConstants.AdminEmail,
                        FirstName = "Admin",
                        LastName = "Admin",
                        DateOfBirth = DateTime.Parse("1/1/1991"),
                        Email = DataConstants.AdminEmail,
                        Wallet = new Wallet() { Balance = 0, Currency = Currency.BGN, }
                    };

                    var result = await userManager.CreateAsync(user, "Admin123_");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, DataConstants.AdminRole);
                    }
                }

                //Here we create the MasterAdmin account
                if (await userManager.FindByEmailAsync(DataConstants.MasterAdminEmail) == null)
                {
                    User user = new User
                    {
                        UserName = DataConstants.MasterAdminEmail,
                        FirstName = "MasterAdmin",
                        LastName = "MasterAdmin",
                        DateOfBirth = DateTime.Parse("1/1/1991"),
                        Email = DataConstants.MasterAdminEmail,
                        Wallet = new Wallet() { Balance = 0, Currency = Currency.BGN, }
                    };

                    var result = await userManager.CreateAsync(user, "Admin123_");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, DataConstants.MasterAdminRole);
                    }
                }
            }).Wait();
        }
    }
}
