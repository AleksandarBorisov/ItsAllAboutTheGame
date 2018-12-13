using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.CardServiceTests
{
    [TestClass]
    public class IsExpired_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;

        [DataTestMethod]
        [DataRow("02.02.2022")]
        [DataRow("03.13.2021")]
        [DataRow("03.10.2019")]
        public void ReturnTrue_IfDateHasNotExpired(string expiryDate)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnTrue_IfDateHasNotExpired")
                .Options;

            //Act & Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(assertContext);
                var hasExpired = cardService.IsExpired(DateTime.Parse(expiryDate));

                Assert.IsTrue(hasExpired);
            }
        }

        [DataTestMethod]
        [DataRow("02.02.2018")]
        [DataRow("12.13.2018")]
        [DataRow("10.01.2")]
        public void ReturnFalse_IfDateHasExpired(string expiryDate)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnTrue_IfDateHasNotExpired")
                .Options;

            //Act & Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(assertContext);
                var hasExpired = cardService.IsExpired(DateTime.Parse(expiryDate));

                Assert.IsFalse(hasExpired);
            }
        }
    }
}
