using ItsAllAboutTheGame.Controllers;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Models.GameViewModels;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Game.Contracts.GameOne;
using ItsAllAboutTheGame.Services.Game.DTO;
using ItsAllAboutTheGame.Services.Game.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.WebTests.GameControllerTests
{
    [TestClass]
    public class GameOneAction_Should
    {
        private Mock<IGame> gameMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<UserManager<User>> mockUserManager;
        private Mock<ITransactionService> transactionServiceMock;
        private Mock<HttpContext> mockContext;
        private User testUser;
        private string testUserFirstname = "TestUserFirstname";
        private string testUserLastname = "TestUserLastname";
        private string testUserEmail = "TestUserEmail";
        private string testUserUsername = "TestUserUsername";
        private int wallteId = 1;

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
            var testWalletDTO = new WalletDTO();
            var gameResultDTO = new GameResultDTO()
            {
                Grid = new GameResults[0, 0],
                WinningRows = new bool[0]
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
            mockContext = new Mock<HttpContext>();

            mockUserManager = new Mock<UserManager<User>>(
                    new Mock<IUserStore<User>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<User>>().Object,
                    new IUserValidator<User>[0],
                    new IPasswordValidator<User>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(testUser);

            gameMock = new Mock<IGame>();
            gameMock
                .Setup(game => game.GenerateGrid(It.IsAny<string>()))
                .Returns(gameResultDTO);

            walletServiceMock = new Mock<IWalletService>();
            walletServiceMock
                .Setup(wallet => wallet.GetUserWallet(It.IsAny<User>()))
                .ReturnsAsync(testWalletDTO);
            transactionServiceMock = new Mock<ITransactionService>();
        }

        [TestMethod]
        public async Task ReturnViewResult_WhenParametersAreCorrect()
        {
            //Act
            var controller = new GameController(walletServiceMock.Object, mockUserManager.Object,
                transactionServiceMock.Object, gameMock.Object);
            
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var result = await controller.GameOne(GlobalConstants.GameOneGrid);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewModel_WhenParametersAreCorrect()
        {
            //Act
            var controller = new GameController(walletServiceMock.Object, mockUserManager.Object,
                transactionServiceMock.Object, gameMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var result = await controller.GameOne(GlobalConstants.GameOneGrid) as ViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(WalletViewModel));
        }

        [TestMethod]
        public async Task CallsCorrectServices_WhenParametersAreCorrect()
        {
            //Act
            var controller = new GameController(walletServiceMock.Object, mockUserManager.Object,
                transactionServiceMock.Object, gameMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var result = await controller.GameOne(GlobalConstants.GameOneGrid) as ViewResult;

            //Assert
            gameMock.Verify(game => game.GenerateGrid(It.IsAny<string>()), Times.Once);
            walletServiceMock.Verify(wallet => wallet.GetUserWallet(It.IsAny<User>()), Times.Once);
            mockUserManager.Verify(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsRedirectToAction_WhenParametersAreNotCorrect()
        {
            //Act
            var controller = new GameController(walletServiceMock.Object, mockUserManager.Object,
                transactionServiceMock.Object, gameMock.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var result = await controller.GameOne("Pesho");

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.IsNull(redirectResult.RouteValues);
        }
    }
}
