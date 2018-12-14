using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.WalletServiceTests
{
    [TestClass]
    public class WithdrawFromUserBalance_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private User user;


    }
}
