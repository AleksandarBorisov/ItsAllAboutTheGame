using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
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
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class LockoutUser_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
        private User testUser;
        private Wallet testWallet;
        private int walletBalance = 0;
        private Currency walletCurrency = Currency.USD;
        private string testUserFirstname = "TestUserFirstname";
        private string testUserLastname = "TestUserLastname";
        private string testUserEmail = "TestUserEmail";
        private string testUserUsername = "TestUserUsername";
        private int wallteId = 1;
        private int lockoutDays = 1;
        private string fakeDate = "01.01.2000";
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

            dateTimeProviderMock
                .Setup(date => date.UtcNow)
                .Returns(DateTime.Parse(fakeDate));

            dateTimeProviderMock
                .Setup(date => date.Now)
                .Returns(DateTime.Parse(fakeDate));

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddAsync(testUser);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                var result = await command.LockoutUser(testUser.Id, lockoutDays);

                Assert.IsInstanceOfType(result, typeof(UserDTO));
                Assert.AreEqual(1, result.LockoutFor);
                Assert.AreEqual(testUser.UserName, result.Username);
                Assert.AreEqual(testUser.Id, result.UserId);
                Assert.IsFalse(result.Admin);
            }
        }

        [TestMethod]
        public async Task UpdateUser_WhenParamatersAreValidAndUserExists()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUser_WhenParamatersAreValidAndUserExists")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            dateTimeProviderMock
                .Setup(date => date.UtcNow)
                .Returns(DateTime.Parse(fakeDate));

            dateTimeProviderMock
                .Setup(date => date.Now)
                .Returns(DateTime.Parse(fakeDate));

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddAsync(testUser);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                await command.LockoutUser(testUser.Id, lockoutDays);
                var lockedUser = await assertContext.Users.Where(user => user.Id == testUser.Id).FirstAsync();

                Assert.IsNotNull(lockedUser.LockoutEnd);
                Assert.AreEqual(DateTime.Parse(fakeDate).AddDays(1).Date,lockedUser.LockoutEnd.Value.Date);
            }
        }

        [TestMethod]
        public async Task ThrowLockoutUserException_WhenUserIsNotFound()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowLockoutUserException_WhenUserDoesntExist")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            //Act and Assert
            using (var actAssertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(actAssertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                await Assert.ThrowsExceptionAsync<LockoutUserException>(async () => await command.LockoutUser(testUser.Id, lockoutDays));
            }
        }
    }
}
