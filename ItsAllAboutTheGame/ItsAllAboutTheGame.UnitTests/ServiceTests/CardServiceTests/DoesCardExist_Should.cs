using ItsAllAboutTheGame.Data;
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
        private User user;
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
                Id = "randomId",
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = "Koicho",
                CreatedOn = dateTimeProvider.Now,
                Email = "testmail@gmail",
                FirstName = "Koichokov",
                LastName = "Velichkov",
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            creditCard = new CreditCard
            {
                CardNumber = "23232141412",
                CVV = "3232",
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
                Id = "randomId",
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = "Koicho",
                CreatedOn = dateTimeProvider.Now,
                Email = "testmail@gmail",
                FirstName = "Koichokov",
                LastName = "Velichkov",
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            creditCard = new CreditCard
            {
                CardNumber = "23232141412",
                CVV = "3232",
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
