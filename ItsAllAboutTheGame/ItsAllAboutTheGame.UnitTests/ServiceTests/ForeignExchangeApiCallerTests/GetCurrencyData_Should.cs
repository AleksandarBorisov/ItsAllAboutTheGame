//using ItsAllAboutTheGame.Services.External;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using Moq.Protected;
//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ItsAllAboutTheGame.UnitTests.ServiceTests.ForeignExchangeApiCallerTests
//{
//    [TestClass]
//    public class GetCurrencyData_Should
//    {
//        private Mock<IHttpClientFactory> clientFactoryMock;
//        private Mock<HttpClient> httpClientMock;

//        [TestMethod]
//        public void ReturnCorrectResult_WhenParametersAreValid()
//        {
//            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

//            mockHttpMessageHandler
//                .Protected()
//                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new System.Uri("https://www.abv.bg")), ItExpr.IsAny<CancellationToken>())
//                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("TestResult") }));

//            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

//            clientFactoryMock = new Mock<IHttpClientFactory>();

//            clientFactoryMock.CallBase = true;
//            clientFactoryMock.Setup(factory => factory.CreateClient())
//                .Returns(httpClient);//Cannot Mock extension method Exception :(

//            var command = new ForeignExchangeApiCaller(clientFactoryMock.Object);

//            var result = command.GetCurrencyData("TestURL");

//        }
//    }
//}
