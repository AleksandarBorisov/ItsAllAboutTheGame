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

namespace ItsAllAboutTheGame.UnitTests.WebTests.AdministrationTests.TransactionsControllerTests
{
    [TestClass]
    public class UpdateTable_Should
    {
        private Mock<ITransactionService> transactionServiceMock;
        private Transaction testTransactionOne;
        private TransactionDTO transactionOneDTO;
        private Transaction testTransactionTwo;
        private TransactionDTO transactionTwoDTO;
        private string testDescription = "TestDescription";
        private int testAmount = 10;
        private User testUser;
        private string testUserFirstname = "TestUserFirstname";
        private string testUserLastname = "TestUserLastname";
        private string testUserEmail = "TestUserEmail";
        private string testUserUsername = "TestUserUsername";
        private int wallteId = 1;
        private List<TransactionDTO> listOfTransactions;
        private Mock<HttpContext> mockContext;

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
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
            testTransactionOne = new Transaction()
            {
                Type = TransactionType.Deposit,
                Description = testDescription,
                User = testUser,
                UserId = testUser.Id,
                Amount = testAmount,
                Currency = Currency.BGN
            };
            testTransactionTwo = new Transaction()
            {
                Type = TransactionType.Deposit,
                Description = testDescription,
                User = testUser,
                UserId = testUser.Id,
                Amount = testAmount,
                Currency = Currency.BGN
            };
            transactionOneDTO = new TransactionDTO(testTransactionOne);
            transactionTwoDTO = new TransactionDTO(testTransactionTwo);
            listOfTransactions = new List<TransactionDTO>() { transactionOneDTO, transactionTwoDTO };

            transactionServiceMock = new Mock<ITransactionService>();
            transactionServiceMock
                .Setup(transaction => transaction.GetAllTransactions(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(listOfTransactions.ToPagedList());

            transactionServiceMock
                .Setup(transaction => transaction.GetAllAmounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<string, decimal>());

            mockContext = new Mock<HttpContext>();
        }

        [TestMethod]
        public async Task ReturnPartialViewResult_WhenParametersAreCorrect()
        {
            //Arrange
            var walletViewModel = new TransactionsViewModel(listOfTransactions.ToPagedList(), new Dictionary<string, decimal>());

            //Act
            var controller = new TransactionsController(transactionServiceMock.Object);

            var result = await controller.UpdateTable(walletViewModel);

            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewModel_WhenParametersAreCorrect()
        {
            //Arrange
            var walletViewModel = new TransactionsViewModel(listOfTransactions.ToPagedList(), new Dictionary<string, decimal>());

            //Act
            var controller = new TransactionsController(transactionServiceMock.Object);

            var result = await controller.UpdateTable(walletViewModel) as PartialViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(TransactionsViewModel));
        }

        [TestMethod]
        public async Task CallsCorrectServices_WhenParametersAreCorrect()
        {
            //Arrange
            var walletViewModel = new TransactionsViewModel(listOfTransactions.ToPagedList(), new Dictionary<string, decimal>());

            //Act
            var controller = new TransactionsController(transactionServiceMock.Object);

            var result = await controller.UpdateTable(walletViewModel) as PartialViewResult;

            //Assert
            transactionServiceMock.Verify(transaction => transaction.GetAllTransactions(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            transactionServiceMock.Verify(transaction => transaction.GetAllAmounts(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnPartialViewResult_WhenModelIsNotValid()
        {
            //Arrange
            var walletViewModel = new TransactionsViewModel(listOfTransactions.ToPagedList(), new Dictionary<string, decimal>());

            //Act
            var controller = new TransactionsController(transactionServiceMock.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };
            controller.TempData = new Mock<ITempDataDictionary>().Object;
            controller.ModelState.AddModelError("error", "error");

            var result = await controller.UpdateTable(walletViewModel);

            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewModel_WhenModelIsNotValid()
        {
            //Arrange
            var walletViewModel = new TransactionsViewModel(listOfTransactions.ToPagedList(), new Dictionary<string, decimal>());

            //Act
            var controller = new TransactionsController(transactionServiceMock.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };
            controller.TempData = new Mock<ITempDataDictionary>().Object;
            controller.ModelState.AddModelError("error", "error");

            var result = await controller.UpdateTable(walletViewModel) as PartialViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(TransactionsViewModel));
        }

        [TestMethod]
        public async Task CallsCorrectServices_WhenModelIsNotValid()
        {
            //Arrange
            var walletViewModel = new TransactionsViewModel(listOfTransactions.ToPagedList(), new Dictionary<string, decimal>());

            //Act
            var controller = new TransactionsController(transactionServiceMock.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };
            controller.TempData = new Mock<ITempDataDictionary>().Object;
            controller.ModelState.AddModelError("error", "error");

            var result = await controller.UpdateTable(walletViewModel);

            //Assert
            transactionServiceMock.Verify(transaction => transaction.GetAllTransactions(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            transactionServiceMock.Verify(transaction => transaction.GetAllAmounts(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
