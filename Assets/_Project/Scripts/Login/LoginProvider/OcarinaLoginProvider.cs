using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace DreamQuiz
{
    public class OcarinaLoginProvider : ILoginProvider
    {
        [Serializable]
        public class LoginForm
        {
            public string userID;
            public string password;
        }

        private LoginProviderResponseData loginProviderResponseData;

        public OcarinaLoginProvider()
        {
            loginProviderResponseData = new LoginProviderResponseData();
        }

        public void ProcessLogin(Action<LoginProviderResponseData> loginCallback)
        {
            LoginManager.Instance.StartCoroutine(SignIn(loginCallback));
        }

        private IEnumerator SignIn(Action<LoginProviderResponseData> loginCallback)
        {
            loginProviderResponseData = new LoginProviderResponseData();

            string userId = PlayerPrefs.GetString(LoginHelper.FormUserIdKey);
            string password = PlayerPrefs.GetString(LoginHelper.FormUserPasswordKey);

            UnityWebRequest request = CreateWebRequest(userId, password);

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

        private UnityWebRequest CreateWebRequest(string userID, string password)
        {
            LoginForm loginForm = new LoginForm()
            {
                userID = userID,
                password = password
            };

            string url = $"{LoginHelper.OcarinaAuthUrl}?deviceId={LoginHelper.DeviceId}&deviceType={LoginHelper.DeviceType}";
            string loginFormJson = JsonConvert.SerializeObject(loginForm);
            byte[] body = new System.Text.UTF8Encoding().GetBytes($"{loginFormJson}");

            UnityWebRequest request = UnityWebRequest.Post(url, loginFormJson);
            request.timeout = LoginHelper.ConnectionTimeout;
            request.uploadHandler = new UploadHandlerRaw(body);
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }
    }
}