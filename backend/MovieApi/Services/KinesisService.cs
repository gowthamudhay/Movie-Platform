using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using System.Text;
using System.Text.Json;

namespace MovieApi.Services;

public class KinesisService
{
    private readonly AmazonKinesisClient _client;
    private readonly string _streamName = "movie-events-stream";
    private readonly ILogger<KinesisService> _logger;

    public KinesisService(ILogger<KinesisService> logger)
    {
        _logger = logger;
        _client = new AmazonKinesisClient(
            Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
            Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"),
            Amazon.RegionEndpoint.USEast1
        );
    }

    public async Task SendEventAsync(object movieEvent)
    {
        try
        {
            var json = JsonSerializer.Serialize(movieEvent);
            var data = Encoding.UTF8.GetBytes(json);

            var request = new PutRecordRequest
            {
                StreamName = _streamName,
                Data = new MemoryStream(data),
                PartitionKey = Guid.NewGuid().ToString()
            };

            var response = await _client.PutRecordAsync(request);
            _logger.LogInformation(
                $"Event sent to Kinesis. ShardId: {response.ShardId}"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send to Kinesis: {ex.Message}");
        }
    }
}