using Newtonsoft.Json;
using System;

public class GetPlayerDataResponse : BaseResponse
{
    public string ObjectId { get; set; } = string.Empty;

    public string PlayerName { get; set; } = string.Empty;

    [JsonProperty("playerGold")]
    public int HouseGold { get; set; } = 0;

    public DateTime CreatedOn { get; set; } = DateTime.MinValue;

    public DateTime LastModified { get; set; } = DateTime.MinValue;
}