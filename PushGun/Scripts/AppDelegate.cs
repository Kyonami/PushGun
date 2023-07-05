using System;
using AppKit;
using Foundation;

namespace PushGun
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private NSStatusItem statusItem;

        private Action onLaunchingFinished;

        public AppDelegate(Action onLaunchingFinished)
        {
            this.onLaunchingFinished = onLaunchingFinished;
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // 트레이 아이콘 생성
            NSStatusBar statusBar = NSStatusBar.SystemStatusBar;
            statusItem = statusBar.CreateStatusItem(NSStatusItemLength.Square);


            // 트레이에 표기될 아이콘
            NSImage icon = NSImage.ImageNamed("icon.png");
            icon.Size = new CoreGraphics.CGSize { Width = 18, Height = 18 };
            statusItem.Button.Image = icon;


            // 창을 숨기고 백그라운드에서 실행되도록 설정
            NSApplication.SharedApplication.DangerousWindows[0].IsVisible = false;
            NSApplication.SharedApplication.ActivationPolicy = NSApplicationActivationPolicy.Prohibited;


            // 메뉴 생성
            var menu = new NSMenu();

            menu.AddItem(new NSMenuItem("Quit", "q", OnQuitMenuItemClicked));
            menu.AddItem(new NSMenuItem("Pause", "p", PauseMenuItemClicked));
            menu.AddItem(new NSMenuItem("Mute", "m", MuteMenuItemClicked));
            menu.AddItem(new NSMenuItem("Ignore Programs On List", "i", IgnoreProgramsOnListMenuItemClicked));

            statusItem.Menu = menu;


            // 다 끝났으면 콜백 호출
            onLaunchingFinished?.Invoke();
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return false;
        }

        private void OnQuitMenuItemClicked(object sender, EventArgs e)
        {
            NSApplication.SharedApplication.Terminate(sender as Foundation.NSObject); // 프로그램 종료
        }
        private void PauseMenuItemClicked(object sender, EventArgs e)
        {
            NSMenuItem menuItem = (NSMenuItem)sender;
            menuItem.State = menuItem.State == NSCellStateValue.On ? NSCellStateValue.Off : NSCellStateValue.On;

            if (menuItem.State == NSCellStateValue.On)
                PushReceiver.it.StopGetting();
            else
                PushReceiver.it.StartGetting();

        }
        private void MuteMenuItemClicked(object sender, EventArgs e)
        {
            NSMenuItem menuItem = (NSMenuItem)sender;
            menuItem.State = menuItem.State == NSCellStateValue.On ? NSCellStateValue.Off : NSCellStateValue.On;

            Config.isMuted = menuItem.State == NSCellStateValue.On;
        }

        private void IgnoreProgramsOnListMenuItemClicked(object sender, EventArgs e)
        {
            NSMenuItem menuItem = (NSMenuItem)sender;
            menuItem.State = menuItem.State == NSCellStateValue.On ? NSCellStateValue.Off : NSCellStateValue.On;

            Config.shouldIgnoreProgramsOnList = menuItem.State == NSCellStateValue.On;
        }
    }
}

