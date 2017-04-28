using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastodon.Model;
using Windows.UI.Xaml.Controls;

namespace Tuuto.Common
{
    public class StatusActionHandler : IStatusActionHandler
    {
        private Frame _frame;

        public StatusActionHandler(Frame frame)
        {
            this._frame = frame;
        }

        public void AccountDetail(AccountModel model)
        {

        }
        
        public void Expand(StatusModel model)
        {

        }

        public void Mention(AccountModel model)
        {

        }

        public void Reply(StatusModel model)
        {

        }

        public void Report(AccountModel account, StatusModel status = null)
        {
            throw new NotImplementedException();
        }
    }
}
