using Microsoft.AspNetCore.Http;
using Narato.Correlations.Correlations.Interfaces;
using System;

namespace Narato.Correlations.Correlations
{
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        public const string CORRELATION_ID_HEADER_NAME = "Nar-Correlation-Id";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCorrelationId()
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Request == null)
            {
                throw new Exception("Correlation Id was asked when not in a request context");
            }
            Guid guid;
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(CORRELATION_ID_HEADER_NAME))
            {
                var correlationId = _httpContextAccessor.HttpContext.Request.Headers[CORRELATION_ID_HEADER_NAME];
                if (Guid.TryParse(correlationId, out guid))
                    return guid;
            }
            if (_httpContextAccessor.HttpContext.Response.Headers.ContainsKey(CORRELATION_ID_HEADER_NAME))
            {
                var correlationId = _httpContextAccessor.HttpContext.Response.Headers[CORRELATION_ID_HEADER_NAME];
                if (Guid.TryParse(correlationId, out guid))
                    return guid;
            }

            return Guid.NewGuid(); // we don't save it in headers here because that is the responsibility of the middleware
        }
    }
}
