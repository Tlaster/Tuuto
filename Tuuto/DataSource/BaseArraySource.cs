using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Mastodon.Model;

namespace Tuuto.DataSource
{
    public abstract class BaseArraySource<T> : IIncrementalSource<T>
    {
        private int _maxid = 0;
        public async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (pageIndex == 0)
                _maxid = 0;
            var result = await GetArrayAsync(_maxid);
            _maxid = result.MaxId;
            return result.Result;
        }

        protected abstract Task<ArrayModel<T>> GetArrayAsync(int max_id);
    }
}
