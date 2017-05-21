using Mastodon.Api;
using Mastodon.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Tuuto.Common;
using Tuuto.Common.Controls;
using Tuuto.ViewModel;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Tuuto.View
{
    public sealed partial class AccountView : ExListView
    {
        //public event EventHandler RefreshRelationshipRequested;

        public AccountViewModel ViewModel
        {
            get { return (AccountViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AccountViewModel), typeof(AccountView), new PropertyMetadata(null));



        //public AccountModel Account
        //{
        //    get { return (AccountModel)GetValue(AccountProperty); }
        //    set { SetValue(AccountProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Account.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty AccountProperty =
        //    DependencyProperty.Register(nameof(Account), typeof(AccountModel), typeof(AccountView), new PropertyMetadata(null));



        //public RelationshipModel Relationship
        //{
        //    get { return (RelationshipModel)GetValue(RelationshipProperty); }
        //    set { SetValue(RelationshipProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Relationship.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty RelationshipProperty =
        //    DependencyProperty.Register(nameof(Relationship), typeof(RelationshipModel), typeof(AccountView), new PropertyMetadata(null));



        public AccountView()
        {
            this.InitializeComponent();
            //x:Bind does not work for ViewModel.Relationship
            DataContext = this;
        }


        void Mention()
        {
            App.StatusAcionHandler.Mention(ViewModel.Account.Result);
        }

        void Report()
        {
            App.StatusAcionHandler.Report(ViewModel.Account.Result);
        }

        async void Block()
        {
            ViewModel.Relationship = await Accounts.Block(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, ViewModel.Account.Result.Id);
            //ViewModel.RefreshRelationship();
        }
        async void UnBlock()
        {
            ViewModel.Relationship = await Accounts.UnBlock(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, ViewModel.Account.Result.Id);
            //ViewModel.RefreshRelationship();
        }

        async void Mute()
        {
            ViewModel.Relationship = await Accounts.Mute(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, ViewModel.Account.Result.Id);
            //ViewModel.RefreshRelationship();
        }
        async void UnMute()
        {
            ViewModel.Relationship = await Accounts.UnMute(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, ViewModel.Account.Result.Id);
            //ViewModel.RefreshRelationship();
        }
        async void Follow()
        {
            ViewModel.Relationship = await Accounts.Follow(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, ViewModel.Account.Result.Id);
            //ViewModel.RefreshRelationship();
        }
        async void UnFollow()
        {
            ViewModel.Relationship = await Accounts.UnFollow(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, ViewModel.Account.Result.Id);
            //ViewModel.RefreshRelationship();
        }
        async void OpenInBrowser()
        {
            await Launcher.LaunchUriAsync(new Uri(ViewModel.Account.Result.Url));
        }

        private async void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
#if DEBUG
            Notification.Show(e.Link);
#endif
            var url = new Uri(e.Link);
            if (url.Host == new Uri(ViewModel.Account.Result.Url).Host || url.Host == new Uri(Settings.CurrentAccountModel.Url).Host)
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

        private void listView_RefreshRequested(object sender, EventArgs e)
        {
            //RefreshRequested="{x:Bind ViewModel.Refresh}" will cause "Xaml Internal Error error WMC9999"
            ViewModel.Refresh();
        }
    }
}
