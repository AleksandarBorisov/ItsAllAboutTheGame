using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.CardServiceTests
{
    [TestClass]
    public class AddCard_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private User user;

        [TestMethod]
        public async Task ReturnCard_WhenValidParamsPassed()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnsCard_WhenValidParamsPassed")
                .Options;

            user = new User
            {
                Id = "randomId",
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = "Koicho",
                CreatedOn = DateTime.Now,
                Email = "testmail@gmail",
                FirstName = "Koichokov",
                LastName = "Velichkov",
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            //Act
            CreditCard creditCardResult;
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(actContext);
                creditCardResult = await cardService.AddCard("3242423532532434", "332", DateTime.Parse("02.03.2020"), user);
            }

            //Assert
            Assert.IsInstanceOfType(creditCardResult, typeof(CreditCard));
        }

        [TestMethod]
        public async Task BeAddedToContext_WhenAddToCreditCard_IsCalled()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "BeAddedToContext_WhenAddToCreditCard_IsCalled")
                .Options;

            user = new User
            {
                Id = "randomId",
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = "Koicho",
                CreatedOn = DateTime.Now,
                Email = "testmail@gmail",
                FirstName = "Koichokov",
                LastName = "Velichkov",
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            //Act
            CreditCard creditCardResult;
            bool doesExist;

            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(actContext);
                creditCardResult = await cardService.AddCard("3242423532532434", "332", DateTime.Parse("02.03.2020"), user);

                doesExist = actContext.CreditCards.Any(cn => cn.CardNumber == "3242423532532434");
            }

            //Assert
            Assert.IsNotNull(creditCardResult);
            Assert.IsTrue(doesExist, "CreditCard was not added after AddCard was called");
        }
    }
}
