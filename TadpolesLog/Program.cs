using System;
using System.Collections.Generic;
using System.Linq;
using TadpolesLog.Clients;
using TadpolesLog.Dtos;
using TadpolesLog.Extensions;
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
            var finalValidation = tadpolesClient.ValidateLogin(validationResult.Result).Result;
            var childResult = tadpolesClient.GetChildDetails(finalValidation);
            var membership = childResult.Result.Memberships.First();
            var dependent = membership.Dependents.First();
            var entryInputs = new[]
            {
                new EntryInput(dependent, "nap")
                {
                    SleepType = "overnight",
                    EndTime = SystemTime.Now().ToEpochTime()
                },
                new EntryInput(dependent, "bathroom")
                {
                    BathroomType = "diaper",
                    ActionTime = SystemTime.Now().ToEpochTime(),
                    Events = new List<string> { "wet" },
                    V = 2,
                    Id = "bkurybn4rzwbtudb6xckdq",
                },
                new EntryInput(dependent, "food")
                {
                    MealType = "food",
                    ActionTime = SystemTime.Now().ToEpochTime(),
                    V = 2,
                    Amount = "all",
                    Foods = "5 oz milk",
                },
            };
            var eventTime = SystemTime.Now().AddDays(-1);
            var events = tadpolesClient.GetEventsSince(eventTime, finalValidation).Result;
            tadpolesClient.LogReport(finalValidation, membership, dependent, entryInputs);
            events = tadpolesClient.GetEventsSince(eventTime, finalValidation).Result;
        }
    }
}
