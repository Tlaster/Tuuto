using Mastodon.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tuuto.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Tuuto.Common.Helpers;
using Mastodon.Api;
using Microsoft.Toolkit.Uwp;
using System.Text.RegularExpressions;
using Tuuto.Common.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Tuuto.View
{
    public class StatusReblogConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = value as StatusModel;
            return item?.Reblog ?? item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public sealed partial class StatusView : UserControl
    {
        public StatusModel ViewModel
        {
            get { return (StatusModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(StatusModel), typeof(StatusView), new PropertyMetadata(null));



        public Visibility MenuExpandVisibility
        {
            get { return (Visibility)GetValue(MenuExpandVisibilityProperty); }
            set { SetValue(MenuExpandVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuExpandVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuExpandVisibilityProperty =
            DependencyProperty.Register(nameof(MenuExpandVisibility), typeof(Visibility), typeof(StatusView), new PropertyMetadata(Visibility.Visible));



        public StatusView()
        {
            this.InitializeComponent();
        }

        private async void LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
#if DEBUG
            Notification.Show(e.Link);
#endif
            var url = new Uri(e.Link);
            if (ViewModel.Mentions != null && ViewModel.Mentions.Any(item => item.Url == e.Link))
            {
                App.StatusAcionHandler.AccountDetail(ViewModel.Mentions.FirstOrDefault(item => item.Url == e.Link).Id);
            }
            else if (url.Host == new Uri(ViewModel.Account.Url).Host || url.Host == new Uri(Settings.CurrentAccountModel.Url).Host)
            {
                var groups = Regex.Match(e.Link, "(http|https)://([^/]*)/([^/]*)/([^/]*)").Groups;
                if (!groups[3].Success && groups[4].Success) await Launcher.LaunchUriAsync(url);
                var value = System.Net.WebUtility.UrlDecode(groups[4].Value);
                switch (groups[3].Value.ToLower())
                {
                    case "tags":
                        App.StatusAcionHandler.HashTag(value);
                        break;
                    default:
                        await Launcher.LaunchUriAsync(url);
                        break;
                }
            }
            else
            {
                await Launcher.LaunchUriAsync(url);
            }
        }

        private async void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            await new MediaViewDialog((ViewModel?.Reblog ?? ViewModel).MediaAttachments, (ViewModel?.Reblog ?? ViewModel).MediaAttachments.IndexOf(e.ClickedItem as AttachmentModel)).ShowAsync();
        }

        private void Reply_Click(object sender, RoutedEventArgs e)
        {
            App.StatusAcionHandler.Reply(ViewModel?.Reblog ?? ViewModel);
        }

        private void Retweet_Click(object sender, RoutedEventArgs e)
        {
            Statuses.Reblog(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, (ViewModel?.Reblog ?? ViewModel).Id);
        }

        private void UnRetweet_Click(object sender, RoutedEventArgs e)
        {
            Statuses.UnReblog(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, (ViewModel?.Reblog ?? ViewModel).Id);
        }

        private void Favourite_Click(object sender, RoutedEventArgs e)
        {
            Statuses.Favourite(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, (ViewModel?.Reblog ?? ViewModel).Id);
        }

        private void UnFavourite_Click(object sender, RoutedEventArgs e)
        {
            Statuses.UnFavourite(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, (ViewModel?.Reblog ?? ViewModel).Id);
        }

        void Report()
        {
            App.StatusAcionHandler.Report((ViewModel?.Reblog ?? ViewModel).Account, ViewModel?.Reblog ?? ViewModel);
        }

        void Block()
        {
            Accounts.Block(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, (ViewModel?.Reblog ?? ViewModel).Id);
        }

        void Mute()
        {
            Accounts.Mute(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, (ViewModel?.Reblog ?? ViewModel).Id);
        }

        void Expand()
        {
            App.StatusAcionHandler.Expand(ViewModel?.Reblog ?? ViewModel);
        }

        void Delete()
        {
            Statuses.Delete(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, (ViewModel?.Reblog ?? ViewModel).Id);
            Visibility = Visibility.Collapsed;
        }

        void Mention()
        {
            App.StatusAcionHandler.Mention((ViewModel?.Reblog ?? ViewModel).Account);
        }

        private void Account_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.StatusAcionHandler.AccountDetail((ViewModel?.Reblog ?? ViewModel).Account.Id);
        }
    }
}
