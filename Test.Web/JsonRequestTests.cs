using Moq;
using Moq.Protected;
using SharedTestData;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Web;
using Xunit;

namespace Test.Web
{
    /// <summary>
    /// Tests
    /// </summary>
    [Trait("Web","Json Requests")]
    public class JsonRequestTests
    {
        private Mock<HttpMessageHandler> sut(HttpStatusCode httpStatusCode, string stringContent)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
              .Protected()
              .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(new HttpResponseMessage
              {
                  StatusCode = httpStatusCode,
                  Content = new StringContent(stringContent),
              });

            return mockHttpMessageHandler;
        }

        [Fact(DisplayName = "Request GET 200 returns Model.")]
        public async Task Request_GET_returnsModel200()
        {
            //Arrange
            var mockHttpMessageHandler = sut(HttpStatusCode.OK, "{'id':1,'fullName':'John Smith','age':21}");
            var request = new JsonRequest(new HttpClient(mockHttpMessageHandler.Object));
            //Act
            var result = await request.Get<Person>("http://webAddress", It.IsAny<CancellationToken>());
            //Assert
            Assert.Equal(result, new Person { Id = 1, FullName = "John Smith", Age = 21 });
        }

        [Fact(DisplayName = "Request GET 200 returns 404 Not found.")]
        public async Task Request_GET_returnsNotFound()
        {
            //Arrange
            var mockHttpMessageHandler = sut(HttpStatusCode.NotFound, "Not Found");
            var request = new JsonRequest(new HttpClient(mockHttpMessageHandler.Object));
            //Act
            var ex = await Assert.ThrowsAsync<ApiException>(() => request.Get<Person>("http://webAddress", It.IsAny<CancellationToken>()));
            //Assert
            Assert.True(ex.StatusCode == 404);
        }

        [Fact(DisplayName = "Request POST 201 returns Model.")]
        public async Task Request_POST_CreatesModel()
        {
            //Arrange
            var mockHttpMessageHandler = sut(HttpStatusCode.Created, "{'id':1,'fullName':'John Smith','age':21}");
            var request = new JsonRequest(new HttpClient(mockHttpMessageHandler.Object));
            //Act
            var result = await request.Post<Person>("http://webAddress", new Person { Id = 1, FullName = "John Smith", Age = 21 }, It.IsAny<CancellationToken>());
            //Assert
            Assert.Equal(result, new Person { Id = 1, FullName = "John Smith", Age = 21 });
        }

        [Fact(DisplayName = "Request PUT 200 updates Model.")]
        public async Task Request_PUT_UpdatesModel()
        {
            //Arrange
            var mockHttpMessageHandler = sut(HttpStatusCode.OK, "{'id':1,'fullName':'John Smith','age':21}");
            var request = new JsonRequest(new HttpClient(mockHttpMessageHandler.Object));
            //Act
            var result = await request.Put<Person>("http://webAddress", new Person { Id = 1, FullName = "John Smith", Age = 21 }, It.IsAny<CancellationToken>());
            //Assert
            Assert.Equal(result, new Person { Id = 1, FullName = "John Smith", Age = 21 });
        }
    }
}
