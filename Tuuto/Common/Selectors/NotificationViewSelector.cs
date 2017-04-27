using Mastodon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tuuto.Common.Selectors
{
    class NotificationViewSelector : DataTemplateSelector
    {
        public DataTemplate Status { get; set; }
        public DataTemplate Account { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null)
                return null;
            var model = item as NotificationModel;
            switch (model.Type)
            {
                case NotificationModel.NOTIFICATIONTYPE_FOLLOW:
                    return Account;
                default:
                    return Status;
            }
        }
    }
}
