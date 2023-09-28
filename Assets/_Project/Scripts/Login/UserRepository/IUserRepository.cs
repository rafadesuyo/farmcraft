using System.Collections;

namespace DreamQuiz
{
    public interface IUserRepository
    {
        IEnumerator FetchUser(string bearerToken);
        UserRepositoryResponseData GetResponseData();
    }
}