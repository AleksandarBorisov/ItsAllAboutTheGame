using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class GetUserInfo_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
        private User testUser;
        private string testUserFirstname = "TestUserFirstname";
        private string testUserLastname = "TestUserLastname";
        private string testUserEmail = "TestUserEmail";
        private string testUserUsername = "TestUserUsername";
        private int wallteId = 1;
        private Wallet testWallet;
        private int walletBalance = 0;
        private Currency walletCurrency = Currency.USD;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<IDateTimeProvider> dateTimeProviderMock;

        [TestInitialize]
        public void TestInitialize()
        {
            testWallet = new Wallet()
            {
                Currency = walletCurrency,
                Balance = walletBalance
            };
            testUser = new User()
            {
                FirstName = testUserFirstname,
                LastName = testUserLastname,
                Email = testUserEmail,
                UserName = testUserUsername,
                IsDeleted = false,
                WalletId = wallteId,
                Role = UserRole.None
            };
            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();
            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            walletServiceMock = new Mock<IWalletService>();
        }

        [TestMethod]
        public async Task ReturnCorrectData_WhenParamatersAreValidAndUserExists()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCorrectData_WhenParamatersAreValidAndUserExists")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 0m)
            };

            foreignExchangeServiceMock.Setup(exchange => exchange.GetConvertionRates())
                .Returns(Task.FromResult(foreignExchangeDTO));

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddAsync(testWallet);
                await actContext.SaveChangesAsync();
                testUser.Wallet = testWallet;
                await actContext.AddAsync(testUser);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                var result = await command.GetUserInfo(testUser.Id);

                Assert.IsInstanceOfType(result, typeof(UserInfoDTO));
                Assert.AreEqual(testUser.UserName, result.Username);
                Assert.AreEqual(testWallet.Balance, result.Balance);
                Assert.AreEqual(walletCurrency.ToString(), result.Currency);
                Assert.AreEqual(testUser.Id, result.UserId);
                Assert.IsFalse(result.Admin);
            }
        }

        [TestMethod]
        public async Task ThrowEntityNotFoundException_WhenUserIdIsNull()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowEntityNotFoundException_WhenUserIdIsNull")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            //Act and Assert
            using (var actAssertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(actAssertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async() => await command.GetUserInfo(null));
            }
        }

        [TestMethod]
        public async Task ThrowEntityNotFoundException_WhenUserCurrencyIsNotInCurrencyRates()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowEntityNotFoundException_WhenUserCurrencyIsNotInCurrencyRates")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = new Dictionary<string, decimal>()
            };

            foreignExchangeServiceMock.Setup(exchange => exchange.GetConvertionRates())
                .Returns(Task.FromResult(foreignExchangeDTO));

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddAsync(testWallet);
                await actContext.SaveChangesAsync();
                testUser.Wallet = testWallet;
                await actContext.AddAsync(testUser);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => await command.GetUserInfo(testUser.Id));
            }
        }
    }
}
