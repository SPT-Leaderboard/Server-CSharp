using System.Text.Json.Serialization;
using JetBrains.Annotations;
using SPTarkov.Server.Core.Models.Common;

namespace SPTLeaderboard.Models.Responses;

public record CheckInboxResponseData
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("sessionId")]
    public MongoId SessionId { get; set; }
    
    [JsonPropertyName("messageText")]
    public string? Message { get; set; }
    
    [JsonPropertyName("rewardTpls")]
    public List<MongoId>? Items { get; set; }
}