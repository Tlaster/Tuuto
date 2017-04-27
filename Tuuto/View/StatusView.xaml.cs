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
            DependencyProperty.Register(nameof(ViewModel), typeof(StatusModel), typeof(StatusView), new PropertyMetadata(null, OnViewModelChanged));

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as StatusView).OnViewModelChanged(e.NewValue as StatusModel);
        }

        private void OnViewModelChanged(StatusModel statusModel)
        {
            var model = statusModel?.Reblog ?? statusModel;
            if (model == null)
                return;
            StatusViewMenu_Mention.Text = $"{ResourceHelper.GetString("StatusViewMenu_Mention")} @{model.Account.UserName}";
            StatusViewMenu_Mute.Text = $"{ResourceHelper.GetString("StatusViewMenu_Mute")} @{model.Account.UserName}";
            StatusViewMenu_Block.Text = $"{ResourceHelper.GetString("StatusViewMenu_Block")} @{model.Account.UserName}";
            StatusViewMenu_Report.Text = $"{ResourceHelper.GetString("StatusViewMenu_Report")} @{model.Account.UserName}";
        }



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
            var url = new Uri(e.Link);
            if (url.Host == new Uri(ViewModel.Account.Url).Host || url.Host == new Uri(Settings.CurrentAccountModel.Url).Host)
            {
                //TODO:Parse content
            }
            else
            {
                await Launcher.LaunchUriAsync(url);
            }
        }

        private void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void More_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.CurrentAccount.Id == ((sender as FrameworkElement).DataContext as StatusModel).Account.Id)
            {
                MenuMe.ShowAt(sender as FrameworkElement);
            }
            else
            {
                MenuOthers.ShowAt(sender as FrameworkElement);
            }
        }
    }
}
