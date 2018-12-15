using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.TransactionServiceTests
{
    [TestClass]
    public class MakeDeposit_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private ForeignExchangeDTO foreignExchangeDTO;
        private User user;
        private Wallet userWallet;
        private IDateTimeProvider dateTimeProvider;

        [TestMethod]
        public async Task ReturnTransactionDTO_When_PassedValidParams()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnTransactionDTO_WhenPassedValidParams")
                .Options;

            dateTimeProvider = new DateTimeProvider();
            decimal amount = 1000;

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
                Wallet = userWallet,
            };

            userWallet = new Wallet
            {
                Currency = Currency.GBP,
                Balance = 2000, // we put 2000 in balance otherwise the method will return an null DTO (business logic)
                User = user,
            };

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            var rates = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(userWallet);
                await actContext.SaveChangesAsync();
            }

            ////Assert
            //using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            //{
            //    var sut = new TransactionService();
            //}
        }
    }
}
