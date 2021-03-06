﻿using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.WalletServiceTests
{
    [TestClass]
    public class UpdateUserWallet_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;

        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private User user;
        private string userId = "randomId";
        private string userName = "Koicho";
        private string email = "testmail@gmail";
        private string firstName = "Koichkov";
        private string lastName = "Velichkov";
        private Wallet userWallet;
        private ForeignExchangeDTO foreignExchangeDTO;
        private IDateTimeProvider dateTimeProvider;

        [TestMethod]
        public async Task ReturnWalletDTO_When_PassedCorrectParams()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnWalletDTO_When_PassedCorrectParams")
                .Options;

            decimal stake = 1500;

            dateTimeProvider = new DateTimeProvider();

            user = new User
            {
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userName,
                CreatedOn = dateTimeProvider.Now,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            userWallet = new Wallet
            {
                Currency = Currency.GBP,
                Balance = 0,
                User = user,
            };

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            var currencies = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(userWallet);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var walletService = new WalletService(assertContext, foreignExchangeServiceMock.Object, dateTimeProvider);
                assertContext.Attach(userWallet);
                var updateWallet = await walletService.UpdateUserWallet(user, stake);
                var userOfReturnedWallet = await assertContext.Users.Where(u => u.Wallet.Balance * 2 == updateWallet.Balance).FirstOrDefaultAsync();
                Assert.IsInstanceOfType(updateWallet, typeof(WalletDTO));
                Assert.IsTrue(userOfReturnedWallet == user);
            }
        }

        [TestMethod]
        public async Task ReturnCorrectUpdatedWallet()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnCorrectUpdatedWallet")
                .Options;

            decimal stake = 1000;

            dateTimeProvider = new DateTimeProvider();

            user = new User
            {
                Id = userId,
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userName,
                CreatedOn = dateTimeProvider.Now,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None,
            };

            userWallet = new Wallet
            {
                Currency = Currency.GBP,
                Balance = 0,
                User = user,
            };

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            var currencies = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(userWallet);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var walletService = new WalletService(assertContext, foreignExchangeServiceMock.Object, dateTimeProvider);
                var updateWallet = await walletService.UpdateUserWallet(user, stake);

                Assert.AreEqual(stake, updateWallet.Balance);
            }
        }

        [TestMethod]
        public async Task ThrowException_When_NullValuesArePassed()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ThrowException_When_NullValuesArePassed")
                .Options;

            decimal stake = 1000;

            user = new User();

            userWallet = new Wallet();

            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            var currencies = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);

            dateTimeProvider = new DateTimeProvider();

            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(userWallet);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var walletService = new WalletService(assertContext, foreignExchangeServiceMock.Object, dateTimeProvider);

                await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => await walletService.UpdateUserWallet(user, stake));
            }
        }
    }
}
