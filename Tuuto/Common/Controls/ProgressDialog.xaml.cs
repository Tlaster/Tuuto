using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PropertyChanged;
using Tuuto.Common.Helpers;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace Tuuto.Common.Controls
{
    [ImplementPropertyChanged]
    public sealed partial class ProgressDialog : ContentDialog, IDisposable
    {
        public double ProgressValue { get; set; } = 0d;
        public double ProgressMaximum { get; set; } = 100d;
        public bool IsIndeterminate { get; set; } = true;
        public string DialogText { get; set; }
        public ProgressDialog(string dialogTextResource = "ProgressDialogText")
        {
            this.InitializeComponent();
            DialogText = ResourceHelper.GetString(dialogTextResource);
            ShowAsync();
        }

        public void Dispose()
        {
            Hide();
        }
    }
}
