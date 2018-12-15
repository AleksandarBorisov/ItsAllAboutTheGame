using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Exceptions;
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
    public class GetUserWallet_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private User user;
        private Wallet userWallet;
        private ForeignExchangeDTO foreignExchangeDTO;
        private IDateTimeProvider dateTimeProvider;

        [TestMethod]
        public async Task ReturnWalletDTO_When_PassedValidParams()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnWalletDTO_When_PassedValidParams")
                .Options;

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

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

            var amountsDictionary = Enum.GetNames(typeof(TransactionType)).ToDictionary(name => name, value => 0m);

            userWallet = new Wallet
            {
                Currency = Currency.GBP,
                Balance = 0,
                User = user,
            };

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 0m)
            };


            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(userWallet);
                await actContext.SaveChangesAsync();
            }

            var getCurrencySymbol = CultureReferences.CurrencySymbols.TryGetValue(userWallet.Currency.ToString(), out string currencySymbol);
            var currencies = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);


            //Act & Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var walletService = new WalletService(assertContext, foreignExchangeServiceMock.Object, dateTimeProvider);
                var walletDTO = await walletService.GetUserWallet(user);

                Assert.IsInstanceOfType(walletDTO, typeof(WalletDTO));
            }
        }

        [TestMethod]
        public async Task ThrowException_WhenAParameter_IsNotPassed()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ThrowException_WhenAParameter_IsNotPassed")
                .Options;

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

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

            var amountsDictionary = Enum.GetNames(typeof(TransactionType)).ToDictionary(name => name, value => 0m);

            userWallet = new Wallet
            {
                Currency = Currency.GBP,
                Balance = 0,
                User = user,
            };

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = null // here there is no passed parameter
            };

            //Act 
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(userWallet);
                await actContext.SaveChangesAsync();
            }

            var getCurrencySymbol = CultureReferences.CurrencySymbols.TryGetValue(userWallet.Currency.ToString(), out string currencySymbol);
            var currencies = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var walletService = new WalletService(assertContext, foreignExchangeServiceMock.Object, dateTimeProvider);
                await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => await walletService.GetUserWallet(user));
            }
        }

        [TestMethod]
        public async Task ThrowException_WhenUser_IsNull()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ThrowException_WhenUser_IsNull")
                .Options;

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            dateTimeProvider = new DateTimeProvider();

            //Act & Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var walletService = new WalletService(assertContext, foreignExchangeServiceMock.Object, dateTimeProvider);
                await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => await walletService.GetUserWallet(null));
            }
        }
    }
}
