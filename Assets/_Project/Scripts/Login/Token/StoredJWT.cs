using UnityEngine;

namespace DreamQuiz
{
    public static class StoredJWT
    {
        private const string tokenKey = "token";

        public static bool HasValidToken()
        {
            if (PlayerPrefs.HasKey(tokenKey) == false)
            {
                return false;
            }

            string token = PlayerPrefs.GetString(tokenKey);

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            return true;
        }

        public static void SetToken(string tokenValue)
        {
            PlayerPrefs.SetString(tokenKey, tokenValue);
        }

        public static string GetToken()
        {
            return PlayerPrefs.GetString(tokenKey);
        }

        public static void ClearToken()
        {
            PlayerPrefs.SetString(tokenKey, string.Empty);
        }
    }
}