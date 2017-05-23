using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Narato.Correlations.IntergrationTest.Correlations
{
    public class AddCorrelationIdToResponseMiddlewareTest
    {
        private TestServer SetupServer()
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseCorrelations();
                })
                .ConfigureServices(services =>
                {
                    services.AddCorrelations();
                });

            return new TestServer(builder);
        }

        [Fact]
        public async void TestCorrelationIdShouldRemainSameIfAvailableOnRequest()
        {
            // Arrange
            var server = SetupServer();

            var guid = Guid.NewGuid();

            // Act
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/");
            requestMessage.Headers.Add("Nar-Correlation-Id", guid.ToString());
            var responseMessage = await server.CreateClient().SendAsync(requestMessage);

            // Assert

            IEnumerable<string> correlationId;
            Assert.True(responseMessage.Headers.TryGetValues("Nar-Correlation-Id", out correlationId));
            Assert.Equal(guid.ToString(), correlationId.First());
        }

        [Fact]
        public async void TestCorrelationIdShouldBeCreatedIfNotAvailableOnRequest()
        {
            // Arrange
            var server = SetupServer();

            // Act
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/");
            var responseMessage = await server.CreateClient().SendAsync(requestMessage);

            // Assert

            IEnumerable<string> correlationId;
            Assert.True(responseMessage.Headers.TryGetValues("Nar-Correlation-Id", out correlationId));
            Assert.NotEqual(Guid.Empty.ToString(), correlationId.First());
            Guid noOp;
            Assert.True(Guid.TryParse(correlationId.First(), out noOp));
        }
    }
}
