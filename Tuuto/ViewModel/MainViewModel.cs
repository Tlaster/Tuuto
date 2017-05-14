using Mastodon.Model;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.Mvvm;
using Tuuto.Common;
using Mastodon.Api;

namespace Tuuto.ViewModel
{
    public class MainViewModel
    {
        public ArrayIncrementalLoading<StatusModel> TimelineHome { get; } = new ArrayIncrementalLoading<StatusModel>((max_id) => Timelines.Home(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, max_id: max_id));
        public ArrayIncrementalLoading<NotificationModel> Notification { get; } = new ArrayIncrementalLoading<NotificationModel>((max_id) => Notifications.Fetching(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, max_id: max_id));
        public ArrayIncrementalLoading<StatusModel> TimelineLocal { get; } = new ArrayIncrementalLoading<StatusModel>((max_id) => Timelines.Public(Settings.CurrentAccount.Domain, max_id: max_id, local: true));
        public ArrayIncrementalLoading<StatusModel> TimelineFederated { get; } = new ArrayIncrementalLoading<StatusModel>((max_id) => Timelines.Public(Settings.CurrentAccount.Domain, max_id: max_id));
        public AccountViewModel Account { get; } = new AccountViewModel(Settings.CurrentAccount.Id);
    }
}
