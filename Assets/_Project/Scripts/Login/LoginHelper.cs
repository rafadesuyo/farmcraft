using UnityEngine;

namespace DreamQuiz
{
    public static class LoginHelper
    {
        public const string FormUserIdKey = "formUserId";
        public const string FormUserPasswordKey = "formUserPassword";

        public const string OcarinaAuthUrl = "https://dreamquiztest.ocarinastudio.com/api/game/accounts/auth";
        public const string ThirdPartyAuthUrl = "https://dreamquiztest.ocarinastudio.com/api/game/accounts/third/party/auth";

        public const int ConnectionTimeout = 30;
        public const string DeviceId = "1";
        public const string DeviceType = "unity";

        public static ILoginProvider GetLoginProvider()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return new GoogleLoginProvider();
            }
            #if UNITY_EDITOR || UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return new AppleLoginProvider();
            }
            #endif
            return new OcarinaLoginProvider();
        }
    }
}