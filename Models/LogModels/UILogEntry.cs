namespace MessageBusExample.Models.LogModels
{
    public class UILogEntry
    {
        public DateTimeOffset Timestamp { get; set; }
        public string Level { get; set; } = "";
        public string Message { get; set; } = "";
        public string Source { get; set; } = "";
    }
}
