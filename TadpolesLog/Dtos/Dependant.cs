using Newtonsoft.Json;

namespace TadpolesLog.Dtos
{
    public class Dependant
    {
        [JsonProperty("child_type")]
        public string ChildType { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}