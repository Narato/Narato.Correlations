using System;

namespace Narato.Correlations.Correlations.Interfaces
{
    public interface ICorrelationIdProvider
    {
        Guid GetCorrelationId();
    }
}
