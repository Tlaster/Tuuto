using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Common.Helpers;
using Windows.UI.Xaml;

namespace Tuuto.Common.Extensions
{
    public class XamlExtensions : DependencyObject
    {
        public static void SetVisiableAfter(UIElement element, WindowsVersions value)
        {
            element.SetValue(VisiableAfterProperty, value);
        }

        public static WindowsVersions GetVisiableAfter(UIElement element)
        {
            return (WindowsVersions)element.GetValue(VisiableAfterProperty);
        }


        // Using a DependencyProperty as the backing store for VisiableAfter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisiableAfterProperty =
            DependencyProperty.Register("VisiableAfter", typeof(WindowsVersions), typeof(XamlExtensions), new PropertyMetadata(WindowsVersions.RTM, OnVisiableAfterChanged));

        private static void OnVisiableAfterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement)
            {
                (d as UIElement).Visibility = (WindowsVersions)e.NewValue > DeviceHelper.GetCurrentVersion() ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        
        public static void SetCollapseAfter(UIElement element, WindowsVersions value)
        {
            element.SetValue(CollapseAfterProperty, value);
        }

        public static WindowsVersions GetCollapseAfter(UIElement element)
        {
            return (WindowsVersions)element.GetValue(CollapseAfterProperty);
        }

        // Using a DependencyProperty as the backing store for CollapseAfter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollapseAfterProperty =
            DependencyProperty.Register("CollapseAfter", typeof(WindowsVersions), typeof(XamlExtensions), new PropertyMetadata(WindowsVersions.RTM, OnCollapseAfterChanged));

        private static void OnCollapseAfterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement)
            {
                (d as UIElement).Visibility = (WindowsVersions)e.NewValue <= DeviceHelper.GetCurrentVersion() ? Visibility.Collapsed : Visibility.Visible;
            }
        }
    }
}
