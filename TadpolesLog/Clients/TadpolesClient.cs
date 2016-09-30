using System;
using System.Net.Http;
using TadpolesLog.Dtos;

namespace TadpolesLog.Clients
{
    public class TadpolesClient : BaseClient
    {
        protected override Uri Domain => new Uri("https://www.tadpoles.com");
        public TadpolesClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public void ValidateToken(LoginResult loginResult)
        {
            throw new NotImplementedException();
        }
    }
}