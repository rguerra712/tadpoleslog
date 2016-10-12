using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TadpolesLog.Dtos
{
    public class PushNotificationRegistrationInput
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }
        [JsonProperty("badge")]
        public int Badge { get; set; }
        [JsonProperty("quiettime")]
        public QuietTime QuietTime { get; set; }
        [JsonProperty("tz")]
        public string Tz { get; set; } = "American/Chicago"; // TODO, setting
        [JsonProperty("messageservice")]
        public string Messageservice => "gcm";
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
        [JsonProperty("device_token")]
        public string DeviceToken { get; set; }
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
        [JsonProperty("app")]
        public string App => "parent:bh";
        [JsonProperty("is_provider")]
        public bool IsProvider { get; set; }

        public PushNotificationRegistrationInput(Membership membership)
        {
            Alias = membership.Email;
            // TODO your device credentials
            DeviceToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            DeviceId = Guid.NewGuid().ToString();
        }
    }
}