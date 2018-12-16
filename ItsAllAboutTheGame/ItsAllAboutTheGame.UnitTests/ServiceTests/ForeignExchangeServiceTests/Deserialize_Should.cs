using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.ForeignExchangeApiService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.ForeignExchangeServiceTests
{
    [TestClass]
    public class Deserialize_Should
    {
        [TestMethod]
        public void ThrowArgumentNullException_WhenResponseStringIsNull()
        {
            //Act and Assert
            var command = new JsonDeserializer();
            Assert.ThrowsException<ArgumentNullException>(() => command.Deserialize<ForeignExchangeDTO>(null));
        }
    }
}
