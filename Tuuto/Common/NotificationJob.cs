using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastodon.Api;

namespace Tuuto.Common
{
    class NotificationJob : IJob
    {
        public void Execute()
        {
            ExecuteAsync().Wait();
        }
        async Task ExecuteAsync()
        {
            var notification = await Notifications.GetSingle(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, Settings.CurrentAccount.Id);

        }
    }
}
