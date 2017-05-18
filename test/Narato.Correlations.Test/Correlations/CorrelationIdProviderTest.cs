using Microsoft.AspNetCore.Http;
using Moq;
using Narato.Correlations.Correlations;
using System;
using Xunit;

namespace Narato.Correlations.Test.Correlations
{
    public class CorrelationIdProviderTest
    {
        [Fact]
        public void TestExceptionThrownWhenNotInHttpContext()
        {
            var httpContextAccessorMoq = new Mock<IHttpContextAccessor>();

            var provider = new CorrelationIdProvider(httpContextAccessorMoq.Object);

            var ex = Assert.Throws<Exception>(() => provider.GetCorrelationId());
            Assert.Equal("Correlation Id was asked when not in a request context", ex.Message);
        }

        [Fact]
        public void TestGetFromRequestHeaders()
        {
            var guid = Guid.NewGuid();
            var headerDictionary = new HeaderDictionary();

            headerDictionary.Add("Nar-Correlation-Id", guid.ToString());

            var httpRequestMoq = new Mock<HttpRequest>();
            httpRequestMoq.SetupGet(hrm => hrm.Headers).Returns(headerDictionary);

            var httpContextMoq = new Mock<HttpContext>();
            httpContextMoq.SetupGet(hcm => hcm.Request).Returns(httpRequestMoq.Object);

            var httpContextAccessorMoq = new Mock<IHttpContextAccessor>();
            httpContextAccessorMoq.SetupGet(hcam => hcam.HttpContext).Returns(httpContextMoq.Object);

            var provider = new CorrelationIdProvider(httpContextAccessorMoq.Object);

            Assert.Equal(guid, provider.GetCorrelationId());
        }

        [Fact]
        public void TestGetFromResponseHeaders()
        {
            var guid = Guid.NewGuid();
            var requestHeaderDictionary = new HeaderDictionary();
            var responseHeaderDictionary = new HeaderDictionary();
            responseHeaderDictionary.Add("Nar-Correlation-Id", guid.ToString());

            var httpRequestMoq = new Mock<HttpRequest>();
            httpRequestMoq.SetupGet(hrm => hrm.Headers).Returns(requestHeaderDictionary);

            var httpResponseMoq = new Mock<HttpResponse>();
            httpResponseMoq.SetupGet(hrm => hrm.Headers).Returns(responseHeaderDictionary);

            var httpContextMoq = new Mock<HttpContext>();
            httpContextMoq.SetupGet(hcm => hcm.Request).Returns(httpRequestMoq.Object);
            httpContextMoq.SetupGet(hcm => hcm.Response).Returns(httpResponseMoq.Object);

            var httpContextAccessorMoq = new Mock<IHttpContextAccessor>();
            httpContextAccessorMoq.SetupGet(hcam => hcam.HttpContext).Returns(httpContextMoq.Object);

            var provider = new CorrelationIdProvider(httpContextAccessorMoq.Object);

            Assert.Equal(guid, provider.GetCorrelationId());
        }

        [Fact]
        public void TestCreateNewGuid()
        {
            var requestHeaderDictionary = new HeaderDictionary();
            var responseHeaderDictionary = new HeaderDictionary();

            var httpRequestMoq = new Mock<HttpRequest>();
            httpRequestMoq.SetupGet(hrm => hrm.Headers).Returns(requestHeaderDictionary);

            var httpResponseMoq = new Mock<HttpResponse>();
            httpResponseMoq.SetupGet(hrm => hrm.Headers).Returns(responseHeaderDictionary);

            var httpContextMoq = new Mock<HttpContext>();
            httpContextMoq.SetupGet(hcm => hcm.Request).Returns(httpRequestMoq.Object);
            httpContextMoq.SetupGet(hcm => hcm.Response).Returns(httpResponseMoq.Object);

            var httpContextAccessorMoq = new Mock<IHttpContextAccessor>();
            httpContextAccessorMoq.SetupGet(hcam => hcam.HttpContext).Returns(httpContextMoq.Object);

            var provider = new CorrelationIdProvider(httpContextAccessorMoq.Object);

            Assert.NotEqual(Guid.Empty, provider.GetCorrelationId());
        }
    }
}
