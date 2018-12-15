using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.WalletServiceTests
{
    [TestClass]
    public class ConvertBalance_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private IDateTimeProvider dateTimeProvider;
        private ForeignExchangeDTO foreignExchangeDTO;
        private User user;
        private Wallet userWallet;

        [TestMethod]
        public async Task ReturnCorrectConvertedBalance_When_PassedValidValue()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnCorrectConvertedBalance_When_PassedValidValue")
                .Options;

            dateTimeProvider = new DateTimeProvider();

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
            };

            userWallet = new Wallet
            {
                Currency = Currency.GBP,
                Balance = 2000, // we put 2000 in balance otherwise the method will return an null DTO (business logic)
                User = user,
            };

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            var currencies = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(userWallet);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var sut = new WalletService(assertContext, foreignExchangeServiceMock.Object, dateTimeProvider);
                var convertedBalance = await sut.ConvertBalance(user);
                var currentWalletBalance = await assertContext.Wallets.Where(w => w.User == user).FirstOrDefaultAsync();
                var expectedAmount = currentWalletBalance.Balance * foreignExchangeDTO.Rates[userWallet.Currency.ToString()];
                Assert.AreEqual(expectedAmount, convertedBalance);
            }
        }
    }
}