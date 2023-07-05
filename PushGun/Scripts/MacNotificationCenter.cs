using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using AppKit;
using Foundation;
using GameKit;
using UserNotifications;

public class MacNotificationCenter
{
    public void ShowNotification(string iconBase64, string title, string message, string noti_id)
    {
        // setting notification info
        NSUserNotification notification = new NSUserNotification();
        notification.Title = title;
        notification.InformativeText = message;
        notification.Identifier = noti_id;
        notification.SoundName = Config.isMuted ? string.Empty : NSUserNotification.NSUserNotificationDefaultSoundName;
        notification.HasActionButton = false;
        notification.ContentImage = iconBase64.Equals(string.Empty) ? NSImage.ImageNamed("default_icon.png") : new NSImage(Utilizer.ParseImage(iconBase64));

        // setting center and delivering notification
        NSUserNotificationCenter center = NSUserNotificationCenter.DefaultUserNotificationCenter;
        center.Delegate = new MyUserNotificationCenterDelegate();
        foreach(var noti in center.DeliveredNotifications)
        {
            if (noti.Identifier.Equals(noti_id))
            {
                noti.InformativeText = message;
                return;
            }
        } 
        center.DeliverNotification(notification);
    }

    public class MyUserNotificationCenterDelegate : NSUserNotificationCenterDelegate
    {
        public override void DidActivateNotification(NSUserNotificationCenter center, NSUserNotification noti)
        {
            center.RemoveDeliveredNotification(noti);
        }
    }
}