using Microsoft.AspNetCore.Http;
using Narato.Correlations.Correlations.Interfaces;
using System;
using System.Threading.Tasks;

namespace Narato.Correlations.Correlations
{
    public class AddCorrelationIdToResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public AddCorrelationIdToResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context, ICorrelationIdProvider correlationIdProvider)
        {
            if (correlationIdProvider == null)
            {
                throw new Exception("No correlationId provider found. Did you setup Narato.Correlations correctly?");
            }

            var correlationId = correlationIdProvider.GetCorrelationId().ToString();

            if (!context.Response.Headers.ContainsKey(CorrelationIdProvider.CORRELATION_ID_HEADER_NAME))
            {
                context
                .Response
                .Headers
                .Add(CorrelationIdProvider.CORRELATION_ID_HEADER_NAME, correlationId);
            }

            return _next(context);
        }
    }
}
