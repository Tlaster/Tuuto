using Mastodon.Model;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.DataSource;

namespace Tuuto.ViewModel
{
    public class MainViewModel
    {
        public IncrementalLoadingCollection<TimelineHomeSource, StatusModel> TimelineHome { get; } = new IncrementalLoadingCollection<TimelineHomeSource, StatusModel>();
        public IncrementalLoadingCollection<NotificationSource, NotificationModel> Notification { get; } = new IncrementalLoadingCollection<NotificationSource, NotificationModel>();
        public IncrementalLoadingCollection<TimelineLocalSource, StatusModel> TimelineLocal { get; } = new IncrementalLoadingCollection<TimelineLocalSource, StatusModel>();
        public IncrementalLoadingCollection<TimelineFederatedSource, StatusModel> TimelineFederated { get; } = new IncrementalLoadingCollection<TimelineFederatedSource, StatusModel>();
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
