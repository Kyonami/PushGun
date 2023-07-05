using System;
using AppKit;
using Foundation;
using PushGun;

public class Program
{
    ~Program()
    {
        NSUserNotificationCenter.DefaultUserNotificationCenter.RemoveAllDeliveredNotifications();   // 프로그램 사망시 모든 알림 제거.
    }

    public static void Main(string[] args)
    {
        NSApplication.Init();
        NSApplication.SharedApplication.Delegate = new AppDelegate(OnLaunchFinished);           
        NSApplication.SharedApplication.Run();
    }

    private static void OnLaunchFinished()
    {
        // 노티를 받기 시작
        MacNotificationCenter notificationCenter = new MacNotificationCenter();

        PushReceiver.it.Init("o.VHB358PlzEBHdgqILRdmOXTbwhCZ3aOT", notificationCenter.ShowNotification);
        PushReceiver.it.StartGetting();
    }
}
