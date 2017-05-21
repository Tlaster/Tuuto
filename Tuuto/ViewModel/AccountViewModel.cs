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
using PropertyChanged;

namespace Tuuto.ViewModel
{
    [ImplementPropertyChanged]
    public class AccountViewModel
    {
        public ArrayIncrementalLoading<StatusModel> AccountTimeline { get; } 
        public NotifyTask<AccountModel> Account { get; private set; }
        public RelationshipModel Relationship { get; internal set; }

        public int Id { get; }
        public bool OnlyMedia { get; set; } = false;
        public bool ExcludeReplies { get; set; } = false;

        public void OnOnlyMediaChanged()
        {
            AccountTimeline.Refresh();
        }

        public void OnExcludeRepliesChanged()
        {
            AccountTimeline.Refresh();
        }

        public AccountViewModel(int id)
        {
            Id = id;
            AccountTimeline = new ArrayIncrementalLoading<StatusModel>((max_id) => Accounts.Statuses(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, Id, max_id: max_id, only_media: OnlyMedia, exclude_replies: ExcludeReplies));
            RefreshAccount();
            RefreshRelationship();
        }

        public void Refresh()
        {
            AccountTimeline.Refresh();
            RefreshAccount();
            RefreshRelationship();
        }

        public async void RefreshRelationship()
        {
            Relationship = await GetRelationshipTask();
            //Relationship = NotifyTask.Create(GetRelationshipTask);
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
