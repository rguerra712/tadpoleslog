using System.Collections.Generic;

namespace TadpolesLog.Dtos
{
    public class EventsResult
    {
        public string Cursor { get; set; }
        public List<SingleEventResult> Events { get; set; } 
    }
}