public static class NotificationsProvider
{
    public static INotificationsProvider CreateNotificationsProvider()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return new NotificationsAndroidProvider();
#endif

        return new NotificationsBlankProvider();
    }
}
