#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace DreamQuiz
{
    public class GoogleLoginProvider : ILoginProvider
    {
        private LoginProviderResponseData loginProviderResponseData;
        private Action<LoginProviderResponseData> loginCallback;

        public GoogleLoginProvider()
        {
            loginProviderResponseData = new LoginProviderResponseData();
#if UNITY_ANDROID
            PlayGamesPlatform.Activate();
#endif
        }

        public void ProcessLogin(Action<LoginProviderResponseData> loginCallback)
        {
            this.loginCallback = loginCallback;
            loginProviderResponseData = new LoginProviderResponseData();

#if UNITY_ANDROID
            GooglePlayGamesSignIn();
#endif
        }

#if UNITY_ANDROID
        private void GooglePlayGamesSignIn()
        {
            PlayGamesPlatform.Instance.ManuallyAuthenticate(SignInCallback);
        }

        private void SignInCallback(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                string userId = Social.localUser.id;
                string email = Social.localUser.userName;

                Debug.Log("Login with Google Play Games successful.");
                Debug.Log($"User ID:{userId}; Email:{email}");

                LoginManager.Instance.StartCoroutine(SignInToServer(userId, email));
                return;
            }

            loginProviderResponseData.HasError = true;
            loginProviderResponseData.ErrorMessage = $"Failed to login using the Google Play Services. Status: {status}";

            loginCallback?.Invoke(loginProviderResponseData);
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
                thirdPartyAccount = 1
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
#endif
    }
}