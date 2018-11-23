using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<User> RegisterUser(string email, string firstName, string lastName,  DateTime dateOfBirth, Currency userCurrency);

        Task<User> RegisterUserWithLoginProvider(ExternalLoginInfo info, Currency userCurrency, DateTime dateOfBirth);
    }
}
