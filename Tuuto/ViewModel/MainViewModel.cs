using Mastodon.Model;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.DataSource;
using Nito.Mvvm;
using Tuuto.Common;
using Mastodon.Api;

namespace Tuuto.ViewModel
{
    public class MainViewModel
    {
        public ExIncrementalLoadingCollection<TimelineHomeSource, StatusModel> TimelineHome { get; } = new ExIncrementalLoadingCollection<TimelineHomeSource, StatusModel>();
        public ExIncrementalLoadingCollection<NotificationSource, NotificationModel> Notification { get; } = new ExIncrementalLoadingCollection<NotificationSource, NotificationModel>();
        public ExIncrementalLoadingCollection<TimelineLocalSource, StatusModel> TimelineLocal { get; } = new ExIncrementalLoadingCollection<TimelineLocalSource, StatusModel>();
        public ExIncrementalLoadingCollection<TimelineFederatedSource, StatusModel> TimelineFederated { get; } = new ExIncrementalLoadingCollection<TimelineFederatedSource, StatusModel>();
        public AccountViewModel Account { get; } = new AccountViewModel(Settings.CurrentAccount.Id);
        public void RefreshTimelineHome()
        {
            TimelineHome.RefreshAsync();
        }
        public void RefreshNotification()
        {
            Notification.RefreshAsync();
        }
        public void RefreshTimelineLocal()
        {
            TimelineLocal.RefreshAsync();
        }
        public void RefreshTimelineFederated()
        {
            TimelineFederated.RefreshAsync();
        }
    }
}
