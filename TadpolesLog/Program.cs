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
            var loginClient = new BrightHorizonsClient();
            var loginResult = loginClient.Login(loginCredentials.UserName, loginCredentials.Password);
            var tadpolesClient = new TadpolesClient();
            var validationResult = tadpolesClient.ValidateToken(loginResult.Result);
            validationResult = tadpolesClient.ValidateLogin(validationResult.Result);
            Console.WriteLine(validationResult.Result.Cookie);
        }
    }
}
