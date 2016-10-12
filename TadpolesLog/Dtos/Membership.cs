using System.Collections.Generic;
using Newtonsoft.Json;

namespace TadpolesLog.Dtos
{
    public class Membership
    {
        [JsonProperty("location_key")]
        public string LocationKey { get; set; }
        [JsonProperty("dependants")]
        public IEnumerable<Dependant> Dependents { get; set; }
        [JsonProperty("person")]
        public string Person { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}