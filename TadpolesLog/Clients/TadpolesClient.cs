using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TadpolesLog.Dtos;

namespace TadpolesLog.Clients
{
    public class TadpolesClient : BaseClient
    {
        protected override Uri Domain => new Uri("https://www.tadpoles.com");

        public async Task<ValidationResult> ValidateToken(LoginResult loginResult)
        {
            var request = BuildRequest("/auth/jwt/validate");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "token", loginResult.Token},
            });
            var response = await GetAndValidateResponse(request);
            var cookie = ExtractCookieFrom(response);
            return new ValidationResult
            {
                Cookie = cookie
            };
        }

        public async Task<ValidationResult> ValidateLogin(ValidationResult jwtValidationResult)
        {
            var request = BuildRequest("/remote/v1/athome/admit");
            AddCookie(jwtValidationResult);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "available_memory", "9915384" },
                { "uses_dst",  "true" },
                { "utc_offset", "-06 % 3A00" },
                { "tz", "America % 2FChicago" },
                {  "logged_in", "true" },
                { "locale", "en - US" },
                { "state", "client" },
                { "v", "2" },
                { "battery_level", "-1"},
                { "app_version", "6.5.19" },
                { "platform_version", "6.0.1" },
                { "ostype", "32bit" },
                { "os_name", "android" },
            });
            var response = await GetAndValidateResponse(request);
            var cookie= ExtractCookieFrom(response);
            return new ValidationResult
            {
                Cookie = cookie
            };
        }

        public async Task<ChildDetailsResult> GetChildDetails(ValidationResult validation)
        {
            var request = BuildRequest("/remote/v1/parameters?include_all_kids=true&include_guardians=True");
            request.Method = HttpMethod.Get;
            AddCookie(validation);
            var response = await GetAndValidateResponse(request);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ChildDetailsResult>(content);
        }

        public async void LogReport(string report)
        {
            throw new NotImplementedException();
        }

        private void AddCookie(ValidationResult validationResult)
        {
            AddCookie(validationResult.Cookie.Key, validationResult.Cookie.Value);
        }

        private static KeyValuePair<string, string> ExtractCookieFrom(HttpResponseMessage response)
        {
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
            var cookie = cookies.Single();
            var semicolonIndex = cookie.IndexOf(";", StringComparison.Ordinal);
            if (semicolonIndex > 0)
            {
                cookie = cookie.Substring(0, semicolonIndex);
            }
            var equalsIndex = cookie.IndexOf("=", StringComparison.Ordinal);
            if (equalsIndex <= 0)
            {
                throw new Exception("Unable to parse key and value from cookie, no = provided: " + cookie);
            }
            var key = cookie.Substring(0, equalsIndex);
            var value = cookie.Substring(equalsIndex + 1);
            return new KeyValuePair<string, string>(key, value);
        }

    }
}