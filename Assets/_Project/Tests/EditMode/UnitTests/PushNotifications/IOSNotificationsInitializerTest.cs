#if UNITY_EDITOR || UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Notifications.iOS;

public class IOSNotificationsInitializerTest
{
    [TestCase(4, 12, 2, true)]
    [TestCase(-2, 7, 5, true)]
    [TestCase(3, -9, 4, false)]
    public void CreateIOSNotifications_HappyPath(int minInterval, int maxInterval, int notificationsAmount, bool isRepeatable)
    {
        // Arrange
        PushNotificationInfosSO testPushNotifications = ScriptableObject.CreateInstance<PushNotificationInfosSO>();

        for (int i = 0; i < notificationsAmount; i++)
        {
            PushNotificationInfosSO.NotificationInfo testNotification = new PushNotificationInfosSO.NotificationInfo()
            {
                title = "Title",
                bodyText = "bodyText",
                notificationType = PushNotificationInfosSO.NotificationType.DAILY_MISSIONS,
                isRepeatable = isRepeatable
            };

            testPushNotifications.AllNotifications.Add(testNotification);
        }

        // Act
        IOSNotificationsInitializer.CreateIOSNotifications(testPushNotifications, minInterval, maxInterval);

        // Assert
        Assert.IsTrue(IOSNotificationsInitializer.ScheduledNotificationsID.Count == notificationsAmount,
                      $"Expected {notificationsAmount} scheduled notifications, received {IOSNotificationsInitializer.ScheduledNotificationsID.Count}");
    }
}
#endif