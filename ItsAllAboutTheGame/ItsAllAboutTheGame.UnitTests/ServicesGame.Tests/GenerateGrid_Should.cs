using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Services.Core.Game;
using ItsAllAboutTheGame.Services.Game.DTO;
using ItsAllAboutTheGame.Services.Game.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ItsAllAboutTheGame.UnitTests.ServicesGame.Tests
{
    [TestClass]
    public class GenerateGrid_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
        private Mock<IGameRandomizer> gameRandomizerMock;

        [TestMethod]
        public void ReturnCorrectObjects_WhenParametersAreValid()
        {
            //Arrange
            gameRandomizerMock = new Mock<IGameRandomizer>();
            gameRandomizerMock.Setup(randomizer => randomizer.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);

            //Act
            var command = new Game(gameRandomizerMock.Object);
            var result = command.GenerateGrid(GlobalConstants.GameOneGrid);

            //Assert
            Assert.IsInstanceOfType(result, typeof(GameResultDTO));
            CollectionAssert.AllItemsAreInstancesOfType(result.Grid, typeof(GameResults));
        }

        [TestMethod]
        public void ReturnCorrectGrid_WhenParametersAreValid()
        {
            //Arrange
            gameRandomizerMock = new Mock<IGameRandomizer>();
            gameRandomizerMock.Setup(randomizer => randomizer.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);

            //Act
            var command = new Game(gameRandomizerMock.Object);
            var result = command.GenerateGrid(GlobalConstants.GameOneGrid);

            Assert.IsNotNull(result.Grid);
            Assert.AreEqual(GlobalConstants.GameOneGrid.Split('x')[0], result.Grid.GetLength(0).ToString());
            Assert.AreEqual(GlobalConstants.GameOneGrid.Split('x')[1], result.Grid.GetLength(1).ToString());
        }
    }
}
