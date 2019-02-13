using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Services.Data.Contracts.ForeignExchangeApiService;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.ForeignExchangeApiService;
using ItsAllAboutTheGame.Services.External.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.UnitTests.ServiceTests.ForeignExchangeServiceTests
{
    [TestClass]
    public class GetConvertionRates_Should
    {
        private IMemoryCache memoryCacheMock;
        private Mock<IJsonDeserializer> jsonDeserializerMock;
        private Mock<IDateTimeProvider> dateTimeProviderMock;
        private Mock<IForeignExchangeApiCaller> foreignExchangeApiCallerMock;
        private string fakeDate = "01.01.2000";
        private string fakeApiResponse = @"{\""date\"":\""2000-12-14\"",\""rates\"":{\""USD\"":1.0,\""EUR\"":0.90,\""BGN\"":1.90,\""GBP\"":0.90},\""base\"":\""USD\""}";
        private ForeignExchangeDTO fakeResponseObject = new ForeignExchangeDTO() { Base = GlobalConstants.BaseCurrency };

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
            memoryCacheMock = new MemoryCache(new MemoryCacheOptions());
            jsonDeserializerMock = new Mock<IJsonDeserializer>();
            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            foreignExchangeApiCallerMock = new Mock<IForeignExchangeApiCaller>();
        }

        [TestMethod]
        public async Task ReturnCorrectObjects_WhenParametersAreValid()
        {
            //Arrange
            dateTimeProviderMock
                .Setup(date => date.UtcNow)
                .Returns(DateTime.Parse(fakeDate));

            foreignExchangeApiCallerMock
                .Setup(exchange => exchange.GetCurrencyData(It.IsAny<string>()))
                .ReturnsAsync(fakeApiResponse);

            jsonDeserializerMock
                .Setup(deserializer => deserializer.Deserialize<ForeignExchangeDTO>(fakeApiResponse))
                .Returns(fakeResponseObject);

            //Act
            var command = new ForeignExchangeService(jsonDeserializerMock.Object,
                foreignExchangeApiCallerMock.Object, memoryCacheMock, dateTimeProviderMock.Object);
            var result = await command.GetConvertionRates();

            //Assert
            Assert.IsInstanceOfType(result, typeof(ForeignExchangeDTO));
            Assert.AreEqual(GlobalConstants.BaseCurrency.ToString(), result.Base);
        }
    }
}
