using System;
using System.Linq;
using Args;

namespace TadpolesLog.Inputs
{
    public class LoginCredentials
    {
        public static LoginCredentials ExtractFrom(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            if (!args.Any()) throw new ArgumentException("expected command line arguments");

            var loginCredentials = Configuration.Configure<LoginCredentials>().CreateAndBind(args);
            if (string.IsNullOrWhiteSpace(loginCredentials.UserName))
            {
                throw new ArgumentException("username is a required arg");
            }
            if (string.IsNullOrWhiteSpace(loginCredentials.Password))
            {
                throw new ArgumentException("password is a required arg");
            }
            return loginCredentials;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}