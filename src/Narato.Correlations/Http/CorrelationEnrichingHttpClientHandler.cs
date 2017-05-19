using Narato.Correlations.Correlations;
using Narato.Correlations.Correlations.Interfaces;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Narato.Correlations.Http
{
    public class CorrelationEnrichingHttpClientHandler : HttpClientHandler
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public CorrelationEnrichingHttpClientHandler(ICorrelationIdProvider correlationIdProvider)
        {
            _correlationIdProvider = correlationIdProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (! request.Headers.Contains(CorrelationIdProvider.CORRELATION_ID_HEADER_NAME))
                request.Headers.Add(CorrelationIdProvider.CORRELATION_ID_HEADER_NAME, _correlationIdProvider.GetCorrelationId().ToString());

            return base.SendAsync(request, cancellationToken);
        }
    }
}
