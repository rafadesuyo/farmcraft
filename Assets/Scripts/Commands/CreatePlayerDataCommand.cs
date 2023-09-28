using System.Threading.Tasks;
using UnityEngine.Events;

public class CreatePlayerDataCommand : ICommandAsync<CreatePlayerDataResponse>
{
    private readonly string _playerName;
    private readonly IPlayerService _playerService;

    private UnityEvent _onPlayerCreatedFailed;

    public CreatePlayerDataCommand(string playerName, IPlayerService playerService, UnityEvent onPlayerCreatedFailed)
    {
        _playerName = playerName;
        _playerService = playerService;
        _onPlayerCreatedFailed = onPlayerCreatedFailed;
    }

    public async Task<CreatePlayerDataResponse> Execute()
    {
        var request = new CreatePlayerDataRequest()
        {
            PlayerName = _playerName
        };

        var response = await _playerService.CreatePlayerData(request);

        if (response.HasErrored)
        {
            _onPlayerCreatedFailed.Invoke();
        }

        return response;
    }
}