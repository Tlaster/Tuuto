using Mastodon.Api;
using Mastodon.Model;
using Nito.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Common;

namespace Tuuto.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        public ArrayIncrementalLoading<StatusModel> AccountTimeline { get; } 
        public NotifyTask<AccountModel> Account { get; private set; }
        public NotifyTask<RelationshipModel> Relationship { get; private set; }
        public int Id { get; }
        public bool OnlyMedia { get; set; } = false;
        public bool ExcludeReplies { get; set; } = false;

        public AccountViewModel(int id)
        {
            Id = id;
            AccountTimeline = new ArrayIncrementalLoading<StatusModel>((max_id) => Accounts.Statuses(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, id, max_id: max_id, only_media: OnlyMedia, exclude_replies: ExcludeReplies));
            RefreshAccount();
            RefreshRelationship();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Refresh()
        {
            AccountTimeline.Refresh();
            RefreshAccount();
            RefreshRelationship();
        }
        public void RefreshRelationship()
        {
            Relationship = NotifyTask.Create(GetRelationshipTask);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Relationship)));
        }

        private async Task<RelationshipModel> GetRelationshipTask()
        {
            return (await Accounts.Relationships(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, Id)).Result.FirstOrDefault();
        }

        public void RefreshAccount()
        {
            Account = NotifyTask.Create(Accounts.Fetching(Settings.CurrentAccount.Domain, Id, Settings.CurrentAccount.AccessToken));
        }
    }
}
