//using ItsAllAboutTheGame.Data;
//using ItsAllAboutTheGame.Data.Models;
//using ItsAllAboutTheGame.GlobalUtilities.Enums;
//using ItsAllAboutTheGame.Services.Data.Exceptions;
//using ItsAllAboutTheGame.Services.Data.Services;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace ItsAllAboutTheGame.UnitTests.ServiceTests.CardServiceTests
//{
//    [TestClass]
//    public class DeleteCard_Should
//    {
//        private DbContextOptions<ItsAllAboutTheGameDbContext> contextOptions;
//        private CreditCard creditCard;
//        private User user = new User();

//        [TestMethod]
//        public async Task ThrowEntityNotFoundException_When_NoCardIsFound()
//        {
//            //Arrange
//            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
//            .UseInMemoryDatabase(databaseName: "ThrowEntityNotFoundException_When_NoCardIsFound")
//                .Options;

//            //Act & Assert

//            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                var cardService = new CardService(actContext);
//                int noCard = 0;

//                await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => await cardService.DeleteCard(noCard));
//            }
//        }
        
//        [TestMethod]
//        public async Task ReturnCardToDelete_WhenValidParams_Passed()
//        {
//            //Arrange
//            contextOptions = new DbContextOptionsBuilder<ItsAllAboutTheGameDbContext>()
//            .UseInMemoryDatabase(databaseName: "ReturnCardToDelete_WhenValidParams_Passed")
//                .Options;


//            creditCard = new CreditCard
//            {
//                CardNumber = "23232141412",
//                CVV = "3232",
//                ExpiryDate = DateTime.Parse("02.03.2018"),
//                User = user,
//                UserId = user.Id,
//            };


//            //Act 
//            using (var actContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                await actContext.CreditCards.AddAsync(creditCard);
//                await actContext.SaveChangesAsync();
//            }


//            //Assert
//            using (var assertContext = new ItsAllAboutTheGameDbContext(contextOptions))
//            {
//                var cardService = new CardService(assertContext);
//                var cardToDelete = await cardService.DeleteCard(creditCard.Id);

//                Assert.IsInstanceOfType(cardToDelete, typeof(CreditCard));
//                Assert.IsTrue(cardToDelete.IsDeleted);
//            }
//        }
//    }
//}
