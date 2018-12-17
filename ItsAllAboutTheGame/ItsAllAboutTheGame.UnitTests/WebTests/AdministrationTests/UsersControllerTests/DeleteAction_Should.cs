using ItsAllAboutTheGame.Areas.Administration.Controllers;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.WebTests.AdministrationTests.UsersControllerTests
{
    [TestClass]
    public class DeleteAction_Should
    {
        private Mock<IUserService> userServiceMock;
        private string userId = "userId";

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
            userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(user => user.DeleteUser(It.IsAny<string>()))
                .ReturnsAsync(new UserDTO());
        }

        [TestMethod]
        public async Task ReturnViewResult_WhenParametersAreCorrect()
        {
            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.Delete(userId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public async Task ReturnPartialViewResult_WhenParametersAreCorrect()
        {
            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.Delete(userId) as PartialViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(UserDTO));
        }

        [TestMethod]
        public async Task CallsCorrectServices_WhenParametersAreCorrect()
        {
            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.Delete(userId);

            //Assert
            userServiceMock.Verify(user => user.DeleteUser(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsRedirectToAction_WhenUserIsNull()
        {
            //Act
            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.Delete(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.IsNull(redirectResult.RouteValues);
        }
    }
}
