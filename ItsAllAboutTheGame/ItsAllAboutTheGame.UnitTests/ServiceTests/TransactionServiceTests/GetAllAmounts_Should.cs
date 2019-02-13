using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.TransactionServiceTests
{
    [TestClass]
    public class GetAllAmounts_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ICardService> cardServiceMock;
        private Mock<IDateTimeProvider> dateTimeProviderMock;
        private ForeignExchangeDTO foreignExchangeDTO;
        private User user;
        private string email = "testmail@gmail";

        [DataTestMethod]
        [DataRow("Win", "Mitko12", "Dimitar", "Dimitrov")]
        [DataRow("Stake", "Nikolai34", "Niki", "Nikolaev")]
        [DataRow("Withdraw", "Horeisho56", "Horei", "Shov")]
        [DataRow("Deposit", "Avatar78", "Avatari", "Avatarov")]
        public async Task ReturnDictionary_WithConcreteUserParams(string transactionType, string userName, string firstName, string lastName)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnDictionary_WithConcreteUserParams")
                .Options;

            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();
            walletServiceMock = new Mock<IWalletService>();
            userServiceMock = new Mock<IUserService>();
            cardServiceMock = new Mock<ICardService>();

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

            user = new User
            {
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userName,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            var transaction = new Transaction
            {
                Type = (TransactionType)Enum.Parse(typeof(TransactionType), transactionType),
                Description = GlobalConstants.DepositDescription + "3232".PadLeft(16, '*'),
                User = user,
                Amount = 1000,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Currency = Currency.BGN
            };

            var transactionTwo = new Transaction
            {
                Type = (TransactionType)Enum.Parse(typeof(TransactionType), transactionType),
                Description = GlobalConstants.DepositDescription + "3102".PadLeft(16, '*'),
                User = user,
                Amount = 1495.5332m,
                CreatedOn = dateTimeProviderMock.Object.Now.AddDays(1), // adding 1 day otherwise it will be created at the exact same time as the first transaction
                Currency = Currency.BGN
            };

            int digitsAfterFloatingPoint;

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Transactions.AddAsync(transaction);
                await actContext.Transactions.AddAsync(transactionTwo);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var sut = new TransactionService(assertContext, walletServiceMock.Object, userServiceMock.Object,
                    foreignExchangeServiceMock.Object, cardServiceMock.Object, dateTimeProviderMock.Object);
                var dictionaryResult = await sut.GetAllAmounts();
                digitsAfterFloatingPoint = dictionaryResult[transactionType]
                    .ToString()
                    .Substring(dictionaryResult[transactionType]
                    .ToString()
                    .IndexOf(".") + 1)
                    .Length;

                Assert.IsInstanceOfType(dictionaryResult, typeof(Dictionary<string,decimal>));
                Assert.IsTrue(digitsAfterFloatingPoint == 2);
            }
        }
    }
}
