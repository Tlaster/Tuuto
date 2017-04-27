using Mastodon.Api;
using Mastodon.Model;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tuuto.Common;

namespace Tuuto.DataSource
{
    public class TimelineLocalSource : BaseArraySource<StatusModel>
    {
        protected override async Task<ArrayModel<StatusModel>> GetArrayAsync(int max_id)
        {
            return await Timelines.Public(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, max_id: max_id, local: true);
        }
    }
}
