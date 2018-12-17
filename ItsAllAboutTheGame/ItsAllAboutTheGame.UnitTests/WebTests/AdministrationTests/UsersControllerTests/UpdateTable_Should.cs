using ItsAllAboutTheGame.Areas.Administration.Controllers;
using ItsAllAboutTheGame.Areas.Administration.Models;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace ItsAllAboutTheGame.UnitTests.WebTests.AdministrationTests.UsersControllerTests
{
    [TestClass]
    public class UpdateTable_Should
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
        private List<UserDTO> listOfUsers;
        private Mock<HttpContext> mockContext;

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
            listOfUsers = new List<UserDTO>() { userOneDTO, userTwoDTO };

            userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(user => user.GetAllUsers(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(listOfUsers.ToPagedList());
            mockContext = new Mock<HttpContext>();
        }

        [TestMethod]
        public async Task ReturnPartialViewResult_WhenParametersAreCorrect()
        {
            //Arrange
            var usersViewModel = new UsersViewModel(listOfUsers.ToPagedList());

            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.UpdateTable(usersViewModel);

            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewModel_WhenParametersAreCorrect()
        {
            //Arrange
            var usersViewModel = new UsersViewModel(listOfUsers.ToPagedList());

            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.UpdateTable(usersViewModel) as PartialViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(UsersViewModel));
        }

        [TestMethod]
        public async Task CallsCorrectServices_WhenParametersAreCorrect()
        {
            //Arrange
            var usersViewModel = new UsersViewModel(listOfUsers.ToPagedList());

            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.UpdateTable(usersViewModel);

            //Assert
            userServiceMock.Verify(user => user.GetAllUsers(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnPartialViewResult_WhenModelIsNotValid()
        {
            //Arrange
            var usersViewModel = new UsersViewModel(listOfUsers.ToPagedList());

            //Act
            var controller = new UsersController(userServiceMock.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };
            controller.TempData = new Mock<ITempDataDictionary>().Object;
            controller.ModelState.AddModelError("error", "error");

            var result = await controller.UpdateTable(usersViewModel);

            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewModel_WhenModelIsNotValid()
        {
            //Arrange
            var usersViewModel = new UsersViewModel(listOfUsers.ToPagedList());

            //Act
            var controller = new UsersController(userServiceMock.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };
            controller.TempData = new Mock<ITempDataDictionary>().Object;
            controller.ModelState.AddModelError("error", "error");

            var result = await controller.UpdateTable(usersViewModel) as PartialViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(UsersViewModel));
        }

        [TestMethod]
        public async Task CallsCorrectServices_WhenModelIsNotValid()
        {
            //Arrange
            var usersViewModel = new UsersViewModel(listOfUsers.ToPagedList());

            //Act
            var controller = new UsersController(userServiceMock.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };
            controller.TempData = new Mock<ITempDataDictionary>().Object;
            controller.ModelState.AddModelError("error", "error");

            var result = await controller.UpdateTable(usersViewModel);

            //Assert
            userServiceMock.Verify(user => user.GetAllUsers(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }
    }
}
