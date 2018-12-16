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

namespace ItsAllAboutTheGame.UnitTests.WebTests.AdministrationTests.TransactionsControllerTests
{
    [TestClass]
    public class IndexAction_Should
    {
        private Mock<ITransactionService> transactionServiceMock;
        private Transaction testTransactionOne;
        private Transaction testTransactionTwo;
        private string testDescription = "TestDescription";
        private int testAmount = 10;
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
            var transactionOneDTO = new TransactionDTO(testTransactionOne);
            var transactionTwoDTO = new TransactionDTO(testTransactionTwo);
            var listOfTransactions = new List<TransactionDTO>() { transactionOneDTO, transactionTwoDTO };

            transactionServiceMock = new Mock<ITransactionService>();
            transactionServiceMock
                .Setup(transaction => transaction.GetAllTransactions(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(listOfTransactions.ToPagedList());

            transactionServiceMock
                .Setup(transaction => transaction.GetAllAmounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<string, decimal>());
        }

        [TestMethod]
        public async Task ReturnViewResult_WhenParametersAreCorrect()
        {
            //Act
            var controller = new TransactionsController(transactionServiceMock.Object);

            var result = await controller.Index();

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task ReturnsCorrectViewModel_WhenParametersAreCorrect()
        {
            //Act
            var controller = new TransactionsController(transactionServiceMock.Object);

            var result = await controller.Index() as ViewResult;

            //Assert
            Assert.IsInstanceOfType(result.Model, typeof(TransactionsViewModel));
        }

        [TestMethod]
        public async Task CallsCorrectServices_WhenParametersAreCorrect()
        {
            //Act
            var controller = new TransactionsController(transactionServiceMock.Object);

            var result = await controller.Index() as ViewResult;

            //Assert
            transactionServiceMock.Verify(transaction => transaction.GetAllTransactions(It.IsAny<string>(),It.IsAny<int>(),It.IsAny<int>(),It.IsAny<string>()), Times.Once);
            transactionServiceMock.Verify(transaction => transaction.GetAllAmounts(It.IsAny<string>(),It.IsAny<string>()), Times.Once);
        }
    }
}
