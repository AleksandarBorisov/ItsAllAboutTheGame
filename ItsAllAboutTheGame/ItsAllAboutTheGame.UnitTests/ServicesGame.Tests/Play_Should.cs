using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Services.Core.Game;
using ItsAllAboutTheGame.Services.Game.DTO;
using ItsAllAboutTheGame.Services.Game.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections;
using System.Linq;

namespace ItsAllAboutTheGame.UnitTests.ServicesGame.Tests
{
    [TestClass]
    public class Play_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
        private Mock<IGameRandomizer> gameRandomizerMock;
        private int amount = 10;

        [TestMethod]
        public void ReturnCorrectObjects_WhenParametersAreValid()
        {
            //Arrange
            gameRandomizerMock = new Mock<IGameRandomizer>();
            gameRandomizerMock.Setup(randomizer => randomizer.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(50);

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
            var result = command.Play(amount ,GlobalConstants.GameOneGrid);

            //Assert
            Assert.IsNotNull(result.Grid);
            Assert.AreEqual(GlobalConstants.GameOneGrid.Split('x')[0], result.Grid.GetLength(0).ToString());
            Assert.AreEqual(GlobalConstants.GameOneGrid.Split('x')[1], result.Grid.GetLength(1).ToString());
        }

        [TestMethod]
        [DataRow(5, 0d)]
        [DataRow(20, (double)(12 * 0.8 * 10))]
        [DataRow(55, (double)(12 * 0.6 * 10))]
        [DataRow(80, (double)(12 * 0.4 * 10))]
        public void ReturnCorrectValues_WhenParametersAreValid(int value, double expectedWin)
        {
            //Arrange
            gameRandomizerMock = new Mock<IGameRandomizer>();
            gameRandomizerMock.Setup(randomizer => randomizer.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(value);

            //Act
            var command = new Game(gameRandomizerMock.Object);
            var result = command.Play(amount, GlobalConstants.GameOneGrid);

            //Assert
            Assert.AreEqual((int)expectedWin,(int)result.WonAmount);
        }

        [TestMethod]
        public void ReturnCorrectWinningRows_WhenParametersAreValid()
        {
            //Arrange
            gameRandomizerMock = new Mock<IGameRandomizer>();
            gameRandomizerMock.Setup(randomizer => randomizer.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);

            //Act
            var command = new Game(gameRandomizerMock.Object);
            var result = command.Play(amount, GlobalConstants.GameOneGrid);
            var expectedWinningRows = new bool[result.WinningRows.Count()];
            //Assert
            CollectionAssert.AreNotEquivalent(expectedWinningRows, result.WinningRows as ICollection);
            
        }
    }
}
