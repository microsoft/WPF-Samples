using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win11ThemeGallery.ViewModels
{
    public partial class NotificationsPageViewModel
    {
        private static AppNotificationManager notificationManager;

        static NotificationsPageViewModel()
        {
            notificationManager = AppNotificationManager.Default;
            notificationManager.NotificationInvoked += OnNotificationInvoked;
            notificationManager.Register();
        }

        private static void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public void SendNotification(object pageType)
        {
            
                var appNotification = new AppNotificationBuilder()
                      .AddArgument("action", "ToastClick")
                      .AddText("Notification From Gallery App")
                      .AddText("This is an example message using WPF")
                      .AddButton(new AppNotificationButton("Open App")
                          .AddArgument("action", "OpenApp")
                      )
                      .BuildNotification();
                notificationManager.Show(appNotification);
            
        }
    }
}
