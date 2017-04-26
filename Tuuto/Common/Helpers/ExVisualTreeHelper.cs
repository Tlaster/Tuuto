using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Tuuto.Common.Helpers
{
    internal static class ExVisualTreeHelper
    {
        public static T GetObject<T>(DependencyObject o) where T : DependencyObject
        {
            if (o == null)
            {
                return null;
            }

            if (o is T)
            {
                return o as T;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);

                var result = GetObject<T>(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;
        }

        public static T GetObjectByName<T>(DependencyObject o, string name) where T : DependencyObject
        {
            if (o == null)
            {
                return null;
            }

            if (o is T && o.GetValue(FrameworkElement.NameProperty) as string == name)
            {
                return o as T;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);

                var result = GetObjectByName<T>(child, name);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;
        }
    }
}
