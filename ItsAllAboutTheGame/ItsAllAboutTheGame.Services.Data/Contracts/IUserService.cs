using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<User> RegisterUser(string email, string firstName, string lastName,  DateTime dateOfBirth, Currency userCurrency);

        Task<User> RegisterUserWithLoginProvider(ExternalLoginInfo info, Currency userCurrency, DateTime dateOfBirth);

        Task<UserInfoDTO> GetUserInfo(ClaimsPrincipal userClaims);

        Task<User> GetUser(string username);

        Task<IEnumerable<CreditCard>> UserCards(User user);

        //List<UserDTO> GetAllUsers();
    }
}
