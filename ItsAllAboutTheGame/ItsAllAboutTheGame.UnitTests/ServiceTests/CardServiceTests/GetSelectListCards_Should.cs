﻿//using ItsAllAboutTheGame.Data;
//using ItsAllAboutTheGame.Data.Models;
//using ItsAllAboutTheGame.GlobalUtilities.Enums;
//using ItsAllAboutTheGame.Services.Data.Services;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace ItsAllAboutTheGame.UnitTests.ServiceTests.CardServiceTests
//{
//    [TestClass]
//    public class GetSelectListCards_Should
//    {
//        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
//        private CreditCard creditCard;
//        private CreditCard creditCardTwo;
//        private User user = new User();

//        [TestMethod]
//        public async Task ReturnListOfCards_AfterSelectingUserExistingCards()
//        {
//            //Arrange
//            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
//            .UseInMemoryDatabase(databaseName: "ReturnListOfCards_AfterSelectingUserExistingCards")
//                .Options;

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

//            creditCard = new CreditCard
//            {
//                CardNumber = "23232141412",
//                CVV = "3232",
//                ExpiryDate = DateTime.Parse("02.03.2020"),
//                User = user,
//                UserId = user.Id,
//            };

//            creditCardTwo = new CreditCard
//            {
//                CardNumber = "23232141212",
//                CVV = "3212",
//                ExpiryDate = DateTime.Parse("02.03.2016"),
//                User = user,
//                UserId = user.Id,
//            };



//            //Act 
//            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                await actContext.Users.AddAsync(user);
//                await actContext.CreditCards.AddAsync(creditCard);
//                await actContext.CreditCards.AddAsync(creditCardTwo);
//                await actContext.SaveChangesAsync();
//            }

//            //Assert
//            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                var cardService = new CardService(assertContext);
//                var userCards = await cardService.GetSelectListCards(user);

//                Assert.IsInstanceOfType(userCards, typeof(IEnumerable<SelectListItem>));
//            }
//        }
//    }
//}