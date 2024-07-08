namespace SignalRServer.Models
{
    public class Alert
    {
        public string ServerIdentifier { get; set; } = string.Empty;
        public string AlertType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
