using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class PlayerService : IPlayerService
{
    private const string _baseUrl = "https://dreamquiztest.ocarinastudio.com/api/player/gold";

    public async Task<CreatePlayerDataResponse> CreatePlayerData(CreatePlayerDataRequest request)
    {
        var response = new CreatePlayerDataResponse();

        try
        {
            HttpResponseMessage httpResponse = await SendRequest(request, HttpMethod.Post, "create");

            if (!httpResponse.IsSuccessStatusCode)
            {
                response.HasErrored = true;
            }
        }
        catch (Exception)
        {
            response.HasErrored = true;
        }

        return response;
    }

    public async Task<GetPlayerDataResponse> GetPlayerData(GetPlayerDataRequest request)
    {
        var response = new GetPlayerDataResponse();

        try
        {
            HttpResponseMessage httpResponse = await SendRequest(request, HttpMethod.Get, $"name/{request.PlayerName}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<GetPlayerDataResponse>(content) ?? new GetPlayerDataResponse() { HasErrored = true };
            }
            else
            {
                response.HasErrored = true;
            }
        }
        catch (Exception)
        {
            response.HasErrored = true;
        }

        return response;
    }

    public async Task<UpdatePlayerDataResponse> UpdatePlayerData(UpdatePlayerDataRequest request)
    {
        var response = new UpdatePlayerDataResponse();

        try
        {
            HttpResponseMessage httpResponse = await SendRequest(request, HttpMethod.Put, "update");

            if (!httpResponse.IsSuccessStatusCode)
            {
                response.HasErrored = true;
            }
        }
        catch (Exception)
        {
            response.HasErrored = true;
        }

        return response;
    }

    public async Task<DeletePlayerDataResponse> DeletePlayerData(DeletePlayerDataRequest request)
    {
        var response = new DeletePlayerDataResponse();

        try
        {
            HttpResponseMessage httpResponse = await SendRequest(request, HttpMethod.Delete, $"delete/{request.PlayerName}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                response.HasErrored = true;
            }
        }
        catch (Exception)
        {
            response.HasErrored = true;
        }

        return response;
    }

    private static HttpClient GetHttpClient()
    {
        HttpClient client = new()
        {
            BaseAddress = new Uri(_baseUrl)
        };

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return client;
    }

    private static async Task<HttpResponseMessage> SendRequest<T>(T request, HttpMethod method, string path)
    {
        HttpClient httpClient = GetHttpClient();

        var uri = new Uri($"{_baseUrl}/{path}");

        var httpRequest = new HttpRequestMessage(method, uri);

        if (method != HttpMethod.Get)
        {
            httpRequest.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }

        var httpResponse = await httpClient.SendAsync(httpRequest);

        httpClient.Dispose();

        return httpResponse;
    }
}