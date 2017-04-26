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
    public class TimelineHomeSource : IIncrementalSource<StatusModel>
    {
        private int _maxid = 0;

        public async Task<IEnumerable<StatusModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (pageIndex == 0)
                _maxid = 0;
            var result = await Timelines.Home(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, max_id: _maxid);
            _maxid = result.MaxId;
            return result.Result;
        }
    }
}
