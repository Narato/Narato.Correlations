using Moq;
using Narato.Correlations.Correlations.Interfaces;
using Narato.Correlations.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Xunit;

namespace Narato.Correlations.Test.Http
{
    public class HttpClientFactoryTest
    {
        [Fact]
        public void TestCallbackDoesItsJob()
        {
            var correlationIdProviderMock = new Mock<ICorrelationIdProvider>();

            var factory = new HttpClientFactory(correlationIdProviderMock.Object);

            var client = factory.Create((c) =>
            {
                c.DefaultRequestHeaders.Add("meep", "moop");
            });

            Assert.Equal("moop", client.DefaultRequestHeaders.Where(x => x.Key == "meep").First().Value.First());
        }

        [Fact]
        public void TestFactoryCreatesWithCorrelationEnrichingClientHandler()
        {
            var correlationIdProviderMock = new Mock<ICorrelationIdProvider>();

            var factory = new HttpClientFactory(correlationIdProviderMock.Object);

            var client = factory.Create();

            Assert.NotNull(client);
            Assert.IsType(typeof(HttpClient), client);

            var handler = typeof(HttpMessageInvoker).GetField("_handler", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(client);
            Assert.IsType(typeof(CorrelationEnrichingHttpClientHandler), handler);
        }
    }
}
