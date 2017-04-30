using Mastodon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuuto.Common
{
    public interface IStatusActionHandler
    {
        void Report(AccountModel account, StatusModel status = null);
        void Reply(StatusModel model);
        void Expand(StatusModel model);
        void AccountDetail(int id);
        void Mention(AccountModel model);
        void HashTag(string tag);
    }
}
