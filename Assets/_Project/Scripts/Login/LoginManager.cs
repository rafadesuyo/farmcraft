using System.Collections;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;

namespace DreamQuiz
{
    public class LoginManager : PersistentSingleton<LoginManager>
    {
        public enum LoginState { Ready, Busy, SignIn, UserFetch }

        private ILoginProvider loginProvider;
        private IUserRepository userRepository;

        [SerializeField] string onLoginScene = "02 - HomeMenu";
        [SerializeField] string onLogoutScene = "LoginScene";

        public bool IsLoggedIn { get; private set; }
        public LoginState CurrentLoginState { get; private set; }

        public UserModel UserModel { get; private set; }

        public event Action<LoginState> OnLoginStateChange;
        public event Action<string> OnLoginError;

        private async void Start()
        {
            SetLoginState(LoginState.Busy);

            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.SignedOut += AuthenticationService_SignedOut;

            if (StoredJWT.HasValidToken())
            {
                FetchUserFromRepository();
                return;
            }

            SetLoginState(LoginState.Ready);
        }

        public void Login()
        {
            if (CurrentLoginState != LoginState.Ready)
            {
                return;
            }

            SetLoginState(LoginState.SignIn);
            IsLoggedIn = false;

            if (loginProvider == null)
            {
                loginProvider = LoginHelper.GetLoginProvider();
            }

            loginProvider.ProcessLogin(OnLoginCallback);
        }

        private void OnLoginCallback(LoginProviderResponseData responseData)
        {
            if (responseData.HasError)
            {
                Debug.LogError($"[UserManager] Error trying to login: {responseData.ErrorMessage}");
                OnLoginError?.Invoke(responseData.ErrorMessage);
                SetLoginState(LoginState.Ready);
                return;
            }

            StoredJWT.SetToken(responseData.Token);

            FetchUserFromRepository();
        }

        private void FetchUserFromRepository()
        {
            if (StoredJWT.HasValidToken() == false)
            {
                SetLoginState(LoginState.Ready);
                return;
            }

            StartCoroutine(ProcessFetchUserFromRepository());
        }

        private IEnumerator ProcessFetchUserFromRepository()
        {
            SetLoginState(LoginState.UserFetch);

            if (userRepository == null)
            {
                userRepository = new OcarinaUserRepository();
            }

            yield return userRepository.FetchUser(StoredJWT.GetToken());

            var responseData = userRepository.GetResponseData();

            if (responseData.HasError)
            {
                Debug.LogError($"[UserManager] Error trying to fetch user from repository: {responseData.ErrorMessage}");
                OnLoginError?.Invoke(responseData.ErrorMessage);
                SetLoginState(LoginState.Ready);
                yield break;
            }

            UserModel = responseData.UserModel;

            AfterLogin();
        }

        private void SetLoginState(LoginState loginState)
        {
            CurrentLoginState = loginState;
            OnLoginStateChange?.Invoke(CurrentLoginState);
        }

        private void AfterLogin()
        {
            IsLoggedIn = true;
            GameManager.Instance.Initialize();
            SceneLoader.Instance.LoadScene(onLoginScene, () =>
            {
                CanvasManager.Instance.OpenMenu(Menu.Home);
            });
        }

        private void AfterLogout()
        {
            IsLoggedIn = false;
            StoredJWT.SetToken(string.Empty);
            SetLoginState(LoginState.Ready);
            SceneLoader.Instance.LoadScene(onLogoutScene);
        }

        [ContextMenu("Sign Out")]
        public void SignOut()
        {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {
                AuthenticationService.Instance.SignOut();
                return;
            }

            AfterLogout();
        }

        private void AuthenticationService_SignedOut()
        {
            AfterLogout();
        }
    }
}