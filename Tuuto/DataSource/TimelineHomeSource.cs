using Mastodon.Api;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Tuuto.Common;
using Mastodon.Model;

namespace Tuuto.DataSource
{
    public class TimelineHomeSource : BaseArraySource<StatusModel>
    {
        protected override async Task<ArrayModel<StatusModel>> GetArrayAsync(int max_id)
        {
            return await Timelines.Home(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, max_id: max_id);
        }
    }
}
