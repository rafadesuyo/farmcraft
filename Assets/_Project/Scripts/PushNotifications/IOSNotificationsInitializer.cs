#if UNITY_EDITOR || UNITY_IOS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.iOS;
using System;

public static class IOSNotificationsInitializer 
{
    public const string categoryIdentifier = "category_a";
    public const string threadIdentifier = "thread1";

    private static List<string> scheduledNotificationsID;
    public static List<string> ScheduledNotificationsID => scheduledNotificationsID;

    public static void CreateIOSNotifications(PushNotificationInfosSO pushNotifications, int minIntervalHours, int maxIntervalHours)
    {
        if (scheduledNotificationsID == null)
        {
            scheduledNotificationsID = new List<string>();
        }
        else
        {
            scheduledNotificationsID.Clear();
        }

        var baseNotificationInfo = new iOSNotification
        {
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Sound,
            CategoryIdentifier = categoryIdentifier,
            ThreadIdentifier = threadIdentifier
        };

        int idValueCounter = 0;

        foreach (var item in pushNotifications.AllNotifications)
        {
            var iOSNotification = new iOSNotification();

            if (item.notificationType == PushNotificationInfosSO.NotificationType.DAILY_MISSIONS)
            {
                var trigger = new iOSNotificationCalendarTrigger()
                {
                    Hour = 0, //midnight
                    Minute = 0,
                    Repeats = true
                };

                iOSNotification = new iOSNotification
                {
                    Identifier = $"_{item.title.ToLower()}_{idValueCounter}",
                    Title = item.title,
                    Body = item.bodyText,
                    Trigger = trigger,
                    ShowInForeground = baseNotificationInfo.ShowInForeground,
                    ForegroundPresentationOption = baseNotificationInfo.ForegroundPresentationOption,
                    CategoryIdentifier = baseNotificationInfo.CategoryIdentifier,
                    ThreadIdentifier = baseNotificationInfo.ThreadIdentifier
                };
            }

            else
            {
                var trigger = new iOSNotificationTimeIntervalTrigger()
                {
                    TimeInterval = GetTimeIntervalInSeconds(minIntervalHours, maxIntervalHours),
                    Repeats = item.isRepeatable
                };

                iOSNotification = new iOSNotification
                {
                    Identifier = $"_{item.title.ToLower()}_{idValueCounter}",
                    Title = item.title,
                    Body = item.bodyText,
                    Trigger = trigger,
                    ShowInForeground = baseNotificationInfo.ShowInForeground,
                    ForegroundPresentationOption = baseNotificationInfo.ForegroundPresentationOption,
                    CategoryIdentifier = baseNotificationInfo.CategoryIdentifier,
                    ThreadIdentifier = baseNotificationInfo.ThreadIdentifier
                };
            }

            iOSNotificationCenter.ScheduleNotification(iOSNotification);
            scheduledNotificationsID.Add(iOSNotification.Identifier);
            idValueCounter++;
        }
    }

    private static TimeSpan GetTimeIntervalInSeconds(int minIntervalHours, int maxIntervalHours)
    {
        int totalHours = Mathf.RoundToInt(Mathf.Floor(HeartManager.Instance.TimeLeftInSecondsToFullHearts() / 3600));
        totalHours = Mathf.Clamp(totalHours, minIntervalHours, maxIntervalHours);

        int totalSeconds = totalHours * 3600;

        return TimeSpan.FromSeconds(totalSeconds);
    }
}
#endif
