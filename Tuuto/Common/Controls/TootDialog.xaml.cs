using Mastodon.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Tuuto.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using PropertyChanged;
using Windows.Graphics.Imaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.ComponentModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace Tuuto.Common.Controls
{
    public sealed partial class TootDialog : ContentDialog
    {
        public TootDialog()
        {
            this.InitializeComponent();
        }

        private void PostingTootView_CloseRequested(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
