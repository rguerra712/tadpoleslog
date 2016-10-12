using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TadpolesLog.Extensions;

namespace TadpolesLog.Dtos
{
    public class DailyLogInput
    {
        [JsonProperty("location_key")]
        public string LocationKey { get; set; }
        [JsonProperty("start_timestamp")]
        public long StartTimestamp { get; set; }

        [JsonProperty("subject_type")]
        public string SubjectType => "individual";

        [JsonProperty("checkpoints")]
        public List<object> Checkpoints { get; set; } = new List<object>();
        [JsonProperty("client_update_timestamp")]
        public long ClientUpdateTimestamp { get; set; }
        [JsonProperty("creator_key")]
        public object CreatorKey { get; set; }
        [JsonProperty("entries")]
        public List<EntryInput> Entries { get; set; }
        [JsonProperty("batch")]
        public bool Batch { get; set; }
        [JsonProperty("unviewed_data")]
        public bool UnviewedData { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("action_history")]
        public List<object> ActionHistory => new List<object>();
        [JsonProperty("key")]
        public object Key { get; set; }
        [JsonProperty("published")]
        public bool Published { get; set; }
        [JsonProperty("group_key")]
        public object GroupKey { get; set; }
        [JsonProperty("type_name")]
        public string TypeName => "daily_report";
        [JsonProperty("stage")]
        public string Stage { get; set; }
        [JsonProperty("delivery_state")]
        public string DeliveryState => "unsent";
        [JsonProperty("approved")]
        public bool Approved { get; set; }
        [JsonProperty("subject_key")]
        public string SubjectKey { get; set; }

        public DailyLogInput(Membership membership,
            Dependant dependant,
            params EntryInput[] entries)
        {
            LocationKey = membership.LocationKey;
            var now = SystemTime.Now();
            StartTimestamp = now.ToEpochTime();
            ClientUpdateTimestamp = now.ToEpochTime() * 1000;
            Date = $"{now.Year}-{now.Month}-{now.Day}";
            Stage = dependant.ChildType;
            Entries = entries.ToList();
            SubjectKey = dependant.Key;
        }
    }
}