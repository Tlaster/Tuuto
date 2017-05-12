using Mastodon.Model;
using Mastodon.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Nito.Mvvm;
using System.ComponentModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Tuuto.View
{
    public sealed partial class UserAccountsView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AccountModel Account { get; private set; }

        public (string Domain, string AccessToken, int Id) ViewModel
        {
            get { return ((string Domain, string AccessToken, int Id))GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof((string Domain, string AccessToken, int Id)), typeof(UserAccountsView), new PropertyMetadata((string.Empty, string.Empty, -1), OnViewModelChanged));


        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as UserAccountsView).OnViewModelChanged(((string domain, string accessToken, int id))e.NewValue);
        }

        private async void OnViewModelChanged((string domain, string accessToken, int id) newValue)
        {
            if (string.IsNullOrEmpty(newValue.domain)) return;
            Account = await Accounts.Fetching(newValue.domain, newValue.id, newValue.accessToken);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Account)));
        }

        public UserAccountsView()
        {
            this.InitializeComponent();
        }
    }
}
