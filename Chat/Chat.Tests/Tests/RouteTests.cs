﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Tests.Tests
{
    [TestClass]
    public class RouteTests
    {
        [TestMethod]
        public void TestIncomingRoutes()
        {
            IncomingRouteMatchTest("~/", "List", "Room");
            IncomingRouteMatchTest("~/Room", "List", "Room");
            IncomingRouteMatchTest("~/Room/1/Info", "Info", "Room", new { roomId = "1" });
            IncomingRouteMatchTest("~/Room/1", "Join", "Room", new { roomId = "1" });
            IncomingRouteMatchTest("~/Room/1/LoadRecords", "LoadRecords", "Room", new { roomId = "1" });
        }

        private static void IncomingRouteMatchTest(string url, string action, string controller, object routeProperties = null, string httpMethod = "GET")
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act - process the route
            var result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller, action, routeProperties));
        }

        [TestMethod]
        public void TestOutgoingRoutes()
        {
            OutgoingRouteMatchTest("List", "Room", null, "/");
            OutgoingRouteMatchTest("Info", "Room", new RouteValueDictionary
                {
                    {"roomId", "1"}
                }, "/Room/1/Info");
            OutgoingRouteMatchTest("Join", "Room", new RouteValueDictionary
                {
                    {"roomId", "1"}
                }, "/Room/1");
            OutgoingRouteMatchTest("LoadRecords", "Room", new RouteValueDictionary
                {
                    {"roomId", "1"},
                    {"lastRecordCreationDate", "2"}
                }, "/Room/1/LoadRecords?lastRecordCreationDate=2");
        }

        private static void OutgoingRouteMatchTest(string action, string controller,
                                                   RouteValueDictionary routeProperties, string url)
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var context = new RequestContext(CreateHttpContext(), new RouteData());

            // Act - generate the URL
            var result = UrlHelper.GenerateUrl(null, action, controller, routeProperties, routes, context, true);

            // Assert
            Assert.AreEqual(url, result);
        }

        private static HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            // create the mock request
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath)
                       .Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);
            // create the mock response
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>()))
                        .Returns<string>(s => s);
            // create the mock context, using the request and response
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);
            // return the mocked context
            return mockContext.Object;
        }

        private static bool TestIncomingRouteResult(RouteData routeResult,
                                                    string controller, string action, object propertySet = null)
        {
            Func<object, object, bool> compareFunc = (v1, v2) => StringComparer.InvariantCultureIgnoreCase
                                                                               .Compare(v1, v2) == 0;
            var result = compareFunc(routeResult.Values["controller"], controller)
                         && compareFunc(routeResult.Values["action"], action);

            if (propertySet != null)
            {
                var propInfo = propertySet.GetType().GetProperties();
                if (propInfo.Any(propertyInfo =>
                    {
                        var isContains = routeResult.Values.ContainsKey(propertyInfo.Name);
                        var compareResult = compareFunc(routeResult.Values[propertyInfo.Name],
                                                        propertyInfo.GetValue(propertySet, null));
                        return !(isContains && compareResult);
                    }))
                {
                    result = false;
                }
            }
            return result;
        }
    }
}