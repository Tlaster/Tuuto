using Mastodon.Api;
using Mastodon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Common;
using PropertyChanged;

namespace Tuuto.DataSource
{
    [ImplementPropertyChanged]
    public class AccountStatusSource : BaseArraySource<StatusModel>
    {
        private readonly int _id;

        public AccountStatusSource(int id, bool only_media = false, bool exclude_replies = false)
        {
            _id = id;
            OnlyMedia = only_media;
            ExcludeReplies = exclude_replies;
        }

        public bool OnlyMedia { get; set; }
        public bool ExcludeReplies { get; set; }

        protected override async Task<ArrayModel<StatusModel>> GetArrayAsync(int max_id)
        {
            return await Accounts.Statuses(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, _id, max_id: max_id, only_media: OnlyMedia, exclude_replies: ExcludeReplies);
        }
    }
}
