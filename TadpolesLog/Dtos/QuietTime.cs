using Newtonsoft.Json;

namespace TadpolesLog.Dtos
{
    public class QuietTime
    {
        [JsonProperty("start")]
        public string Start => "1:00";

        [JsonProperty("end")]
        public string End => "6:30";
    }
}