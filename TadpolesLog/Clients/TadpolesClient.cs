using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TadpolesLog.Dtos;

namespace TadpolesLog.Clients
{
    public class TadpolesClient : BaseClient
    {
        protected override Uri Domain => new Uri("https://www.tadpoles.com");
        public TadpolesClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ValidationResult> ValidateToken(LoginResult loginResult)
        {
            var request = BuildRequest("/auth/jwt/validate");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "token", loginResult.Token},
            });
            var response = await GetAndValidateResponse(request);
            IEnumerable<string> cookieValues;
            var doesCookieExist = response.Headers.TryGetValues("Set-Cookie", out cookieValues);
            var cookies = cookieValues as string[] ?? cookieValues.ToArray();
            if (!doesCookieExist || !cookies.Any())
            {
                throw new Exception("Unexpected result from API, cookie not preset");
            }
            if (cookies.Count() != 1)
            {
                throw new Exception("Unable to process more than one cookie value");
            }
            return new ValidationResult
            {
                Cookie = cookies.Single()
            };
        }
    }
}