using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class ToggleAdmin_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
        private User testUser;
        private IdentityRole testRole;
        private string testUserFirstname = "TestUserFirstname";
        private string testUserLastname = "TestUserLastname";
        private string testUserEmail = "TestUserEmail";
        private string testUserUsername = "TestUserUsername";
        private int wallteId = 1;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<IDateTimeProvider> dateTimeProviderMock;

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
            testRole = new IdentityRole()
            {
                Name = GlobalConstants.AdminRole,
                NormalizedName = GlobalConstants.AdminRole.ToUpper()
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
                .UseInMemoryDatabase(databaseName: "ReturnCorrectData_WhenParamatersAreValidAndUserExistsToggleAdmin")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddAsync(testUser);
                await actContext.SaveChangesAsync();
                await actContext.AddAsync(testRole);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                var result = await command.ToggleAdmin(testUser.Id);

                Assert.IsInstanceOfType(result, typeof(UserDTO));
                Assert.IsFalse(result.Deleted);
                Assert.AreEqual(testUser.UserName, result.Username);
                Assert.AreEqual(testUser.Id, result.UserId);
                Assert.IsTrue(result.Admin);
            }
        }

        [TestMethod]
        public async Task UpdateUser_WhenParamatersAreValidAndUserExists()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUser_WhenParamatersAreValidAndUserExistsToggleAdmin")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddAsync(testUser);
                await actContext.SaveChangesAsync();
                await actContext.AddAsync(testRole);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                await command.ToggleAdmin(testUser.Id);
                var adminUser = await assertContext.Users.Where(user => user.Id == testUser.Id).FirstAsync();

                Assert.AreEqual(GlobalConstants.AdminRole.ToString(), adminUser.Role.ToString());
            }
        }

        [TestMethod]
        public async Task ThrowDeleteUserException_WhenUserIsNotFound()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowDeleteUserException_WhenUserIsNotFoundToggleAdmin")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            //Act and Assert
            using (var actAssertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(actAssertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                await Assert.ThrowsExceptionAsync<ToggleAdminException>(async () => await command.ToggleAdmin(testUser.Id));
            }
        }

        [TestMethod]
        public async Task AddToUserRolesTable_WhenParamatersAreValidAndUserExists()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "AddToUserRolesTable_WhenParamatersAreValidAndUserExists")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddAsync(testUser);
                await actContext.SaveChangesAsync();
                await actContext.AddAsync(testRole);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                await command.ToggleAdmin(testUser.Id);
                var updatedUser = await assertContext.Users.Where(user => user.Id == testUser.Id).FirstOrDefaultAsync();
                var assignedRole = await assertContext.Roles.Where(role => role.Id == testRole.Id).FirstOrDefaultAsync();
                var userRole = await assertContext.UserRoles.Where(ur => ur.UserId == updatedUser.Id && ur.RoleId == assignedRole.Id).FirstOrDefaultAsync();

                Assert.IsNotNull(userRole);
            }
        }

        [TestMethod]
        public async Task RemoveFromUserRolesTable_WhenParamatersAreValidAndUserExists()
        {
            //Arrange
            var contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
                .UseInMemoryDatabase(databaseName: "RemoveFromUserRolesTable_WhenParamatersAreValidAndUserExists")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.AddAsync(testUser);
                await actContext.SaveChangesAsync();
                await actContext.AddAsync(testRole);
                await actContext.SaveChangesAsync();
                var userRole = new IdentityUserRole<string>();
                userRole.UserId = testUser.Id;
                userRole.RoleId = testRole.Id;
                await actContext.AddAsync(userRole);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var command = new UserService(assertContext, foreignExchangeServiceMock.Object,
                    walletServiceMock.Object, dateTimeProviderMock.Object);

                await command.ToggleAdmin(testUser.Id);
                var updatedUser = await assertContext.Users.Where(user => user.Id == testUser.Id).FirstOrDefaultAsync();
                var assignedRole = await assertContext.Roles.Where(role => role.Id == testRole.Id).FirstOrDefaultAsync();
                var userRole = await assertContext.UserRoles.Where(ur => ur.UserId == updatedUser.Id && ur.RoleId == assignedRole.Id).FirstOrDefaultAsync();

                Assert.IsNull(userRole);
            }
        }
    }
}
