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

    public KinesisService(ILogger<KinesisService> logger, IConfiguration config)
    {
        _logger = logger;

        // Use AWS credentials from environment or appsettings
        _client = new AmazonKinesisClient(
            config["AWS:AccessKey"],
            config["AWS:SecretKey"],
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
            _logger.LogInformation($"Event sent to Kinesis. ShardId: {response.ShardId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send event to Kinesis: {ex.Message}");
        }
    }
}