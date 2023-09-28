using System;

namespace DreamQuiz
{
    [Serializable]
    public class ThirdPartyPayloadDto
    {
        public string thirdPartyId;
        public string email;
        public short thirdPartyAccount;
    }
}