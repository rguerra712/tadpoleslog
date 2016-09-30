using System;
using System.Net.Http;
using TadpolesLog.Clients;
using TadpolesLog.Inputs;

namespace TadpolesLog
{
    class Program
    {
        static void Main(string[] args)
        {
            var loginCredentials = LoginCredentials.ExtractFrom(args);
            var client = new HttpClient();
            var loginClient = new BrightHorizonsClient(client);
            var loginResult = loginClient.Login(loginCredentials.UserName, loginCredentials.Password);
            Console.WriteLine(loginResult.Result.Token);
        }
    }
}
