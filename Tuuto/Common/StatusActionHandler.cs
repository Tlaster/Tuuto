using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastodon.Model;
using Windows.UI.Xaml.Controls;
using Tuuto.View;
using Tuuto.ViewModel;
using Tuuto.Common.Controls;

namespace Tuuto.Common
{
    public class StatusActionHandler : IStatusActionHandler
    {
        private Frame _frame;

        public StatusActionHandler(Frame frame)
        {
            this._frame = frame;
        }

        public void AccountDetail(int id)
        {
            _frame.Navigate(typeof(AccountPage), new AccountViewModel(id));
        }
        
        public void Expand(StatusModel model)
        {

        }

        public void HashTag(string tag)
        {

        }

        public void Mention(AccountModel model)
        {
            new TootDialog().WithText($"@{model.Acct} ").ShowAsync();
        }

        public void Reply(StatusModel model)
        {
            new TootDialog().WithReply(model).ShowAsync();
        }

        public void Report(AccountModel account, StatusModel status = null)
        {

        }
    }
}
