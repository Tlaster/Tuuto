using FontAwesome.UWP;
using Mastodon.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tuuto.Common.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Tuuto.Common.Controls
{
    public sealed partial class StatusAction : UserControl, INotifyPropertyChanged
    {
        public event EventHandler<AccountModel> AccountClick;
        public event PropertyChangedEventHandler PropertyChanged;

        public FontAwesomeIcon Icon
        {
            get { return (FontAwesomeIcon)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(Icon), typeof(FontAwesomeIcon), typeof(StatusAction), new PropertyMetadata(FontAwesomeIcon.SmileOutline));



        public string ActionType
        {
            get { return (string)GetValue(ActionTypeProperty); }
            set { SetValue(ActionTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionTypeProperty =
            DependencyProperty.Register(nameof(ActionType), typeof(string), typeof(StatusAction), new PropertyMetadata("", OnActionTypeChanged));

        private static void OnActionTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as StatusAction).OnActionTypeChanged(e.NewValue as string);
        }

        private void OnActionTypeChanged(string v)
        {
            switch (v)
            {
                case NotificationModel.NOTIFICATIONTYPE_FAVOURITE:
                    Icon = FontAwesomeIcon.Star;
                    break;
                case NotificationModel.NOTIFICATIONTYPE_FOLLOW:
                    Icon = FontAwesomeIcon.UserPlus;
                    break;
                case NotificationModel.NOTIFICATIONTYPE_MENTION:
                    Icon = FontAwesomeIcon.At;
                    break;
                case NotificationModel.NOTIFICATIONTYPE_REBLOG:
                    Icon = FontAwesomeIcon.Retweet;
                    break;
                default:
                    break;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Icon)));
            ActionText = ResourceHelper.GetString($"StatusAction_{v}");
        }



        public string ActionText
        {
            get { return (string)GetValue(ActionTextProperty); }
            set { SetValue(ActionTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionTextProperty =
            DependencyProperty.Register(nameof(ActionText), typeof(string), typeof(StatusAction), new PropertyMetadata(""));



        public AccountModel ActionAccount
        {
            get { return (AccountModel)GetValue(ActionAccountProperty); }
            set { SetValue(ActionAccountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionAccount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionAccountProperty =
            DependencyProperty.Register(nameof(ActionAccount), typeof(AccountModel), typeof(StatusAction), new PropertyMetadata(null));




        public StatusAction()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AccountClick?.Invoke(this, ActionAccount);
        }
    }
}
