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
        public IncrementalLoadingCollection<TimelineHomeSource, StatusModel> TimelineHome { get; } = new IncrementalLoadingCollection<TimelineHomeSource, StatusModel>();
        public IncrementalLoadingCollection<NotificationSource, NotificationModel> Notification { get; } = new IncrementalLoadingCollection<NotificationSource, NotificationModel>();
        public IncrementalLoadingCollection<TimelineLocalSource, StatusModel> TimelineLocal { get; } = new IncrementalLoadingCollection<TimelineLocalSource, StatusModel>();
        public IncrementalLoadingCollection<TimelineFederatedSource, StatusModel> TimelineFederated { get; } = new IncrementalLoadingCollection<TimelineFederatedSource, StatusModel>();
        public IncrementalLoadingCollection<AccountStatusSource, StatusModel> AccountTimeline { get; } = new IncrementalLoadingCollection<AccountStatusSource, StatusModel>(new AccountStatusSource(Settings.CurrentAccount.Id));
        public NotifyTask<AccountModel> Account { get; private set; } = NotifyTask.Create(Accounts.VerifyCredentials(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken));

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

        public void RefreshAccount()
        {
            Account = NotifyTask.Create(Accounts.VerifyCredentials(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken));
            AccountTimeline.RefreshAsync();
        }
    }
}
