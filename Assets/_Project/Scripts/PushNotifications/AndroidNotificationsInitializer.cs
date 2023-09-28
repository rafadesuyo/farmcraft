#if UNITY_EDITOR || UNITY_ANDROID

using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;
using System;
using System.Collections.Generic;

public static class AndroidNotificationsInitializer
{
    private const string defaultChannelID = "default_channel";
    private const string defaultChannelName = "Default Channel";
    private const string defaultChannelDescription = "General notifications";
    private const string androidPostNotificationsPermission = "android.permission.POST_NOTIFICATIONS";

    private static List<int> scheduledNotificationsID;
    public static List<int> ScheduledNotificationsID => scheduledNotificationsID;

    public static void RequestAndroidAuthorization()
    {
        if (!Permission.HasUserAuthorizedPermission(androidPostNotificationsPermission))
        {
            Permission.RequestUserPermission(androidPostNotificationsPermission);
        }
    }

    public static void RegisterAndroidNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = defaultChannelID,
            Name = defaultChannelName,
            Importance = Importance.Default,
            Description = defaultChannelDescription
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    public static void CreateAndroidNotifications(PushNotificationInfosSO pushNotifications, int minIntervalHours, int maxIntervalHours)
    {
        if (scheduledNotificationsID == null)
        {
            scheduledNotificationsID = new List<int>();
        }
        else
        {
            scheduledNotificationsID.Clear();
        }

        foreach (var item in pushNotifications.AllNotifications)
        {
            AndroidNotification androidNotification = new AndroidNotification();
            androidNotification.Title = item.title;
            androidNotification.Text = item.bodyText;
            androidNotification.FireTime = GetFireTime(item, minIntervalHours, maxIntervalHours);

            if (!string.IsNullOrEmpty(item.iconRef))
            {
                androidNotification.LargeIcon = item.iconRef;
            }

            if (item.isRepeatable)
            {
                androidNotification.RepeatInterval = GetRepeatInterval(item, maxIntervalHours);
            }

            scheduledNotificationsID.Add(AndroidNotificationCenter.SendNotification(androidNotification, defaultChannelID));
        }
    }

    private static DateTime GetFireTime(PushNotificationInfosSO.NotificationInfo notificationInfo, int minIntervalHours, int maxIntervalHours)
    {
        DateTime now = DateTime.Now;

        if (notificationInfo.notificationType == PushNotificationInfosSO.NotificationType.DAILY_MISSIONS)
        {
            DateTime nextMidnight = now.Date.AddDays(1);
            int fireTimeInSeconds = (int)(nextMidnight - now).TotalSeconds;

            return now.AddSeconds(fireTimeInSeconds);
        }

        int totalHours = Mathf.RoundToInt(Mathf.Floor(HeartManager.Instance.TimeLeftInSecondsToFullHearts() / 3600));

        if (minIntervalHours < 0 || maxIntervalHours < 0)
        {
            minIntervalHours = Mathf.Abs(minIntervalHours);
            maxIntervalHours = Mathf.Abs(maxIntervalHours);
        }

        if (maxIntervalHours < minIntervalHours)
        {
            maxIntervalHours = minIntervalHours;
        }

        totalHours = Mathf.Clamp(totalHours, minIntervalHours, maxIntervalHours);

        TimeSpan timeUntilNextNotification = TimeSpan.FromHours(totalHours);
        return now.Add(timeUntilNextNotification);
    }

    private static TimeSpan GetRepeatInterval(PushNotificationInfosSO.NotificationInfo notificationInfo, int maxIntervalHours)
    {
        if (notificationInfo.notificationType == PushNotificationInfosSO.NotificationType.DAILY_MISSIONS)
        {
            return TimeSpan.FromDays(1);
        }

        return TimeSpan.FromHours(Mathf.Abs(maxIntervalHours));
    }
}

#endif
