using ItsAllAboutTheGame.Data.Models;
using System;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface IUserService
    {
        User RegisterUser(string email, string firstName, string lastName, DateTime dateOfBirth);
    }
}
