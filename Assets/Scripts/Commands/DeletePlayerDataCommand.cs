using System.Threading.Tasks;

public class DeletePlayerDataCommand : ICommandAsync<DeletePlayerDataResponse>
{
    private readonly string _playerName;

    private readonly IPlayerService _playerService;

    public DeletePlayerDataCommand(string playerName, IPlayerService playerService)
    {
        _playerName = playerName;
        _playerService = playerService;
    }

    public async Task<DeletePlayerDataResponse> Execute()
    {
        var request = new DeletePlayerDataRequest()
        {
            PlayerName = _playerName
        };

        var response = await _playerService.DeletePlayerData(request);

        return response;
    }
}