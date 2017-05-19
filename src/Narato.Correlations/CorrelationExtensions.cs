using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Narato.Correlations.Correlations;
using Narato.Correlations.Correlations.Interfaces;
using System;

namespace Narato.Correlations
{
    public static class CorrelationExtensions
    {
        public static IServiceCollection AddCorrelations(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // TODO: I haven't seen any library that does this, even when they rely on it... Should we
            // take this out, and document that the user has to supply a IHttpContextAccessor?
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ICorrelationIdProvider, CorrelationIdProvider>();

            return services;
        }

        public static IApplicationBuilder UseCorrelations(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<AddCorrelationIdToResponseMiddleware>();
        }
    }
}
