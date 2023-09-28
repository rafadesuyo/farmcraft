using System;

namespace DreamQuiz
{
    public interface ILoginProvider
    {
        void ProcessLogin(Action<LoginProviderResponseData> loginCallback);
    }
}