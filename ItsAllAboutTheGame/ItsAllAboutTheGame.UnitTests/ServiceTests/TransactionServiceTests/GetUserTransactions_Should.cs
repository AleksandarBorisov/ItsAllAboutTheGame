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
    public class GetUserTransactions_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ICardService> cardServiceMock;
        private Mock<IDateTimeProvider> dateTimeProviderMock;
        private ForeignExchangeDTO foreignExchangeDTO;
        private User user;
        private string userName = "Koicho";
        private string email = "testmail@gmail";
        private string firstName = "Koichkov";
        private string lastName = "Velichkov";

        [TestMethod]
        public async Task ReturnTransactionListDTO_With_ConcreteUserParams()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnTransactionListDTO_With_ConcreteUserParam")
                .Options;

            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            walletServiceMock = new Mock<IWalletService>();
            userServiceMock = new Mock<IUserService>();
            cardServiceMock = new Mock<ICardService>();

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

            var userTwo = new User
            {
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = "blabla",
                CreatedOn = dateTimeProviderMock.Object.Now.AddDays(1),
                Email = "email",
                FirstName = "firstName",
                LastName = "lastName",
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            var transaction = new Transaction
            {
                Type = TransactionType.Deposit,
                Description = GlobalConstants.DepositDescription + "3232".PadLeft(16, '*'),
                User = user,
                Amount = 1000,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Currency = Currency.BGN
            };

            var transactionTwo = new Transaction
            {
                Type = TransactionType.Deposit,
                Description = GlobalConstants.DepositDescription + "3102".PadLeft(16, '*'),
                User = userTwo,
                Amount = 1500,
                CreatedOn = dateTimeProviderMock.Object.Now.AddDays(1), // adding 1 day otherwise it will be created at the exact same time as the first transaction
                Currency = Currency.USD
            };

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Users.AddAsync(userTwo);
                await actContext.Transactions.AddAsync(transaction);
                await actContext.Transactions.AddAsync(transactionTwo);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var sut = new TransactionService(assertContext, walletServiceMock.Object, userServiceMock.Object,
                    foreignExchangeServiceMock.Object, cardServiceMock.Object, dateTimeProviderMock.Object);
                var listDTO = await sut.GetUserTransactions(user.UserName); // we get only the DTO of one of the users

                Assert.IsInstanceOfType(listDTO, typeof(TransactionListDTO));
                Assert.IsTrue(listDTO.TotalItemCount == 1);
            }
        }

        [TestMethod]
        public async Task ReturnEmptyTransactionListDTO_With_UserWalletCurrency_WhenUserHasNoTransactions()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnEmptyTransactionListDTO_With_UserWalletCurrency_WhenUserHasNoTransactions")
                .Options;

            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            walletServiceMock = new Mock<IWalletService>();
            userServiceMock = new Mock<IUserService>();
            cardServiceMock = new Mock<ICardService>();

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

            var userTwo = new User
            {
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = "blabla",
                CreatedOn = dateTimeProviderMock.Object.Now.AddDays(1),
                Email = "email",
                FirstName = "firstName",
                LastName = "lastName",
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            var wallet = new Wallet
            {
                 Balance = 0,
                 Currency = Currency.GBP,
                 User = user
            };

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Users.AddAsync(userTwo);
                await actContext.Wallets.AddAsync(wallet);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var sut = new TransactionService(assertContext, walletServiceMock.Object, userServiceMock.Object,
                    foreignExchangeServiceMock.Object, cardServiceMock.Object, dateTimeProviderMock.Object);
                var listDTO = await sut.GetUserTransactions(user.UserName); // we get only the DTO of one of the users

                Assert.IsInstanceOfType(listDTO, typeof(TransactionListDTO));
                Assert.IsTrue(listDTO.TotalItemCount == 0);
            }
        }

        [TestMethod]
        public async Task ReturnTransactionListDTO_With_ConcreteUserParamsWithAscSortOrder_OnDateOfCreation()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnTransactionListDTO_With_ConcreteUserParamsWithAscSortOrder_OnDateOfCreation")
                .Options;

            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            walletServiceMock = new Mock<IWalletService>();
            userServiceMock = new Mock<IUserService>();
            cardServiceMock = new Mock<ICardService>();

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
                Type = TransactionType.Stake,
                Description = GlobalConstants.DepositDescription + "3232".PadLeft(16, '*'),
                User = user,
                Amount = 1000,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Currency = Currency.BGN
            };

            var transactionTwo = new Transaction
            {
                Type = TransactionType.Deposit,
                Description = GlobalConstants.DepositDescription + "3102".PadLeft(16, '*'),
                User = user,
                Amount = 1500,
                CreatedOn = dateTimeProviderMock.Object.Now.AddDays(1), // adding 1 day otherwise it will be created at the exact same time as the first transaction
                Currency = Currency.USD
            };

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

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
                var listDTO = await sut.GetUserTransactions(user.UserName,1,5,"CreatedOn_Asc"); // we get only the DTO of one of the users

                Assert.IsInstanceOfType(listDTO, typeof(TransactionListDTO));
                Assert.IsTrue(listDTO.Transactions.Select(c => c.Type).First() == TransactionType.Stake);
                Assert.IsTrue(listDTO.TotalItemCount == 2);
            }
        }
    }
}
