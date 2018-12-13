using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
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

            //Act & Assert
            
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(actContext);
                CreditCard creditCardResult = await cardService.AddCard("3242423532532434", "332", DateTime.Parse("02.03.2020"), user);
                Assert.IsInstanceOfType(creditCardResult, typeof(CreditCard));
            }

        }
    }
}
