using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Exceptions;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.CardServiceTests
{
    [TestClass]
    public class AddCard_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private User user;
        private string userId = "randomId";
        private string userName = "Koicho";
        private string email = "testmail@gmail";
        private string firstName = "Koichkov";
        private string lastName = "Velichkov";
        private CreditCard creditCard;
        private string cardNumber = "23232141412";
        private string cvv = "3232";
        private IDateTimeProvider dateTimeProvider;

        [TestMethod]
        public async Task ReturnCard_WhenValidParamsPassed()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnsCard_WhenValidParamsPassed")
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

            //Act & Assert

            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(actContext, dateTimeProvider);
                CreditCard creditCardResult = await cardService.AddCard("3242423532532434", "332", DateTime.Parse("02.03.2020"), user);
                Assert.IsInstanceOfType(creditCardResult, typeof(CreditCard));
            }

        }

        [TestMethod]
        public async Task ThrowException_When_CardAlreadyExists()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ThrowException_When_CardAlreadyExists")
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
                UserId = user.Id
            };

            //Act 
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.CreditCards.AddAsync(creditCard);
                await actContext.SaveChangesAsync();
            }

            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(actContext, dateTimeProvider);
                await Assert.ThrowsExceptionAsync<EntityAlreadyExistsException>(async () => await cardService.AddCard("23232141412", "3232", DateTime.Parse("02.03.2020"), user));
            }
        }
    }
}
