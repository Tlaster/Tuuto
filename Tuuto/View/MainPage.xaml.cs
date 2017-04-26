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
using Tuuto.Common;
using Tuuto.ViewModel;
using Tuuto.Common.Controls;

namespace Tuuto.View
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        public MainPage()
        {
            this.InitializeComponent();
        }

        public void RefreshClick()
        {
            fixedPivot.RequestRefresh();
        }
        public void ShowTootDialog()
        {
            new TootDialog().ShowAsync();
        }
    }
}
