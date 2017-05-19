using Moq;
using Narato.Correlations.Correlations.Interfaces;
using Narato.Correlations.Http;
using System;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Narato.Correlations.Test.Http
{
    public class CorrelationEnrichingHttpClientHandlerTest
    {
        [Fact]
        public async void TestFactoryCreatesWithCorrelationEnrichingClientHandler()
        {
            var guid = Guid.NewGuid();
            var correlationIdProviderMock = new Mock<ICorrelationIdProvider>();
            correlationIdProviderMock.Setup(cipm => cipm.GetCorrelationId()).Returns(guid);

            var client = new HttpClient(new CorrelationEnrichingHttpClientHandler(correlationIdProviderMock.Object));

            var response = await client.GetAsync("http://google.com");
            Assert.Equal(guid.ToString(), response.RequestMessage.Headers.Where(kv => kv.Key == "Nar-Correlation-Id").First().Value.First());
        }
    }
}
