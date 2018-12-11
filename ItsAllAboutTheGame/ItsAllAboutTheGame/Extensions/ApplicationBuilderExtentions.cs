using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
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

        public static void SeedAdmins(this IApplicationBuilder builder, UserManager<User> userManager, ItsAllAboutTheGameDbContext context)
        {
            Task.Run(async () =>
            {
                //Here we create the Admin account
                if (await userManager.FindByEmailAsync(GlobalConstants.AdminEmail) == null)
                {
                    var wallet = new Wallet() { Balance = 0, Currency = Currency.BGN };
                    context.Wallets.Add(wallet);
                    await context.SaveChangesAsync();

                    User user = new User
                    {
                        UserName = GlobalConstants.AdminEmail,
                        FirstName = "Admin",
                        LastName = "Admin",
                        DateOfBirth = DateTime.Parse("1/1/1991"),
                        Email = GlobalConstants.AdminEmail,
                        Wallet = wallet,
                        WalletId = wallet.Id,
                        Role = UserRole.Administrator
                    };

                    var result = await userManager.CreateAsync(user, "Admin123_");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, GlobalConstants.AdminRole);
                    }
                }

                //Here we create the MasterAdmin account
                if (await userManager.FindByEmailAsync(GlobalConstants.MasterAdminEmail) == null)
                {
                    var wallet = new Wallet() { Balance = 0, Currency = Currency.BGN };
                    context.Wallets.Add(wallet);
                    await context.SaveChangesAsync();

                    User user = new User
                    {
                        UserName = GlobalConstants.MasterAdminEmail,
                        FirstName = "MasterAdmin",
                        LastName = "MasterAdmin",
                        DateOfBirth = DateTime.Parse("1/1/1991"),
                        Email = GlobalConstants.MasterAdminEmail,
                        Wallet = wallet,
                        WalletId = wallet.Id,
                        Role = UserRole.MasterAdministrator
                    };
                    

                    var result = await userManager.CreateAsync(user, "Admin123_");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, GlobalConstants.MasterAdminRole);
                    }
                }
            }).Wait();
        }
    }
}
