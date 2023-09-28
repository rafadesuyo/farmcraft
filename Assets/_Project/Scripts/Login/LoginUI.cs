using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DreamQuiz
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] Button loginButton;
        [SerializeField] TMP_InputField userIdInputField;
        [SerializeField] TMP_InputField passwordInputField;
        [SerializeField] CanvasGroup loginPanel;
        [SerializeField] CanvasGroup spinnerPanel;

        private void Start()
        {
            loginButton.onClick.AddListener(Login);

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                userIdInputField.gameObject.SetActive(false);
                passwordInputField.gameObject.SetActive(false);
            }
            else
            {
                userIdInputField.text = PlayerPrefs.GetString(LoginHelper.FormUserIdKey);
                userIdInputField.onValueChanged.AddListener(SetUserID);
                passwordInputField.text = PlayerPrefs.GetString(LoginHelper.FormUserPasswordKey);
                passwordInputField.onValueChanged.AddListener(SetPassword);
            }

            LoginManager.Instance.OnLoginStateChange += Instance_OnLoginStateChange;
            UpdateUI(LoginManager.Instance.CurrentLoginState);
        }

        private void OnDestroy()
        {
            LoginManager.Instance.OnLoginStateChange -= Instance_OnLoginStateChange;
        }

        private void OnEnable()
        {
            UpdateUI(LoginManager.Instance.CurrentLoginState);
        }

        private void Instance_OnLoginStateChange(LoginManager.LoginState loginState)
        {
            UpdateUI(loginState);
        }

        private void UpdateUI(LoginManager.LoginState loginState)
        {
            if (loginState == LoginManager.LoginState.Ready)
            {
                loginPanel.interactable = true;
                spinnerPanel.alpha = 0;
            }
            else
            {
                loginPanel.interactable = false;
                spinnerPanel.alpha = 1;
            }
        }

        public void Login()
        {
            LoginManager.Instance.Login();
        }

        public void SetUserID(string value)
        {
            PlayerPrefs.SetString(LoginHelper.FormUserIdKey, value);
        }

        public void SetPassword(string value)
        {
            PlayerPrefs.SetString(LoginHelper.FormUserPasswordKey, value);
        }
    }
}