using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TadpolesLog.Dtos;
using TadpolesLog.Extensions;

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
            AddCookie(validation);
            var response = await GetAndValidateResponse(request);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ChildDetailsResult>(content);
        }

        public async Task<EventsResult> GetEventsSince(DateTime since, ValidationResult validation)
        {
            var request = BuildRequest("/remote/v1/events?num_events=78&state=client&direction=newer&latest_create_time=" + since.ToEpochTime());
            AddCookie(validation);
            var response = await GetAndValidateResponse(request);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EventsResult>(content);
        }

        public void LogReport(ValidationResult validationResult,
            Membership membership,
            Dependant dependant,
            params EntryInput[] entries)
        {
            var request = BuildRequest("/remote/v1/daily_report/parent");
            AddCookie(validationResult);
            var log = new DailyLogInput(membership, dependant, entries);
            var serialized = JsonConvert.SerializeObject(log);
            var encoded = $"daily_report={WebUtility.UrlEncode(serialized)}";
            request.Content = new StringContent(encoded, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = GetAndValidateResponse(request).Result;
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
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