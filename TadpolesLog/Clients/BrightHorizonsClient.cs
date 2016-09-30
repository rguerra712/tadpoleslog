using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TadpolesLog.Dtos;

namespace TadpolesLog.Clients
{
    public class BrightHorizonsClient
    {
        private HttpClient _httpClient;

        public BrightHorizonsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> Login(string username, string password)
        {
            const string requestUri = @"https://familyinfocenter.brighthorizons.com/mybrightday/login";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
            request.Headers.Add("X-Titanium-Id", "e59d2869-fb2e-4ec5-abfe-ec3a3f006eea");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Connection.Add("Keep-Alive");
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "response", "jwt"},
            });
            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            var responseContext = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("Unable to login: {0} {1}", 
                    response.StatusCode, responseContext));
            }
            return new LoginResult
            {
                Cookie = responseContext
            };
        }
    }
}