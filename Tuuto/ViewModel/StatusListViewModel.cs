using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastodon.Model;
using Tuuto.Common;

namespace Tuuto.ViewModel
{
    public class StatusListViewModel
    {
        public ArrayIncrementalLoading<StatusModel> List { get; }

        public StatusListViewModel(Func<int, Task<ArrayModel<StatusModel>>> source)
        {
            List = new ArrayIncrementalLoading<StatusModel>(source);
        }
    }
}
