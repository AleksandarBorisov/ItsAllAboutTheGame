using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.TransactionServiceTests
{
    [TestClass]
    public class GetAllTransactions_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ICardService> cardServiceMock;
        private Mock<IDateTimeProvider> dateTimeProviderMock;


    }
}
