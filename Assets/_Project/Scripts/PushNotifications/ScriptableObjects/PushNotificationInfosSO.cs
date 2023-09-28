using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PushNotificationInfos", menuName = "PushNotificationInfos")]
public class PushNotificationInfosSO : ScriptableObject
{   
    public enum NotificationType
    {
        DAILY_MISSIONS,
        HEARTS
    }

    [System.Serializable]
    public struct NotificationInfo
    {
        public string title;
        public string bodyText;
        public string iconRef;
        public NotificationType notificationType;
        public bool isRepeatable;
    }

    [SerializeField] List<NotificationInfo> allNotifications = new List<NotificationInfo>();
    public List<NotificationInfo> AllNotifications
    {
        get => allNotifications;
        set => allNotifications = value;
    }
}
