using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tuuto.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Tuuto.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public int SelectedUserIndex
        {
            get => Settings.SelectedUserIndex;
            set { Settings.SelectedUserIndex = value; Window.Current.Content = new ExtendedSplash(null); }
        }
        public SettingPage()
        {
            this.InitializeComponent();
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            App.ReStart();
        }

        void AddUser()
        {
            Frame.Navigate(typeof(LoginPage));
        }
        
        void Logout()
        {
            var account = Settings.Account.ToList();
            account.RemoveAt(Settings.SelectedUserIndex);
            Settings.Account = account.ToArray();
            Settings.SelectedUserIndex = 0;
            App.ReStart();
        }
    }
}
