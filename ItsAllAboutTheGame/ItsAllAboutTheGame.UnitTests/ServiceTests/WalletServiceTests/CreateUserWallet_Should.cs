//using ItsAllAboutTheGame.Data;
//using ItsAllAboutTheGame.Data.Models;
//using ItsAllAboutTheGame.GlobalUtilities.Enums;
//using ItsAllAboutTheGame.Services.Data;
//using ItsAllAboutTheGame.Services.Data.Contracts;
//using ItsAllAboutTheGame.Services.Data.Contracts.ForeignExchangeApiService;
//using ItsAllAboutTheGame.Services.External.Contracts;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace ItsAllAboutTheGame.UnitTests.ServiceTests.WalletServiceTests
//{
//    [TestClass]
//    public class CreateUserWallet_Should
//    {
//        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
//        private Mock<IForeignExchangeService> foreignExchangeServiceMock;

//        [TestMethod]
//        public async Task ReturnWallet_When_PassedCorrectParams()
//        {
//            //Arrange
//            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
//            .UseInMemoryDatabase(databaseName: "ReturnWallet_When_PassedCorrectParams")
//                .Options;

//            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

//            //Act & Assert

//            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                var walletService = new WalletService(actContext, foreignExchangeServiceMock.Object);
//                var walletToCreate = await walletService.CreateUserWallet(Currency.GBP);
//                Assert.IsInstanceOfType(walletToCreate, typeof(Wallet));
//            }
//        }
//    }
//}
