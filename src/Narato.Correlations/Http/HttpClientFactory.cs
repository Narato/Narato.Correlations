using Narato.Correlations.Correlations.Interfaces;
using Narato.Correlations.Http.Interfaces;
using System;
using System.Net.Http;

namespace Narato.Correlations.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public HttpClientFactory(ICorrelationIdProvider correlationIdProvider)
        {
            _correlationIdProvider = correlationIdProvider;
        }

        public HttpClient Create()
        {
            return new HttpClient(new CorrelationEnrichingHttpClientHandler(_correlationIdProvider));
        }

        public HttpClient Create(Action<HttpClient> callback)
        {
            var httpClient = Create();
            callback(httpClient);
            return httpClient;
        }
    }
}
