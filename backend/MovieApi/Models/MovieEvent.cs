namespace MovieApi.Models;

public class MovieEvent
{
    public string EventType { get; set; } = string.Empty;
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Genre { get; set; } = string.Empty;
}