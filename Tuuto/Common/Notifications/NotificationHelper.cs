using Mastodon.Model;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Common.Helpers;
using Windows.UI.Notifications;

namespace Tuuto.Common.Notifications
{
    static class NotificationHelper
    {
        public static async Task CheckForNotification()
        {
            if (!Settings.Account.Any())
                return;
            try
            {
                var notification = (await Mastodon.Api.Notifications.Fetching(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken)).Result.Where(item => item.CreatedAt > Settings.LastNotify);
                foreach (var item in notification)
                {
                    ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(GenerateToastContent(item).GetXml()));
                }
                Settings.LastNotify = DateTime.UtcNow;
            }
            catch(Exception e)
            {
                Debug.WriteLine("NotificationHelper: Fetching notification failed " + e.Message);
            }
        }

        static ToastContent GenerateToastContent(NotificationModel model)
        {
            var text = $"{(string.IsNullOrEmpty(model.Account.DisplayName) ? model.Account.UserName : model.Account.DisplayName)}";
            var sub = "";
            switch (model.Type)
            {
                case NotificationModel.NOTIFICATIONTYPE_FAVOURITE:
                    text = $"{text}{ResourceHelper.GetString("StatusAction_favourite")}";
                    sub = model.Status.Content;
                    break;
                case NotificationModel.NOTIFICATIONTYPE_FOLLOW:
                    text = $"{text}{ResourceHelper.GetString("StatusAction_follow")}";
                    sub = model.Account.Note;
                    break;
                case NotificationModel.NOTIFICATIONTYPE_MENTION:
                    text = $"{text}{ResourceHelper.GetString("StatusAction_mention")}";
                    sub = model.Status.Content;
                    break;
                case NotificationModel.NOTIFICATIONTYPE_REBLOG:
                    text = $"{text}{ResourceHelper.GetString("StatusAction_reblog")}";
                    sub = model.Status.Content;
                    break;
                default:
                    break;
            }
            return new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        AppLogoOverride = new ToastGenericAppLogo
                        {
                            Source = model.Account.Avatar
                        },
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = text
                            },
                            new AdaptiveText
                            {
                                Text = sub
                            }
                        }
                    }
                },
                Launch = "action=notification"
            };
        }
    }
}
