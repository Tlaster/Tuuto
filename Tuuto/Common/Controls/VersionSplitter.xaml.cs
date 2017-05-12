using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tuuto.Common.Helpers;
using Tuuto.Common.Selectors;
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

namespace Tuuto.Common.Controls
{
    public sealed partial class VersionSplitter : ContentPresenter
    {
        public DataTemplate AfterTemplate
        {
            get { return (DataTemplate)GetValue(AfterTemplateProperty); }
            set { SetValue(AfterTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AfterTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AfterTemplateProperty =
            DependencyProperty.Register(nameof(AfterTemplate), typeof(DataTemplate), typeof(VersionSplitter), new PropertyMetadata(null, OnAfterTemplateChanged));

        private static void OnAfterTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VersionSplitter).InitSelector();
        }


        public DataTemplate BeforeTemplate
        {
            get { return (DataTemplate)GetValue(BeforeTemplateProperty); }
            set { SetValue(BeforeTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BeforeTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BeforeTemplateProperty =
            DependencyProperty.Register(nameof(BeforeTemplate), typeof(DataTemplate), typeof(VersionSplitter), new PropertyMetadata(null, OnBeforeTemplateChanged));

        private static void OnBeforeTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VersionSplitter).InitSelector();
        }

        public WindowsVersions Version
        {
            get { return (WindowsVersions)GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Version.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VersionProperty =
            DependencyProperty.Register(nameof(Version), typeof(WindowsVersions), typeof(VersionSplitter), new PropertyMetadata(WindowsVersions.RTM, OnVersionChanged));

        private static void OnVersionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VersionSplitter).InitSelector();
        }

        public VersionSplitter()
        {
            this.InitializeComponent();
        }

        private void InitSelector()
        {
            if (BeforeTemplate == null || AfterTemplate == null || Version == 0)
                return;
            ContentTemplateSelector = new DataTemplateByVersionSelector
            {
                Before = BeforeTemplate,
                After = AfterTemplate,
                Version = Version
            };
        }
        
    }
}
