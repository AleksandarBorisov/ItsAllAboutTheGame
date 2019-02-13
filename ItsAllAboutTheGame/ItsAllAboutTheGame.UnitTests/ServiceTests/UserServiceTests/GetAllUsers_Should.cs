using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class GetAllUsers_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
        private User testUserOne;
        private string testUserOneFirstname = "TestUserOneFirstname";
        private string testUserOneLastname = "TestUserOneLastname";
        private string testUserOneEmail = "TestUserOneEmail";
        private string testUserOneUsername = "TestUserOneUsername";
        private int testWalletOneId = 1;
        private User testUserTwo;
        private string testUserTwoFirstname = "TestUserTwoFirstname";
        private string testUserTwoLastname = "TestUserTwoLastname";
        private string testUserTwoEmail = "TestUserTwoEmail";
        private string testUserTwoUsername = "TestUserTwoUsername";
        private int testWalletTwoId = 2;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<IDateTimeProvider> dateTimeProviderMock;

        [TestInitialize]
        public void TestInitialize()
        {
            testUserOne = new User()
            {
                FirstName = testUserOneFirstname,
                LastName = testUserOneLastname,
                Email = testUserOneEmail,
                UserName = testUserOneUsername,
                IsDeleted = false,
                WalletId = testWalletOneId,
                Role = UserRole.None
            };
            testUserTwo = new User()
            {
                FirstName = testUserTwoFirstname,
                LastName = testUserTwoLastname,
                Email = testUserTwoEmail,
                UserName = testUserTwoUsername,
                IsDeleted = false,
                WalletId = testWalletTwoId,
                Role = UserRole.None
            };
            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();
            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            walletServiceMock = new Mock<IWalletService>();
        }

        [TestMethod]
        public async Task ReturnCorrectData_WhenNoParametersArePassed()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCorrectData_WhenNoParametersArePassed")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddAsync(testUserOne);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                var result = await command.GetAllUsers();

                Assert.IsInstanceOfType(result, typeof(IPagedList<UserDTO>));
                Assert.IsTrue(result.Count() == 1);
                Assert.AreEqual(testUserOne.UserName, result.First().Username);
                Assert.IsFalse(result.First().Admin);
                Assert.IsFalse(result.First().Deleted);
            }
        }

        [TestMethod]
        public async Task ReturnCorrectData_WhenSearchStringIsPassed()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCorrectData_WhenSearchStringIsPassed")
                .UseInternalServiceProvider(serviceProvider)
                .Options;
            var listOfUsers = new List<User>() { testUserOne, testUserTwo };
            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddRangeAsync(listOfUsers);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                var result = await command.GetAllUsers(testUserOneUsername);

                Assert.IsInstanceOfType(result, typeof(IPagedList<UserDTO>));
                Assert.IsTrue(result.Count() == 1);
                Assert.AreEqual(testUserOne.UserName, result.First().Username);
                Assert.IsFalse(result.First().Admin);
                Assert.IsFalse(result.First().Deleted);
            }
        }

        [TestMethod]
        public async Task ReturnCorrectData_WhenSortStringIsPassed()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCorrectData_WhenSortStringIsPassed")
                .UseInternalServiceProvider(serviceProvider)
                .Options;
            var listOfUsers = new List<User>() { testUserOne, testUserTwo };

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddRangeAsync(listOfUsers);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                var result = await command.GetAllUsers(null, 1, GlobalConstants.DefultPageSize, GlobalConstants.DefaultUserSorting);

                Assert.IsInstanceOfType(result, typeof(IPagedList<UserDTO>));
                Assert.IsTrue(result.Count() == 2);
                Assert.AreEqual(testUserOne.UserName, result.First().Username);
                Assert.IsFalse(result.First().Admin);
                Assert.IsFalse(result.First().Deleted);
            }
        }

        [TestMethod]
        public async Task ReturnCorrectData_WhenPageSizeIsPassed()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCorrectData_WhenPageSizeIsPassed")
                .UseInternalServiceProvider(serviceProvider)
                .Options;
            var listOfUsers = new List<User>() { testUserOne, testUserTwo };

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddRangeAsync(listOfUsers);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                var result = await command.GetAllUsers(null, 1, 1, GlobalConstants.DefaultUserSorting);

                Assert.IsInstanceOfType(result, typeof(IPagedList<UserDTO>));
                Assert.IsTrue(result.Count() == 1);
                Assert.AreEqual(testUserOne.UserName, result.First().Username);
                Assert.IsFalse(result.First().Admin);
                Assert.IsFalse(result.First().Deleted);
            }
        }

        [TestMethod]
        public async Task ReturnCorrectData_WhenPageNumberIsPassed()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCorrectData_WhenPageNumberIsPassed")
                .UseInternalServiceProvider(serviceProvider)
                .Options;
            var listOfUsers = new List<User>() { testUserOne, testUserTwo };

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddRangeAsync(listOfUsers);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                var result = await command.GetAllUsers(null, 2, 1, GlobalConstants.DefaultUserSorting);

                Assert.IsInstanceOfType(result, typeof(IPagedList<UserDTO>));
                Assert.IsTrue(result.Count() == 1);
                Assert.AreEqual(testUserTwo.UserName, result.First().Username);
                Assert.IsFalse(result.First().Admin);
                Assert.IsFalse(result.First().Deleted);
            }
        }

        [TestMethod]
        public async Task ReturnCorrectData_WhenAllParametersArePassed()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnCorrectData_WhenAllParametersArePassed")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var listOfUsers = new List<User>() { testUserOne, testUserTwo };

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddRangeAsync(listOfUsers);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                var result = await command.GetAllUsers(testUserOneUsername, 1, GlobalConstants.DefultPageSize, GlobalConstants.DefaultUserSorting);

                Assert.IsInstanceOfType(result, typeof(IPagedList<UserDTO>));
                Assert.IsTrue(result.Count() == 1);
                Assert.AreEqual(testUserOne.UserName, result.First().Username);
                Assert.IsFalse(result.First().Admin);
                Assert.IsFalse(result.First().Deleted);
            }
        }
    }
}
