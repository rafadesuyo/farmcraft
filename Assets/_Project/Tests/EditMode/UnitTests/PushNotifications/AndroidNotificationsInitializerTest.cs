#if UNITY_EDITOR || UNITY_ANDROID
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Notifications.Android;

public class AndroidNotificationsInitializerTest
{
    [TestCase(4, 12, 2, true)]
    [TestCase(-2, 7, 5, true)]
    [TestCase(3, -9, 4, false)]
    public void CreateAndroidNotifications_HappyPath(int minInterval, int maxInterval, int notificationsAmount, bool isRepeatable)
    {
        // Arrange
        PushNotificationInfosSO pushNotifications = ScriptableObject.CreateInstance<PushNotificationInfosSO>();

        for (int i = 0; i < notificationsAmount; i++)
        {
            PushNotificationInfosSO.NotificationInfo testNotification = new PushNotificationInfosSO.NotificationInfo()
            {
                title = "Title",
                bodyText = "bodyText",
                notificationType = PushNotificationInfosSO.NotificationType.DAILY_MISSIONS,
                isRepeatable = isRepeatable
            };

            pushNotifications.AllNotifications.Add(testNotification);
        }

        // Act
        AndroidNotificationsInitializer.CreateAndroidNotifications(pushNotifications, minInterval, maxInterval);

        // Assert
        Assert.IsTrue(AndroidNotificationsInitializer.ScheduledNotificationsID.Count == notificationsAmount,
                      $"Expected {notificationsAmount} scheduled notifications, received {AndroidNotificationsInitializer.ScheduledNotificationsID.Count}");
    }
}
#endif
