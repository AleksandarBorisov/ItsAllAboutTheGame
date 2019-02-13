using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.WalletServiceTests
{
    [TestClass]
    public class CreateUserWallet_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private IDateTimeProvider dateTimeProvider;

        [TestMethod]
        public async Task ReturnWallet_When_PassedCorrectParams()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnWallet_When_PassedCorrectParams")
                .Options;

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            dateTimeProvider = new DateTimeProvider();

            //Act & Assert

            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var walletService = new WalletService(actContext, foreignExchangeServiceMock.Object, dateTimeProvider);
                var walletToCreate = await walletService.CreateUserWallet(Currency.GBP);
                Assert.IsInstanceOfType(walletToCreate, typeof(Wallet));
            }
        }

        //Add test to check if the wallet was added to the base
    }
}
