namespace DreamQuiz
{
    public class UserModel
    {
        public string UserId { get; private set; }

        public UserModel(string userId)
        {
            UserId = userId;
        }
    }
}