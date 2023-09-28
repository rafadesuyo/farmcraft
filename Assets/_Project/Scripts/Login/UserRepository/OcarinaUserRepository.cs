using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace DreamQuiz
{
    public class OcarinaUserRepository : IUserRepository
    {
        private const string baseUrl = "https://dreamquiztest.ocarinastudio.com/api/game/accounts/me";
        private const int connectionTimeout = 30;

        UserRepositoryResponseData responseData;

        public OcarinaUserRepository()
        {
            responseData = new UserRepositoryResponseData();
        }

        public IEnumerator FetchUser(string bearerToken)
        {
            var request = CreateWebRequest(bearerToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var userDto = JsonConvert.DeserializeObject<UserDto>(request.downloadHandler.text);
                responseData.UserModel = ParseUserDtoToModel(userDto);
            }
            else
            {
                responseData.HasError = true;
                responseData.ErrorMessage = request.error;
            }
        }

        public UserRepositoryResponseData GetResponseData()
        {
            return responseData;
        }

        private UnityWebRequest CreateWebRequest(string bearerToken)
        {
            UnityWebRequest request = UnityWebRequest.Get(baseUrl);
            request.timeout = connectionTimeout;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {bearerToken}");

            return request;
        }

        private UserModel ParseUserDtoToModel(UserDto userDto)
        {
            return new UserModel(userDto.UserId);
        }
    }
}