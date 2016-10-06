using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TadpolesLog.Clients
{
    public abstract class BaseClient
    {
        protected abstract Uri Domain { get; }
        private readonly IDictionary<string, string> _cookies = new Dictionary<string, string>();

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

        protected virtual void AddCookie(string key, string value)
        {
            _cookies.Add(key, value);
        }

        protected async virtual Task<HttpResponseMessage> GetAndValidateResponse(HttpRequestMessage request)
        {
            var client = BuildClient();
            _cookies.Clear();
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to login: {response.StatusCode} {responseContent}");
            }
            return response;
        }

        private HttpClient BuildClient()
        {
            var handler = new HttpClientHandler {CookieContainer = new CookieContainer()};
            foreach (var cookie in _cookies)
            {
                handler.CookieContainer.Add(Domain, new Cookie(cookie.Key, cookie.Value));
            }
            var client = new HttpClient(handler);
            return client;
        }
    }
}