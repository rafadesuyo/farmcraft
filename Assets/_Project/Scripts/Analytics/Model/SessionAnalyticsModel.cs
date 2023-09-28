using System;
using UnityEngine;

namespace DreamQuiz
{
    public class SessionAnalyticsModel
    {
        public string UserID { get; set; }
        public short SessionType { get; set; }
        public int SessionDuration { get; set; }

        public SessionAnalyticsModel(short sessionType)
        {
            UserID = LoginManager.Instance.UserModel.UserId;
            SessionType = sessionType;
            SessionDuration = Mathf.RoundToInt(Time.realtimeSinceStartup);
        }

        public SessionAnalyticsDto ToDTO()
        {
            return AnalyticsHelper.Map<SessionAnalyticsModel, SessionAnalyticsDto>(this);
        }
    }
}