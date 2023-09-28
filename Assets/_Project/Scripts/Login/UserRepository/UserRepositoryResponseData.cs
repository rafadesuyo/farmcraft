namespace DreamQuiz
{
    public class UserRepositoryResponseData
    {
        public UserModel UserModel;
        public bool HasError;
        public string ErrorMessage;

        public UserRepositoryResponseData()
        {
            UserModel = null;
            HasError = false;
            ErrorMessage = string.Empty;
        }
    }
}