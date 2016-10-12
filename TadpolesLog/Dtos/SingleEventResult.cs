using System.Collections.Generic;
using Newtonsoft.Json;

namespace TadpolesLog.Dtos
{
    public class SingleEventResult
    {
        public string Action { get; set; }
        [JsonProperty("create_time")]
        public long CreateTime { get; set; }
        [JsonProperty("event_time")]
        public long EventTime { get; set; }
        [JsonProperty("event_date")]
        public string EventDate { get; set; }
        [JsonProperty("entries")]
        public List<EntryInput> Entries { get; set; }
    }
}