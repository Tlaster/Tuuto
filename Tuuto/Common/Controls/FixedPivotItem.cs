using FontAwesome.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tuuto.Common.Controls
{
    public class FixedPivotItem : PivotItem
    {
        internal event EventHandler<int> BadgeChanged;
        public event EventHandler RefreshRequested;

        public FontAwesomeIcon HeaderIcon
        {
            get { return (FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(HeaderIcon), typeof(FontAwesomeIcon), typeof(FixedPivotItem), new PropertyMetadata(FontAwesomeIcon.SmileOutline));



        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(FixedPivotItem), new PropertyMetadata(""));




        public int Badge
        {
            get { return (int)GetValue(BadgeProperty); }
            set { SetValue(BadgeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Badge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BadgeProperty =
            DependencyProperty.Register(nameof(Badge), typeof(int), typeof(FixedPivotItem), new PropertyMetadata(0, OnBadgeChanged));

        private static void OnBadgeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FixedPivotItem).OnBadgeChanged((int)e.NewValue);
        }

        private void OnBadgeChanged(int newValue)
        {
            BadgeChanged?.Invoke(this, newValue);
        }

        internal void InvokeRefresh()
        {
            RefreshRequested?.Invoke(this, null);
        }
    }
}
