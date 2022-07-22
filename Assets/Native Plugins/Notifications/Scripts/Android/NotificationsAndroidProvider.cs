using System;
using Unity.Notifications.Android;

public class NotificationsAndroidProvider : INotificationsProvider
{
    private const string ChannelId = "simple_channel";
    private const string ChannelName = "Simple Channel";
    private const string ChannelDescription = "Generic notifications";

    private readonly AndroidNotificationChannel _notificationChannel;
    
    public NotificationsAndroidProvider()
    {
        _notificationChannel = new AndroidNotificationChannel
        {
            Id = ChannelId,
            Name = ChannelName,
            Importance = Importance.High,
            Description = ChannelDescription,
        };
        
        AndroidNotificationCenter.RegisterNotificationChannel(_notificationChannel);
    }

    public void ShowNotification(string title, string text)
    {
        AndroidNotificationCenter.CancelAllNotifications();
        
        var notification = new AndroidNotification
        {
            Title = title,
            Text = text,
            FireTime = DateTime.Now,
            ShouldAutoCancel = true
        };
        
        AndroidNotificationCenter.SendNotification(notification, ChannelId);
    }
}