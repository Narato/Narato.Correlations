using System;
using System.Net.Http;

namespace Narato.Correlations.Http.Interfaces
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
        HttpClient Create(Action<HttpClient> callback);
    }
}
