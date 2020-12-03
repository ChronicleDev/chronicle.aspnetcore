using System;
using System.Collections.Generic;
using System.Text;
using Chronicle.AspNetCore.AsyncApi.Options;
using NUnit.Framework;

namespace Chronicle.AspNetCore.UnitTests.AsyncApi.Options
{
    [TestFixture]
    public class AsyncApiOptionsTests
    {
        [Test]
        public void RouteTemplate_WhenNotSet_ReturnsDefaultValue()
        {
            const string defaultRouteTemplate = "async-api/async-api.{json|yaml}";

            var target = new AsyncApiOptions();

            Assert.AreEqual(defaultRouteTemplate, target.RouteTemplate);
        }

        [Test]
        public void RouteTemplate_WhenSet_ReturnsValue()
        {
            const string valueRouteTemplate = "some-route";

            var target = new AsyncApiOptions
            {
                RouteTemplate = valueRouteTemplate
            };

            Assert.AreEqual(valueRouteTemplate, target.RouteTemplate);
        }
    }
}
