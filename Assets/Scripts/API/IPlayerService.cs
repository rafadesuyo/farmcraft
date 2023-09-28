using System.Threading.Tasks;

public interface IPlayerService
{
    Task<CreatePlayerDataResponse> CreatePlayerData(CreatePlayerDataRequest request);

    Task<GetPlayerDataResponse> GetPlayerData(GetPlayerDataRequest request);

    Task<UpdatePlayerDataResponse> UpdatePlayerData(UpdatePlayerDataRequest request);

    Task<DeletePlayerDataResponse> DeletePlayerData(DeletePlayerDataRequest request);
}