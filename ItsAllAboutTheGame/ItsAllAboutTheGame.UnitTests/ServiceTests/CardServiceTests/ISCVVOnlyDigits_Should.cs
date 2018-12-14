using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.CardServiceTests
{
    [TestClass]
    public class ISCVVOnlyDigits_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;

        //[TestMethod]
        //public void ReturnTrue_When_ParsingCVVToNumber_IsSuccessful()
        //{
        //    //Arrange
        //    contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
        //    .UseInMemoryDatabase(databaseName: "ReturnTrue_When_ParsingToIntIsSuccessful")
        //        .Options;

        //    var CVV = "3435";

        //    //Act & Assert
        //    using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
        //    {
        //        var cardService = new CardService(assertContext);
        //        var areOnlyDigits = cardService.IsCVVOnlyDigits(CVV);

        //        Assert.IsTrue(areOnlyDigits);
        //    }
        //}


        //[DataTestMethod]
        //[DataRow("34t4")]
        //[DataRow(null)]
        //public void ReturnFalse_When_ParsingCVVToNumber_Fails(string CVV)
        //{
        //    //Arrange
        //    contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
        //    .UseInMemoryDatabase(databaseName: "ReturnFalse_When_ParsingCVVToNumber_Fails")
        //        .Options;

        //    //Act & Assert
        //    using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
        //    {
        //        var cardService = new CardService(assertContext);
        //        var areOnlyDigits = cardService.IsCVVOnlyDigits(CVV);

        //        Assert.IsFalse(areOnlyDigits);
        //    }
        //}
    }
}
