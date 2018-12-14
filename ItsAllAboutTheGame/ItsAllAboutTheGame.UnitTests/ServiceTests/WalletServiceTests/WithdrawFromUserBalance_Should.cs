//using ItsAllAboutTheGame.Data;
//using ItsAllAboutTheGame.Data.Models;
//using ItsAllAboutTheGame.GlobalUtilities.Constants;
//using ItsAllAboutTheGame.GlobalUtilities.Enums;
//using ItsAllAboutTheGame.Services.Data;
//using ItsAllAboutTheGame.Services.Data.Contracts;
//using ItsAllAboutTheGame.Services.Data.DTO;
//using ItsAllAboutTheGame.Services.Data.Exceptions;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ItsAllAboutTheGame.UnitTests.ServiceTests.WalletServiceTests
//{
//    [TestClass]
//    public class WithdrawFromUserBalance_Should
//    {
//        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
//        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
//        private ForeignExchangeDTO foreignExchangeDTO;
//        private User user;
//        private Wallet userWallet;

//        [TestMethod]
//        public async Task ReturnTransactionDTO_WhenPassedValidParams()
//        {
//            //Arrange
//            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
//            .UseInMemoryDatabase(databaseName: "ReturnTransactionDTO_WhenPassedValidParams")
//                .Options;

//            decimal amount = 1000;

//            user = new User
//            {
//                Id = "randomId",
//                Cards = new List<CreditCard>(),
//                Transactions = new List<Transaction>(),
//                UserName = "Koicho",
//                CreatedOn = DateTime.Now,
//                Email = "testmail@gmail",
//                FirstName = "Koichokov",
//                LastName = "Velichkov",
//                DateOfBirth = DateTime.Parse("02.01.1996"),
//                Role = UserRole.None,
//            };

//            userWallet = new Wallet
//            {
//                Currency = Currency.GBP,
//                Balance = 2000, // we put 2000 in balance otherwise the method will return an null DTO (business logic)
//                User = user,
//            };

//            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

//            foreignExchangeDTO = new ForeignExchangeDTO
//            {
//                Base = GlobalConstants.BaseCurrency,
//                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
//            };

//            var currencies = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

//            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                await actContext.Users.AddAsync(user);
//                await actContext.Wallets.AddAsync(userWallet);
//                await actContext.SaveChangesAsync();
//            }


//            //Assert
//            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                var walletService = new WalletService(assertContext, foreignExchangeServiceMock.Object);
//                var transactionDTO = await walletService.WithdrawFromUserBalance(user, amount);

//                Assert.IsInstanceOfType(transactionDTO, typeof(TransactionDTO));
//            }
//        }


//        [TestMethod]
//        public async Task NormaliseBalance_After_Withdrawal()
//        {
//            //Arrange
//            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
//            .UseInMemoryDatabase(databaseName: "NormaliseBalance_After_Withdrawal")
//                .Options;

//            decimal amount = 1000;

//            user = new User
//            {
//                Id = "randomId",
//                Cards = new List<CreditCard>(),
//                Transactions = new List<Transaction>(),
//                UserName = "Koicho",
//                CreatedOn = DateTime.Now,
//                Email = "testmail@gmail",
//                FirstName = "Koichokov",
//                LastName = "Velichkov",
//                DateOfBirth = DateTime.Parse("02.01.1996"),
//                Role = UserRole.None,
//            };

//            userWallet = new Wallet
//            {
//                Currency = Currency.GBP,
//                Balance = 2000, // we put 2000 in balance otherwise the method will return an null DTO (business logic)
//                User = user,
//            };

//            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

//            foreignExchangeDTO = new ForeignExchangeDTO
//            {
//                Base = GlobalConstants.BaseCurrency,
//                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
//                // All of the rates are with value 2 for testing purposes (value => 2m)
//            };

//            var currencies = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

//            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                await actContext.Users.AddAsync(user);
//                await actContext.Wallets.AddAsync(userWallet);
//                await actContext.SaveChangesAsync();
//            }


//            //Assert
//            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                var walletService = new WalletService(assertContext, foreignExchangeServiceMock.Object);
//                var transactionDTO = await walletService.WithdrawFromUserBalance(user, amount);
//                var currentWalletBalance = await assertContext.Wallets.Where(u => u.User == user).FirstOrDefaultAsync();
//                Assert.AreEqual(1500, currentWalletBalance.Balance);
//            }
//        }

//        [TestMethod]
//        public async Task ThrowException_When_NullOrInvalidParamsPassed()
//        {
//            // Arrange
//            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
//            .UseInMemoryDatabase(databaseName: "ThrowException_When_NullOrInvalidParamsPassed")
//                .Options;

//            decimal amount = 1000;

//            user = new User();

//            userWallet = new Wallet
//            {
//                Currency = Currency.GBP,
//                Balance = 2000, // we put 2000 in balance otherwise the method will return an null DTO (business logic)
//                User = user,
//            };

//            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

//            foreignExchangeDTO = new ForeignExchangeDTO
//            {
//                Base = GlobalConstants.BaseCurrency,
//                Rates = null
//            };

//            var currencies = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

//            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                await actContext.Users.AddAsync(user);
//                await actContext.Wallets.AddAsync(userWallet);
//                await actContext.SaveChangesAsync();
//            }


//            //Assert
//            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                var walletService = new WalletService(assertContext, foreignExchangeServiceMock.Object);
//                await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => await walletService.WithdrawFromUserBalance(user, amount));
//            }
//        }
//    }
//}
