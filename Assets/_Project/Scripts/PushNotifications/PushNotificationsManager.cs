using System;
using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS 
using Unity.Notifications.iOS;
#endif

public class PushNotificationsManager : LocalSingleton<PushNotificationsManager>
{
    [SerializeField] PushNotificationInfosSO androidPushNotifications;
    [SerializeField] PushNotificationInfosSO iOSPushNotifications;

    [SerializeField] private int minIntervalHours = 4;
    [SerializeField] private int maxIntervalHours = 12;

    private void Start()
    {
#if UNITY_ANDROID
        AndroidNotificationsInitializer.RequestAndroidAuthorization();
        AndroidNotificationsInitializer.RegisterAndroidNotificationChannel();
        AndroidNotificationCenter.CancelAllNotifications();
#elif UNITY_IOS
        iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif
    }

    //Whenever player minimizes game using Home button, notifications are set.
    private void OnApplicationPause(bool gamePauseStatus)
    {
        if (gamePauseStatus == true)
        {
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationsInitializer.CreateAndroidNotifications(androidPushNotifications, minIntervalHours, maxIntervalHours);
#elif UNITY_IOS
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            IOSNotificationsInitializer.CreateIOSNotifications(iOSPushNotifications, minIntervalHours, maxIntervalHours);
#endif
        }
    }

    //Whenever player exits game, notifications are set.
    private void OnApplicationQuit()
    {
#if UNITY_ANDROID
        AndroidNotificationsInitializer.CreateAndroidNotifications(androidPushNotifications, minIntervalHours, maxIntervalHours);
#elif UNITY_IOS
        IOSNotificationsInitializer.CreateIOSNotifications(iOSPushNotifications, minIntervalHours, maxIntervalHours);
#endif
    }
}
