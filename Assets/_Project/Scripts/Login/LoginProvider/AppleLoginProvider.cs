#if UNITY_EDITOR || UNITY_IOS
using System;
using System.Collections;
using Apple.GameKit;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace DreamQuiz
{
    public class AppleLoginProvider : ILoginProvider
    {
        private LoginProviderResponseData loginProviderResponseData;
        private Action<LoginProviderResponseData> loginCallback;

        public AppleLoginProvider()
        {
            loginProviderResponseData = new LoginProviderResponseData();
        }

        public void ProcessLogin(Action<LoginProviderResponseData> loginCallback)
        {
            loginProviderResponseData = new LoginProviderResponseData();
            this.loginCallback = loginCallback;
            AppleGameCenterSignIn();
        }

        private async void AppleGameCenterSignIn()
        {
            if (!GKLocalPlayer.Local.IsAuthenticated)
            {
                try
                {
                    await GKLocalPlayer.Authenticate();
                }
                catch (Exception ex)
                {
                    loginProviderResponseData.HasError = true;
                    loginProviderResponseData.ErrorMessage = ex.Message;
                    loginCallback?.Invoke(loginProviderResponseData);
                    return;
                }
            }

            var localPlayer = GKLocalPlayer.Local;
            string userId = localPlayer.TeamPlayerId;
            string email = localPlayer.DisplayName;

            LoginManager.Instance.StartCoroutine(SignInToServer(userId, email));
        }

        private IEnumerator SignInToServer(string thirdPartyId, string email)
        {
            UnityWebRequest request = CreateWebRequest(thirdPartyId, email);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var deserializedData = JsonConvert.DeserializeObject<OcarinaTokenResponse>(request.downloadHandler.text);
                loginProviderResponseData.Token = deserializedData.token;
            }
            else
            {
                loginProviderResponseData.HasError = true;
                loginProviderResponseData.ErrorMessage = request.error;
            }

            loginCallback?.Invoke(loginProviderResponseData);
        }

        private UnityWebRequest CreateWebRequest(string thirdPartyId, string email)
        {
            ThirdPartyPayloadDto payload = new ThirdPartyPayloadDto()
            {
                thirdPartyId = thirdPartyId,
                email = email,
                thirdPartyAccount = 2
            };

            string url = $"{LoginHelper.ThirdPartyAuthUrl}?deviceId={LoginHelper.DeviceId}&deviceType={LoginHelper.DeviceType}";
            string loginFormJson = JsonConvert.SerializeObject(payload);
            byte[] body = new System.Text.UTF8Encoding().GetBytes($"{loginFormJson}");

            UnityWebRequest request = UnityWebRequest.Post(url, loginFormJson);
            request.timeout = LoginHelper.ConnectionTimeout;
            request.uploadHandler = new UploadHandlerRaw(body);
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }
    }
}
#endif