using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Controls;
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
using System.Reflection;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Tuuto.Common.Controls
{
    [TemplatePart(Name = "FailedView", Type = typeof(Border))]
    [TemplatePart(Name = "EmptyView", Type = typeof(Border))]
    public partial class ExListView : PullToRefreshListView
    {
        //TODO: Show progressbar
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(ExListView), new PropertyMetadata(false));

        public bool IsError
        {
            get { return (bool)GetValue(LoadErrorProperty); }
            set { SetValue(LoadErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadErrorProperty =
            DependencyProperty.Register(nameof(IsError), typeof(bool), typeof(ExListView), new PropertyMetadata(false, OnIsErrorChanged));

        private static void OnIsErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ExListView).OnIsErrorChanged((bool)e.NewValue);
        }

        private void OnIsErrorChanged(bool newValue)
        {
            _failedView.Visibility = newValue && !Items.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private Border _failedView;
        private Border _emptyView;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _emptyView = GetTemplateChild("EmptyView") as Border;
            _failedView = GetTemplateChild("FailedView") as Border;
        }
        

        public ExListView()
        {
            this.InitializeComponent();
            Items.VectorChanged += Items_VectorChanged;
        }

        private void Items_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        {
            if (_emptyView != null)
                _emptyView.Visibility = sender.Any() && !IsError || !IsLoading ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
