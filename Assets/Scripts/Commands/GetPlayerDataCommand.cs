using System.Threading.Tasks;

public class GetPlayerPlayerDataCommand : ICommandAsync<GetPlayerDataResponse>
{
    private readonly string _playerName;
    private readonly IPlayerService _playerService;

    public GetPlayerPlayerDataCommand(string playerName, IPlayerService playerService)
    {
        _playerName = playerName;
        _playerService = playerService;
    }

    public async Task<GetPlayerDataResponse> Execute()
    {
        var request = new GetPlayerDataRequest()
        {
            PlayerName = _playerName
        };

        var response = await _playerService.GetPlayerData(request);

        return response;
    }
}