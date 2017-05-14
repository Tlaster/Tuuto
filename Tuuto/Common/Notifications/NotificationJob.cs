using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastodon.Api;
using Microsoft.Toolkit.Uwp.Notifications;
using Mastodon.Model;
using Tuuto.Common.Helpers;
using Windows.UI.Notifications;

namespace Tuuto.Common.Notifications
{
    class NotificationJob : IJob
    {
        public void Execute()
        {
            ExecuteAsync().Wait();
        }
        async Task ExecuteAsync()
        {
            await NotificationHelper.CheckForNotification();
        }


    }
}
