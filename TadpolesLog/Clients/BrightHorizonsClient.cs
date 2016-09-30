using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TadpolesLog.Dtos;

namespace TadpolesLog.Clients
{
    public class BrightHorizonsClient : BaseClient
    {
        protected override Uri Domain => new Uri("https://familyinfocenter.brighthorizons.com");

        public BrightHorizonsClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<LoginResult> Login(string username, string password)
        {
            const string requestUri = @"/mybrightday/login";
            var request = BuildRequest(requestUri);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "response", "jwt"},
            });
            var response = await GetAndValidateResponse(request);
            return new LoginResult
            {
                Token = await response.Content.ReadAsStringAsync(),
            };
        }

    }
}