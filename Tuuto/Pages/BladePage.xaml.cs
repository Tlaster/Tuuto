using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Mastodon.Model;
using Tuuto.Common;
using Tuuto.ViewModel;
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
    public sealed partial class BladePage : Page, IStatusActionHandler
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        public BladePage()
        {
            this.InitializeComponent();
            App.StatusAcionHandler = this;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }

        public void Report(AccountModel account, StatusModel status = null)
        {

        }

        public void Reply(StatusModel model)
        {

        }

        public void Expand(StatusModel model)
        {

        }

        public void Mention(AccountModel model)
        {

        }

        public void AccountDetail(int id)
        {

        }

        public void HashTag(string tag)
        {

        }
    }
}
