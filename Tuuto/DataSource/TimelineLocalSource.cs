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
    public class TimelineLocalSource : IIncrementalSource<StatusModel>
    {
        private int _maxid = 0;

        public async Task<IEnumerable<StatusModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (pageIndex == 0)
                _maxid = 0;
            var result = await Timelines.Public(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, max_id: _maxid, local: true);
            _maxid = result.MaxId;
            return result.Result;
        }
    }
}
