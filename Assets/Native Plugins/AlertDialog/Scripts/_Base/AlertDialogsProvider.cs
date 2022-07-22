public static class AlertDialogsProvider
{
    public static IAlertDialogsProvider CreateAlertDialogsProvider()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return new AlertDialogsAndroidProvider();
#endif

        return new AlertDialogsBlankProvider();
    }
}