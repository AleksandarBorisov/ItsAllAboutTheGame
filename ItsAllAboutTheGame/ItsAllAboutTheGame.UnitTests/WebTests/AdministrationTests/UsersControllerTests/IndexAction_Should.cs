using ItsAllAboutTheGame.Areas.Administration.Controllers;
using ItsAllAboutTheGame.Areas.Administration.Models;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace ItsAllAboutTheGame.UnitTests.WebTests.AdministrationTests.UsersControllerTests
{
    [TestClass]
    public class IndexAction_Should
    {
        private Mock<IUserService> userServiceMock;
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

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
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
            var userOneDTO = new UserDTO(testUserOne);
            var userTwoDTO = new UserDTO(testUserTwo);
            var listOfUsers = new List<UserDTO>() { userOneDTO, userTwoDTO };

            userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(user => user.GetAllUsers(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(listOfUsers.ToPagedList());
        }

        [TestMethod]
        public async Task ReturnViewResult_WhenParametersAreCorrect()
        {
            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.Index();

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewModel_WhenParametersAreCorrect()
        {
            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.Index() as ViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(UsersViewModel));
        }

        [TestMethod]
        public async Task CallsCorrectServices_WhenParametersAreCorrect()
        {
            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.Index();

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
