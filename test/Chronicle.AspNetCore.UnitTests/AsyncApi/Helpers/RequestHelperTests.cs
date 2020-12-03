using System;
using System.Collections.Generic;
using System.Text;
using Chronicle.AspNetCore.AsyncApi.Helpers;
using Chronicle.AspNetCore.AsyncApi.Options.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace Chronicle.AspNetCore.UnitTests.AsyncApi.Helpers
{
    [TestFixture]
    public class RequestHelperTests
    {
        private Mock<IAsyncApiOptions> mockAsyncApiOptions;
        private Mock<HttpRequest> mockHttpRequest;
        private RequestHelper target;

        [SetUp]
        public void SetUp()
        {
            this.mockAsyncApiOptions = new Mock<IAsyncApiOptions>(MockBehavior.Strict);
            this.mockHttpRequest = new Mock<HttpRequest>(MockBehavior.Strict);

            this.mockAsyncApiOptions
                .Setup(m => m.RouteTemplate)
                .Returns("/valid-route");

            this.target = new RequestHelper(this.mockAsyncApiOptions.Object);
        }

        [Test]
        [TestCase("CONNECT")]
        [TestCase("DELETE")]
        [TestCase("HEAD")]
        [TestCase("OPTIONS")]
        [TestCase("PATCH")]
        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("TRACE")]
        public void RequestingAsyncApiDocument_InvalidMethods_ReturnsFalse(string method)
        {
            this.mockHttpRequest
                .Setup(m => m.Method)
                .Returns(method);

            var result = this.target.RequestingAsyncApiDocument(this.mockHttpRequest.Object);

            Assert.IsFalse(result);
        }

        [Test]
        public void RequestingAsyncApiDocument_InvalidRoute_ReturnsFalse()
        {
            this.mockHttpRequest
                .Setup(m => m.Path)
                .Returns(new PathString("/invalid-route"));
            this.mockHttpRequest
                .Setup(m => m.Method)
                .Returns(HttpMethods.Get);

            var result = this.target.RequestingAsyncApiDocument(this.mockHttpRequest.Object);

            Assert.IsFalse(result);
        }

        [Test]
        public void RequestingAsyncApiDocument_ValidRoute_ReturnsTrue()
        {
            this.mockHttpRequest
                .Setup(m => m.Path)
                .Returns(new PathString("/valid-route"));
            this.mockHttpRequest
                .Setup(m => m.Method)
                .Returns(HttpMethods.Get);

            var result = this.target.RequestingAsyncApiDocument(this.mockHttpRequest.Object);

            Assert.IsTrue(result);
        }

        [Test]
        public void RequestingDocumentAsYaml_RequestingYaml_ReturnsTrue()
        {
            this.mockHttpRequest
                .Setup(m => m.Path)
                .Returns(new PathString("/some-path.yaml"));

            var result = this.target.RequestingDocumentAsYaml(this.mockHttpRequest.Object);

            Assert.IsTrue(result);
        }

        [Test]
        public void RequestingDocumentAsYaml_RequestingJson_ReturnsFalse()
        {
            this.mockHttpRequest
                .Setup(m => m.Path)
                .Returns(new PathString("/some-path.json"));

            var result = this.target.RequestingDocumentAsYaml(this.mockHttpRequest.Object);

            Assert.IsFalse(result);
        }
    }
}
