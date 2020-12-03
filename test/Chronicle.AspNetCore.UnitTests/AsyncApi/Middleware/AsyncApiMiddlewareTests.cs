using System.Threading.Tasks;
using Chronicle.AspNetCore.AsyncApi.Helpers.Interfaces;
using Chronicle.AspNetCore.AsyncApi.Middleware;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace Chronicle.AspNetCore.UnitTests.AsyncApi.Middleware
{
    [TestFixture]
    public class AsyncApiMiddlewareTests
    {
        private Mock<HttpContext> mockHttpContext;
        private Mock<HttpRequest> mockHttpRequest;
        private Mock<HttpResponse> mockHttpResponse;
        private Mock<RequestDelegate> mockRequestDelegate;
        private Mock<IRequestHelper> mockRequestHelper;
        private Mock<IResponseHelper> mockResponseHelper;
        private AsyncApiMiddleware target;

        [SetUp]
        public void SetUp()
        {
            this.mockHttpContext = new Mock<HttpContext>(MockBehavior.Strict);
            this.mockHttpRequest = new Mock<HttpRequest>(MockBehavior.Strict);
            this.mockHttpResponse = new Mock<HttpResponse>(MockBehavior.Strict);
            this.mockRequestDelegate = new Mock<RequestDelegate>(MockBehavior.Strict);
            this.mockRequestHelper = new Mock<IRequestHelper>(MockBehavior.Strict);
            this.mockResponseHelper = new Mock<IResponseHelper>(MockBehavior.Strict);

            this.target = new AsyncApiMiddleware(this.mockRequestDelegate.Object, this.mockRequestHelper.Object, this.mockResponseHelper.Object);
        }

        [Test]
        public async Task Invoke_NotRequestingDocument_InvokesNext()
        {
            this.mockHttpContext
                .Setup(m => m.Request)
                .Returns(this.mockHttpRequest.Object);
            this.mockRequestHelper
                .Setup(m => m.RequestingAsyncApiDocument(this.mockHttpRequest.Object))
                .Returns(false);
            this.mockRequestDelegate
                .Setup(m => m.Invoke(this.mockHttpContext.Object))
                .Returns(Task.CompletedTask);

            await this.target.Invoke(this.mockHttpContext.Object);

            this.mockRequestDelegate.Verify(m => m.Invoke(this.mockHttpContext.Object));
        }

        [Test]
        public async Task Invoke_RequestingYamlDocument_ReturnsYamlDocument()
        {
            const bool isYaml = true;
            this.mockHttpContext
                .Setup(m => m.Request)
                .Returns(this.mockHttpRequest.Object);
            this.mockHttpContext
                .Setup(m => m.Response)
                .Returns(this.mockHttpResponse.Object);
            this.mockRequestHelper
                .Setup(m => m.RequestingAsyncApiDocument(this.mockHttpRequest.Object))
                .Returns(true);
            this.mockRequestHelper
                .Setup(m => m.RequestingDocumentAsYaml(this.mockHttpRequest.Object))
                .Returns(isYaml);
            this.mockResponseHelper
                .Setup(m => m.RespondWithDocument(this.mockHttpResponse.Object, isYaml))
                .Returns(Task.CompletedTask);

            await this.target.Invoke(this.mockHttpContext.Object);

            this.mockResponseHelper.Verify(m => m.RespondWithDocument(this.mockHttpResponse.Object, isYaml));
        }

        [Test]
        public async Task Invoke_RequestingJsonDocument_ReturnsJsonDocument()
        {
            const bool isYaml = false;
            this.mockHttpContext
                .Setup(m => m.Request)
                .Returns(this.mockHttpRequest.Object);
            this.mockHttpContext
                .Setup(m => m.Response)
                .Returns(this.mockHttpResponse.Object);
            this.mockRequestHelper
                .Setup(m => m.RequestingAsyncApiDocument(this.mockHttpRequest.Object))
                .Returns(true);
            this.mockRequestHelper
                .Setup(m => m.RequestingDocumentAsYaml(this.mockHttpRequest.Object))
                .Returns(isYaml);
            this.mockResponseHelper
                .Setup(m => m.RespondWithDocument(this.mockHttpResponse.Object, isYaml))
                .Returns(Task.CompletedTask);

            await this.target.Invoke(this.mockHttpContext.Object);

            this.mockResponseHelper.Verify(m => m.RespondWithDocument(this.mockHttpResponse.Object, isYaml));
        }

        [Test]
        public async Task Invoke_DocumentGeneratorException_ReturnsNotFound()
        {
            this.mockHttpContext
                .Setup(m => m.Request)
                .Returns(this.mockHttpRequest.Object);
            this.mockHttpContext
                .Setup(m => m.Response)
                .Returns(this.mockHttpResponse.Object);
            this.mockRequestHelper
                .Setup(m => m.RequestingAsyncApiDocument(this.mockHttpRequest.Object))
                .Returns(true);
            this.mockResponseHelper
                .Setup(m => m.RespondWithNotFound(this.mockHttpResponse.Object));
            
            await this.target.Invoke(this.mockHttpContext.Object);

            this.mockResponseHelper.Verify(m => m.RespondWithNotFound(this.mockHttpResponse.Object));
        }
    }
}
