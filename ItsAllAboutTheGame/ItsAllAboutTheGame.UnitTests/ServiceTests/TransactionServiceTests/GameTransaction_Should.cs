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
    public class GameTransaction_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ICardService> cardServiceMock;
        private Mock<IDateTimeProvider> dateTimeProviderMock;
        private ForeignExchangeDTO foreignExchangeDTO;

        [DataTestMethod]
        [DataRow("FirstUser","500", "4x3", "Stake on game 4x3", "Stake")]    
        [DataRow("SecondUser","2500", "8x5", "Win on game 8x5", "Win")]
        public async Task ReturnTransactionDTO_OfTypeStakeAndWin_WhenPassedValidParams(string userName, string amount, string game, string description, string type)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnTransactionDTO_OfTypeStakeAndWin_WhenPassedValidParams")
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

            var user = new User
            {
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userName,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Email = "edward@gmail.com",
                FirstName = "Edward",
                LastName = "Evlogiev",
                DateOfBirth = DateTime.Parse("12.03.1993"),
                Role = UserRole.None,
            };

            var wallet = new Wallet
            {
                Balance = 0,
                Currency = Currency.EUR,
                User = user
            };

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(wallet);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var sut = new TransactionService(assertContext, walletServiceMock.Object, userServiceMock.Object,
                    foreignExchangeServiceMock.Object, cardServiceMock.Object, dateTimeProviderMock.Object);
                assertContext.Attach(wallet);
                var gameResult = await sut.GameTransaction(user, int.Parse(amount),
                    game, description, (TransactionType)Enum.Parse(typeof(TransactionType), type));
                Assert.IsInstanceOfType(gameResult, typeof(TransactionDTO));
                Assert.IsTrue(gameResult.Description == description+game);
                Assert.IsTrue(gameResult.Type == (TransactionType)Enum.Parse(typeof(TransactionType), type));
                Assert.IsTrue(gameResult.Amount == int.Parse(amount) / 2);
                Assert.IsTrue(gameResult.Username == userName);
            }
        }


        [DataTestMethod]
        [DataRow("FirstUser", "500", "4x3", "Stake on game 4x3", "Stake")]
        [DataRow("SecondUser", "2500", "8x5", "Win on game 8x5", "Win")]
        public async Task MakeTransactions_ThatAreAdded_ToTheBase(string userName, string amount, string game, string description, string type)
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "MakeTransactions_ThatAreAdded_ToTheBase")
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

            var user = new User
            {
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userName,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Email = "edward@gmail.com",
                FirstName = "Edward",
                LastName = "Evlogiev",
                DateOfBirth = DateTime.Parse("12.03.1993"),
                Role = UserRole.None,
            };

            var wallet = new Wallet
            {
                Balance = 0,
                Currency = Currency.EUR,
                User = user
            };

            var transaction = new Transaction()
            {
                Type = (TransactionType)Enum.Parse(typeof(TransactionType), type),
                Description = description + game,
                User = user,
                Amount = int.Parse(amount) / foreignExchangeDTO.Rates[Currency.EUR.ToString()],
                CreatedOn = dateTimeProviderMock.Object.Now,
                Currency = wallet.Currency
            };

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(wallet);
                await actContext.Transactions.AddAsync(transaction);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var sut = new TransactionService(assertContext, walletServiceMock.Object, userServiceMock.Object,
                    foreignExchangeServiceMock.Object, cardServiceMock.Object, dateTimeProviderMock.Object);
                assertContext.Attach(wallet);
                var gameResult = await sut.GameTransaction(user, int.Parse(amount),
                    game, description, (TransactionType)Enum.Parse(typeof(TransactionType), type));
                Assert.AreEqual(transaction, await assertContext.Transactions.Where(t => t.User == user).FirstOrDefaultAsync());
            }
        }
    }
}
