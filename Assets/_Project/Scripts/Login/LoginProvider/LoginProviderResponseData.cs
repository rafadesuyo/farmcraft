namespace DreamQuiz
{
    public class LoginProviderResponseData
    {
        public string Token = string.Empty;
        public bool HasError = false;
        public string ErrorMessage = string.Empty;

        public LoginProviderResponseData()
        {
            Token = string.Empty;
            HasError = false;
            ErrorMessage = string.Empty;
        }
    }
}