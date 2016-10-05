using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TadpolesLog.Clients
{
    public abstract class BaseClient
    {
        protected abstract Uri Domain { get; }
        protected virtual HttpClient Client { get; }
        protected CookieContainer Cookies { get; }

        protected BaseClient()
        {
            var handler = new HttpClientHandler();
            Cookies = new CookieContainer();
            handler.CookieContainer = Cookies;
            Client = new HttpClient(handler);
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

        protected async virtual Task<HttpResponseMessage> GetAndValidateResponse(HttpRequestMessage request)
        {
            var response = await Client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to login: {response.StatusCode} {responseContent}");
            }
            return response;
        }
    }
}