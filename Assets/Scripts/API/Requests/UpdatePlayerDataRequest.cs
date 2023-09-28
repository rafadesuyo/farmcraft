using Newtonsoft.Json;

public class UpdatePlayerDataRequest
{
    public string PlayerName { get; set; } = string.Empty;

    [JsonProperty("playerGold")]
    public int HouseGold { get; set; } = 0;
}