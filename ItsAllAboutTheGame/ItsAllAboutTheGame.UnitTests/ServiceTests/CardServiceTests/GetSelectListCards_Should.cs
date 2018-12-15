using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.CardServiceTests
{
    [TestClass]
    public class GetSelectListCards_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private CreditCard creditCard;
        private string cardNumberOne = "23232141412";
        private string cvvOne = "3232";
        private string cardNumberTwo = "23232141212";
        private string cvvTwo = "3212";
        private CreditCard creditCardTwo;
        private User user;
        private string userId = "randomId";
        private string userName = "Koicho";
        private string email = "testmail@gmail";
        private string firstName = "Koichkov";
        private string lastName = "Velichkov";
        private IDateTimeProvider dateTimeProvider;

        [TestMethod]
        public async Task ReturnListOfCards_AfterSelectingUserExistingCards()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnListOfCards_AfterSelectingUserExistingCards")
                .Options;

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

            creditCard = new CreditCard
            {
                CardNumber = cardNumberOne,
                CVV = cvvOne,
                ExpiryDate = DateTime.Parse("02.03.2020"),
                User = user,
                UserId = user.Id,
            };

            creditCardTwo = new CreditCard
            {
                CardNumber = cardNumberTwo,
                CVV = cvvTwo,
                ExpiryDate = DateTime.Parse("02.03.2016"),
                User = user,
                UserId = user.Id,
            };



            //Act 
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.Users.AddAsync(user);
                await actContext.CreditCards.AddAsync(creditCard);
                await actContext.CreditCards.AddAsync(creditCardTwo);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(assertContext, dateTimeProvider);
                var userCards = await cardService.GetSelectListCards(user);

                Assert.IsInstanceOfType(userCards, typeof(IEnumerable<SelectListItem>));
            }
        }
    }
}
