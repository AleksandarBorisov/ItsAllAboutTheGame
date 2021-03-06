﻿using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Services.Data.Exceptions;
using ItsAllAboutTheGame.Services.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.CardServiceTests
{
    [TestClass]
    public class DeleteCard_Should
    {
        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
        private CreditCard creditCard;
        private string cardNumber = "23232141412";
        private string cvv = "3232";
        private User user = new User();
        private IDateTimeProvider dateTimeProvider;

        [TestMethod]
        public async Task ThrowEntityNotFoundException_When_NoCardIsFound()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ThrowEntityNotFoundException_When_NoCardIsFound")
                .Options;

            dateTimeProvider = new DateTimeProvider();

            //Act & Assert
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(actContext, dateTimeProvider);
                int noCard = 0;

                await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => await cardService.DeleteCard(noCard));
            }
        }

        [TestMethod]
        public async Task ReturnCardToDelete_WhenValidParams_Passed()
        {
            //Arrange
            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
            .UseInMemoryDatabase(databaseName: "ReturnCardToDelete_WhenValidParams_Passed")
                .Options;

            creditCard = new CreditCard
            {
                CardNumber = cardNumber,
                CVV = cvv,
                ExpiryDate = DateTime.Parse("02.03.2018"),
                User = user,
                UserId = user.Id,
            };

            dateTimeProvider = new DateTimeProvider();

            //Act 
            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                await actContext.CreditCards.AddAsync(creditCard);
                await actContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
            {
                var cardService = new CardService(assertContext, dateTimeProvider);
                var cardToDelete = await cardService.DeleteCard(creditCard.Id);

                Assert.IsInstanceOfType(cardToDelete, typeof(CreditCard));
                Assert.IsTrue(cardToDelete.IsDeleted);
            }
        }
    }
}
