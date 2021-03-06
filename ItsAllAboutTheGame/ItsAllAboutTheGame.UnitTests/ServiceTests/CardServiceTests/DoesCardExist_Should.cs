﻿using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.CardServiceTests
{
    [TestClass]
    public class DoesCardExist_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private CreditCard creditCard;
        private string cardNumber = "23232141412";
        private string cvv = "3232";
        private User user;
        private string userId = "randomId";
        private string userName = "Koicho";
        private string email = "testmail@gmail";
        private string firstName = "Koichkov";
        private string lastName = "Velichkov";
        private IDateTimeProvider dateTimeProvider;

        [TestMethod]
        public async Task ReturnFalse_IfCardExists()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnFalse_IfCardExists")
                .Options;

            dateTimeProvider = new DateTimeProvider();

            user = new User
            {
                Id = userId,
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userName,
                CreatedOn = dateTimeProvider.Now,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            creditCard = new CreditCard
            {
                CardNumber = cardNumber,
                CVV = cvv,
                ExpiryDate = DateTime.Parse("02.03.2020"),
                User = user,
                UserId = user.Id,
            };

            //Act 
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.CreditCards.AddAsync(creditCard);
                await actContext.SaveChangesAsync();
            }


            // Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(assertContext, dateTimeProvider);
                var doesCardExist = cardService.DoesCardExist(creditCard.CardNumber);

                Assert.IsFalse(doesCardExist);
            }
        }

        [TestMethod]
        public async Task ReturnTrue_IfCardDoesntExist()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnTrue_IfCardDoesntExist")
                .Options;

            dateTimeProvider = new DateTimeProvider();

            user = new User
            {
                Id = userId,
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userName,
                CreatedOn = dateTimeProvider.Now,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            creditCard = new CreditCard
            {
                CardNumber = cardNumber,
                CVV = cvv,
                ExpiryDate = DateTime.Parse("02.03.2020"),
                User = user,
                UserId = user.Id,
            };

            //Act 
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.CreditCards.AddAsync(creditCard);
                await actContext.SaveChangesAsync();
            }

            // Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(assertContext, dateTimeProvider);
                //We pass in a different cardNumber which is not added  thus it does not exist in the context!
                var doesCardExist = cardService.DoesCardExist("23232112412");

                Assert.IsTrue(doesCardExist);
            }
        }
    }
}
