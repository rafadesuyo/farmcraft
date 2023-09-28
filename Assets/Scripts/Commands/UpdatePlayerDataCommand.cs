using System.Threading.Tasks;

public class UpdatePlayerDataCommand : ICommandAsync<UpdatePlayerDataResponse>
{
    private readonly string _playerName;
    private readonly int _houseGold;
    private readonly IPlayerService _playerService;

    public UpdatePlayerDataCommand(string playerName, int houseGold, IPlayerService playerService)
    {
        _playerName = playerName;
        _houseGold = houseGold;
        _playerService = playerService;
    }

    public async Task<UpdatePlayerDataResponse> Execute()
    {
        var request = new UpdatePlayerDataRequest
        {
            PlayerName = _playerName,
            HouseGold = _houseGold
        };

        var response = await _playerService.UpdatePlayerData(request);

        return response;
    }
}