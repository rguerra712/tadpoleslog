using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TadpolesLog.Extensions;

namespace TadpolesLog.Dtos
{
    public class EntryInput
    {
        [JsonProperty("type_name")]
        public string TypeName { get; set; }

        // TODO, different DTO's
        [JsonProperty("bathroom_type")]
        public string BathroomType { get; set; }
        [JsonProperty("action_time")]
        public long ActionTime { get; set; }
        [JsonProperty("events")]
        public List<string> Events { get; set; }

        [JsonProperty("meal_type")]
        public string MealType { get; set; }
        [JsonProperty("meal_name")]
        public string MealName { get; set; }
        [JsonProperty("foods")]
        public string Foods { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("sleep_type")]
        public string SleepType { get; set; }
        [JsonProperty("start_time")]
        public object StartTime { get; set; }
        [JsonProperty("end_time")]
        public long EndTime { get; set; }
        [JsonProperty("checks")]
        public object Checks { get; set; }

        [JsonProperty("note")]
        public object Note { get; set; }
        [JsonProperty("reminder_period")]
        public int ReminderPeriod { get; set; } = 120;
        [JsonProperty("v")]
        public int V { get; set; } = 3;
        [JsonProperty("stage")]
        public string Stage { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("capture_time")]
        public long CaptureTime { get; set; }
        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("client_update_timestamp")]
        public long ClientUpdateTimestamp { get; set; }
        [JsonProperty("parent")]
        public bool Parent { get; set; }

        public EntryInput(Dependant dependant, string type)
        {
            var now = SystemTime.Now().ToEpochTime();
            TypeName = type;
            CaptureTime = now;
            Owner = dependant?.Key;
            ClientUpdateTimestamp = now * 1000;
            Parent = true;
            Stage = dependant?.ChildType;
        }
    }
}