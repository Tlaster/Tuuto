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
using Tuuto.DataSource;

namespace Tuuto.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        public ExIncrementalLoadingCollection<AccountStatusSource, StatusModel> AccountTimeline { get; } 
        public NotifyTask<AccountModel> Account { get; private set; }
        public NotifyTask<RelationshipModel> Relationship { get; private set; }
        public int Id { get; }

        public AccountViewModel(int id)
        {
            Id = id;
            AccountTimeline = new ExIncrementalLoadingCollection<AccountStatusSource, StatusModel>(new AccountStatusSource(Id));
            RefreshAccount();
            RefreshRelationship();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Refresh()
        {
            AccountTimeline.RefreshAsync();
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
