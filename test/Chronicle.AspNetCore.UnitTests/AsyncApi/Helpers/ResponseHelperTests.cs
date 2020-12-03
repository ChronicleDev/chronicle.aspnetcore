using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chronicle.AspNetCore.AsyncApi.Generators.Interfaces;
using Chronicle.AspNetCore.AsyncApi.Helpers;
using Chronicle.AspNetCore.AsyncApi.Models.Interfaces;
using Chronicle.AspNetCore.AsyncApi.Serialization.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Chronicle.AspNetCore.UnitTests.AsyncApi.Helpers
{
    [TestFixture]
    public class ResponseHelperTests
    {
        private Mock<IAsyncApiDocument> mockAsyncApiDocument;
        private Mock<IAsyncApiSerializer> mockAsyncApiSerializer;
        private Mock<IDocumentGenerator> mockDocumentGenerator;
        private Mock<HttpResponse> mockHttpResponse;
        private ResponseHelper target;

        [SetUp]
        public void SetUp()
        {
            this.mockAsyncApiDocument = new Mock<IAsyncApiDocument>(MockBehavior.Strict);
            this.mockAsyncApiSerializer = new Mock<IAsyncApiSerializer>(MockBehavior.Strict);
            this.mockDocumentGenerator = new Mock<IDocumentGenerator>(MockBehavior.Strict);
            this.mockHttpResponse = new Mock<HttpResponse>(MockBehavior.Strict);

            this.mockDocumentGenerator
                .Setup(m => m.GetAsyncApi())
                .Returns(this.mockAsyncApiDocument.Object);
            this.mockAsyncApiSerializer
                .Setup(m => m.Serialize(this.mockAsyncApiDocument.Object, It.IsAny<bool>()))
                .Returns(string.Empty);

            this.target = new ResponseHelper(this.mockDocumentGenerator.Object, this.mockAsyncApiSerializer.Object);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task RespondWithDocument_AllPaths_StatusCodeOk(bool isYaml)
        {
            const int statusCode = 200;
            this.mockHttpResponse
                .SetupSet(m => m.StatusCode = statusCode);
            this.mockHttpResponse
                .SetupSet(m => m.ContentType = It.IsAny<string>());
            this.mockHttpResponse
                .Setup(m => m.Body.WriteAsync(It.IsAny<byte[]>(), 0, It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await this.target.RespondWithDocument(this.mockHttpResponse.Object, isYaml);

            this.mockHttpResponse
                .VerifySet(m => m.StatusCode = statusCode);
        }

        [Test]
        public async Task RespondWithDocument_IsYaml_SetsContentType()
        {
            const string contentType = "text/yaml;charset=utf-8";
            this.mockHttpResponse
                .SetupSet(m => m.StatusCode = It.IsAny<int>());
            this.mockHttpResponse
                .SetupSet(m => m.ContentType = contentType);
            this.mockHttpResponse
                .Setup(m => m.Body.WriteAsync(It.IsAny<byte[]>(), 0, It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await this.target.RespondWithDocument(this.mockHttpResponse.Object, true);

            this.mockHttpResponse
                .VerifySet(m => m.ContentType = contentType);
        }

        [Test]
        public async Task RespondWithDocument_IsJson_SetsContentType()
        {
            const string contentType = "application/json;charset=utf-8";
            this.mockHttpResponse
                .SetupSet(m => m.StatusCode = It.IsAny<int>());
            this.mockHttpResponse
                .SetupSet(m => m.ContentType = contentType);
            this.mockHttpResponse
                .Setup(m => m.Body.WriteAsync(It.IsAny<byte[]>(), 0, It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await this.target.RespondWithDocument(this.mockHttpResponse.Object, false);

            this.mockHttpResponse
                .VerifySet(m => m.ContentType = contentType);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task RespondWithDocument_AllPaths_SetsContent(bool isYaml)
        {
            var stringContent = isYaml ? "yaml" : "json";
            var encoding = new UTF8Encoding(false);
            var bytes = encoding.GetBytes(stringContent);
            var stream = new MemoryStream();
            this.mockHttpResponse
                .SetupSet(m => m.StatusCode = It.IsAny<int>());
            this.mockHttpResponse
                .SetupSet(m => m.ContentType = It.IsAny<string>());
            this.mockHttpResponse
                .SetupGet(m => m.Body)
                .Returns(stream);
            this.mockDocumentGenerator
                .Setup(m => m.GetAsyncApi())
                .Returns(this.mockAsyncApiDocument.Object);
            this.mockAsyncApiSerializer
                .Setup(m => m.Serialize(this.mockAsyncApiDocument.Object, isYaml))
                .Returns(stringContent);

            await this.target.RespondWithDocument(this.mockHttpResponse.Object, isYaml);

            Assert.AreEqual(bytes, stream.ToArray());
        }

        [Test]
        public void RespondWithNotFound_WhenCalled_SetsStatusCode()
        {
            const int statusCode = 404;
            this.mockHttpResponse
                .SetupSet(m => m.StatusCode = statusCode);

            this.target.RespondWithNotFound(this.mockHttpResponse.Object);

            this.mockHttpResponse
                .VerifySet(m => m.StatusCode = statusCode);
        }
    }
}