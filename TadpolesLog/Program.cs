using System;
using Newtonsoft.Json;
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
            var childResult = tadpolesClient.GetChildDetails(validationResult.Result);
            Console.WriteLine(JsonConvert.SerializeObject(childResult));
        }
    }
}
