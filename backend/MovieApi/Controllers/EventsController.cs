using Microsoft.AspNetCore.Mvc;
using MovieApi.Models;
using MovieApi.Services;

namespace MovieApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly KinesisService _kinesis;
    private readonly ILogger<EventsController> _logger;

    public EventsController(KinesisService kinesis, ILogger<EventsController> logger)
    {
        _kinesis = kinesis;
        _logger = logger;
    }

    // POST /api/events
    [HttpPost]
    public async Task<IActionResult> TrackEvent([FromBody] MovieEvent movieEvent)
    {
        if (movieEvent == null)
            return BadRequest("Event data is required");

        // Add timestamp if not provided
        movieEvent.Timestamp = DateTime.UtcNow;

        _logger.LogInformation(
            $"Event received: {movieEvent.EventType} for movie {movieEvent.MovieId}"
        );

        // Send to Kinesis
        await _kinesis.SendEventAsync(movieEvent);

        return Ok(new { message = "Event tracked successfully" });
    }
}