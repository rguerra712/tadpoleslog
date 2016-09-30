using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TadpolesLog.Clients
{
    public abstract class BaseClient
    {
        protected abstract Uri Domain { get; }
        protected HttpClient Client { get; private set; }

        protected BaseClient(HttpClient client)
        {
            Client = client;
        }

        private Uri BuildUrl(string relativeUrl)
        {
            return new Uri(Domain, relativeUrl);
        }

        protected virtual HttpRequestMessage BuildRequest(string relativeUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BuildUrl(relativeUrl));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
            request.Headers.Add("X-Titanium-Id", "e59d2869-fb2e-4ec5-abfe-ec3a3f006eea");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Connection.Add("Keep-Alive");
            return request;
        }
    }
}