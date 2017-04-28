﻿using Mastodon.Api;
using Mastodon.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tuuto.Common;
using Tuuto.Common.Controls;
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

namespace Tuuto.View
{
    public sealed partial class AccountView : ExListView
    {

        public AccountModel Account
        {
            get { return (AccountModel)GetValue(AccountProperty); }
            set { SetValue(AccountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Account.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccountProperty =
            DependencyProperty.Register(nameof(Account), typeof(AccountModel), typeof(AccountView), new PropertyMetadata(null));



        public RelationshipModel Relationship
        {
            get { return (RelationshipModel)GetValue(RelationshipProperty); }
            set { SetValue(RelationshipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Relationship.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelationshipProperty =
            DependencyProperty.Register(nameof(Relationship), typeof(RelationshipModel), typeof(AccountView), new PropertyMetadata(null));



        public AccountView()
        {
            this.InitializeComponent();
        }


        void Mention()
        {
            App.StatusAcionHandler.Mention(Account);
        }

        void Report()
        {
            App.StatusAcionHandler.Report(Account);
        }

        async void Block()
        {
            Relationship = await Accounts.Block(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, Account.Id);
        }
        async void UnBlock()
        {
            Relationship = await Accounts.UnBlock(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, Account.Id);
        }

        async void Mute()
        {
            Relationship = await Accounts.Mute(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, Account.Id);
        }
        async void UnMute()
        {
            Relationship = await Accounts.UnMute(Settings.CurrentAccount.Domain, Settings.CurrentAccount.AccessToken, Account.Id);
        }

    }
}