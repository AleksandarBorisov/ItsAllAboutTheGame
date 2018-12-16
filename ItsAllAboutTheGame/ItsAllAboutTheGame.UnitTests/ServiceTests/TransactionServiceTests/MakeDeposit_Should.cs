using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.TransactionServiceTests
{
    [TestClass]
    public class MakeDeposit_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private Mock<IForeignExchangeService> foreignExchangeServiceMock;
        private Mock<IWalletService> walletServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ICardService> cardServiceMock;
        private Mock<IDateTimeProvider> dateTimeProvider;
        private ForeignExchangeDTO foreignExchangeDTO;
        private User user;
        private string userName = "Koicho";
        private string email = "testmail@gmail";
        private string firstName = "Koichkov";
        private string lastName = "Velichkov";
        private CreditCard creditCard;
        private string cardNumber = "23232141412";
        private string cvv = "3232";
        private string lastDigits = "1412";
        private Wallet userWallet;
        private WalletDTO userWalletDTO;

        [TestMethod]
        public async Task ReturnTransactionDTO_When_PassedValidParams()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnTransactionDTO_WhenPassedValidParams")
                .Options;

            dateTimeProvider = new Mock<IDateTimeProvider>();
            foreignExchangeServiceMock = new Mock<IForeignExchangeService>();
            walletServiceMock = new Mock<IWalletService>();
            userServiceMock = new Mock<IUserService>();
            cardServiceMock = new Mock<ICardService>();
            decimal amount = 1000;

            user = new User
            {
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = userName,
                CreatedOn = dateTimeProvider.Object.Now,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateTime.Parse("02.01.1996"),
                Role = UserRole.None
            };

            userWallet = new Wallet
            {
                Currency = Currency.GBP,
                Balance = 2000, // we put 2000 in balance otherwise the method will return an null DTO (business logic)
                User = user,
            };

            creditCard = new CreditCard
            {
                CardNumber = cardNumber,
                CVV = cvv,
                LastDigits = lastDigits,
                ExpiryDate = DateTime.Parse("03.03.2022"),
                User = user,
                CreatedOn = dateTimeProvider.Object.Now
            };

            foreignExchangeDTO = new ForeignExchangeDTO
            {
                Base = GlobalConstants.BaseCurrency,
                Rates = Enum.GetNames(typeof(Currency)).ToDictionary(name => name, value => 2m)
            };

            userWalletDTO = new WalletDTO(userWallet, foreignExchangeDTO);

            var rates = foreignExchangeServiceMock.Setup(fesm => fesm.GetConvertionRates()).ReturnsAsync(foreignExchangeDTO);
            var wallet = walletServiceMock.Setup(wsm => wsm.GetUserWallet(user)).ReturnsAsync(userWalletDTO);

            //Act
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.Wallets.AddAsync(userWallet);
                await actContext.CreditCards.AddAsync(creditCard);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var sut = new TransactionService(assertContext, walletServiceMock.Object,
                    userServiceMock.Object, foreignExchangeServiceMock.Object, cardServiceMock.Object, dateTimeProvider.Object);
                assertContext.Attach(creditCard);
                var transactionDTO = await sut.MakeDeposit(user, creditCard.Id, amount);

                Assert.IsInstanceOfType(transactionDTO, typeof(TransactionDTO));
            }
        }
    }
}
