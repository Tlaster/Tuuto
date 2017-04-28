using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tuuto.Common.Helpers;
using Tuuto.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    class PivotHeaderSplitterVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var foreground = value as SolidColorBrush;
            var visiableColor = parameter as SolidColorBrush;
            if (foreground.Color.ToString() == visiableColor.Color.ToString())
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public sealed partial class FixedPivot : Pivot
    {
        private bool _isSelectionChanged = false;
        public List<PivotHeaderModel> Headers { get; private set; }

        public object SplitViewPaneButtomContent
        {
            get { return GetValue(SplitViewPaneButtomProperty); }
            set { SetValue(SplitViewPaneButtomProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SplitViewPaneButtom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SplitViewPaneButtomProperty =
            DependencyProperty.Register(nameof(SplitViewPaneButtomContent), typeof(object), typeof(FixedPivot), new PropertyMetadata(null));



        public double SplitViewPaneLength
        {
            get { return (double)GetValue(SplitViewPaneLengthProperty); }
            set { SetValue(SplitViewPaneLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SplitViewPaneLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SplitViewPaneLengthProperty =
            DependencyProperty.Register(nameof(SplitViewPaneLength), typeof(double), typeof(FixedPivot), new PropertyMetadata(300d));



        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentWidthProperty =
            DependencyProperty.Register(nameof(ContentWidth), typeof(double), typeof(FixedPivot), new PropertyMetadata(0));



        public double ContentHeight
        {
            get { return (double)GetValue(ContentHeightProperty); }
            set { SetValue(ContentHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentHeightProperty =
            DependencyProperty.Register(nameof(ContentHeight), typeof(double), typeof(FixedPivot), new PropertyMetadata(0));
        private SplitView _splitView;
        private ExAdaptiveGridView _fixedHeader;

        public FixedPivot()
        {
            this.InitializeComponent();
            Loaded += FixedPivot_Loaded;
            SelectionChanged += Header_SelectionChanged;
            //Pivot content will out of the window range if not calculate the size
            //TODO: fix pivot content out of the window range with a batter way
            SizeChanged += FixedPivot_SizeChanged;
        }

        private void FixedPivot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_fixedHeader == null || _splitView == null)
            {
                ContentHeight = e.NewSize.Height;
                ContentWidth = e.NewSize.Width;
                return;
            }
            if (e.PreviousSize.Width != e.NewSize.Width)
            {
                RecalculateWidth(e.NewSize.Width);
            }
            if (e.PreviousSize.Height != e.NewSize.Height)
            {
                RecalculateHeight(e.NewSize.Height);
            }
        }

        private void RecalculateWidth(double width)
        {
            if (_splitView.IsPaneOpen)
            {
                ContentWidth = width - _splitView.OpenPaneLength;
            }
            else if (_splitView.DisplayMode == SplitViewDisplayMode.CompactInline)
            {
                ContentWidth = width - _splitView.CompactPaneLength;
            }
            else
            {
                ContentWidth = width;
            }
        }

        private void RecalculateHeight(double height)
        {
            if (_fixedHeader.Visibility == Visibility.Visible)
            {
                ContentHeight = height - _fixedHeader.Height;
            }
            else
            {
                ContentHeight = height;
            }
        }

        private void FixedPivot_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= FixedPivot_Loaded;
            Headers = Items.Where(CheckItem).Select(GetHeader).ToList();
            ExVisualTreeHelper.GetObjectByName<ContentControl>(this, "HeaderClipper").Visibility = Visibility.Collapsed;
            ExVisualTreeHelper.GetObjectByName<ContentPresenter>(this, "LeftHeaderPresenter").Visibility = Visibility.Collapsed;
            ExVisualTreeHelper.GetObjectByName<Button>(this, "PreviousButton").Visibility = Visibility.Collapsed;
            ExVisualTreeHelper.GetObjectByName<Button>(this, "NextButton").Visibility = Visibility.Collapsed;
            ExVisualTreeHelper.GetObjectByName<ContentPresenter>(this, "RightHeaderPresenter").Visibility = Visibility.Collapsed;
            _fixedHeader = ExVisualTreeHelper.GetObjectByName<ExAdaptiveGridView>(this, "FixedHeader");
            _fixedHeader.DataContext = this;
            _splitView = ExVisualTreeHelper.GetObjectByName<SplitView>(this, "splitView");
            _splitView.DataContext = this;
            var presenter = ExVisualTreeHelper.GetObjectByName<ItemsPresenter>(this, "PivotItemPresenter");
            presenter.SetBinding(WidthProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(nameof(ContentWidth)),
                Mode = BindingMode.TwoWay
            });
            presenter.SetBinding(HeightProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(nameof(ContentHeight)),
                Mode = BindingMode.TwoWay
            });
            RecalculateHeight(ContentHeight);
            RecalculateWidth(ContentWidth);
        }

        private bool CheckItem(object item)
        {
            var content = (item as FixedPivotItem);
            return content.Visibility == Visibility.Visible;
        }

        private PivotHeaderModel GetHeader(object item)
        {
            var content = (item as FixedPivotItem);
            content.BadgeChanged += Content_BadgeChanged;
            return new PivotHeaderModel
            {
                Icon = content.HeaderIcon,
                Text = content.HeaderText
            };
        }

        private void Content_BadgeChanged(object sender, int e)
        {
            var index = Items.IndexOf(sender as FixedPivotItem);
            Headers[index].Badge = e;
        }

        private void HeaderList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            RequestRefresh();
        }

        public void RequestRefresh()
        {
            (Items[SelectedIndex] as FixedPivotItem).InvokeRefresh();
        }

        private void HeaderList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isSelectionChanged)
            {
                _isSelectionChanged = false;
            }
            else
            {
                ExVisualTreeHelper.GetObject<ScrollViewer>((Items[SelectedIndex] as FixedPivotItem).Content as DependencyObject)?.ChangeView(0, 0, 1);
            }

        }

        private void Header_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isSelectionChanged = true;
        }
    }
}
