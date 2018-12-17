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
using X.PagedList;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.TransactionServiceTests
{
    [TestClass]
    public class GetAllTransactions_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ICardService> cardServiceMock;
        private Mock<IDateTimeProvider> dateTimeProviderMock;
        private User user;
        private string userId = "randomId";
        private string userName = "Koicho";
        private string email = "testmail@gmail";
        private string firstName = "Koichkov";
        private string lastName = "Velichkov";
        private User userTwo;
        private string userIdTwo = "randomId2";
        private string userNameTwo = "Koicho2";
        private string emailTwo = "testmail@gmail2";
        private string firstNameTwo = "Koichkov2";
        private string lastNameTwo = "Velichkov2";

        [TestMethod]
        public async Task ReturnPageList_OfTransactionDTO_WithDefaultParams()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnPageList_OfTransactionDTO_WithDefaultParams")
                .Options;

            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();
            walletServiceMock = new Mock<IWalletService>();
            userServiceMock = new Mock<IUserService>();
            cardServiceMock = new Mock<ICardService>();

            user = new User
            {
                Id = userId,
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

            userTwo = new User
            {
                Id = userIdTwo,
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userNameTwo,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Email = emailTwo,
                FirstName = firstNameTwo,
                LastName = lastNameTwo,
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
                var pageList = await sut.GetAllTransactions();

                Assert.IsInstanceOfType(pageList, typeof(IPagedList<TransactionDTO>));
                Assert.IsTrue(pageList.Count == 2);
                Assert.IsTrue(pageList.Select(c => c.Username).First() == userTwo.UserName);
            }
        }

        [TestMethod]
        public async Task ReturnPageList_OfTransactionDTO_WithConcreteUserNameSearch()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnPageList_OfTransactionDTO_WithConcreteUserNameSearch")
                .Options;

            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();
            walletServiceMock = new Mock<IWalletService>();
            userServiceMock = new Mock<IUserService>();
            cardServiceMock = new Mock<ICardService>();

            user = new User
            {
                Id = userId,
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

            userTwo = new User
            {
                Id = userIdTwo,
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userNameTwo,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Email = emailTwo,
                FirstName = firstNameTwo,
                LastName = lastNameTwo,
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
                CreatedOn = dateTimeProviderMock.Object.Now,
                Currency = Currency.USD
            };

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
                var pageList = await sut.GetAllTransactions(userTwo.UserName);

                Assert.IsInstanceOfType(pageList, typeof(IPagedList<TransactionDTO>));
                Assert.IsTrue(pageList.Select(k => k.Username).FirstOrDefault() == userTwo.UserName);
            }
        }

        [TestMethod]
        public async Task ReturnPageList_OfTransactionDTO_WithConcreteSortOrder()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnPageList_OfTransactionDTO_WithConcreteSortOrder")
                .Options;

            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();
            walletServiceMock = new Mock<IWalletService>();
            userServiceMock = new Mock<IUserService>();
            cardServiceMock = new Mock<ICardService>();

            user = new User
            {
                Id = userId,
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

            userTwo = new User
            {
                Id = userIdTwo,
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userNameTwo,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Email = emailTwo,
                FirstName = firstNameTwo,
                LastName = lastNameTwo,
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
                CreatedOn = dateTimeProviderMock.Object.Now.AddDays(1),
                Currency = Currency.USD
            };


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
                var pageList = await sut.GetAllTransactions(null,1,5,"CreatedOn_asc"); // the last parameter is not a default

                Assert.IsInstanceOfType(pageList, typeof(IPagedList<TransactionDTO>));
                Assert.IsTrue(pageList.Count == 2);
                Assert.IsTrue(pageList.Select(c => c.Username).First() == user.UserName);
            }
        }


        [TestMethod]
        public async Task ReturnPageList_OfTransactionDTO_WithConcretePageSize()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnPageList_OfTransactionDTO_WithConcretePageSize")
                .Options;

            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();
            walletServiceMock = new Mock<IWalletService>();
            userServiceMock = new Mock<IUserService>();
            cardServiceMock = new Mock<ICardService>();

            user = new User
            {
                Id = userId,
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

            userTwo = new User
            {
                Id = userIdTwo,
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userNameTwo,
                CreatedOn = dateTimeProviderMock.Object.Now,
                Email = emailTwo,
                FirstName = firstNameTwo,
                LastName = lastNameTwo,
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
                CreatedOn = dateTimeProviderMock.Object.Now.AddDays(1),
                Currency = Currency.USD
            };


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
                var pageList = await sut.GetAllTransactions(null,1,23); 

                Assert.IsInstanceOfType(pageList, typeof(IPagedList<TransactionDTO>));
                Assert.IsTrue(pageList.PageSize == 23); // asserting if the pagesize is the same as the one passed (23)
            }
        }
    }
}
