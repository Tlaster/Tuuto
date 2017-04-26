using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Tuuto.Common.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isInverted = false;
            if (parameter is string)
            {
                bool.TryParse(parameter.ToString(), out isInverted);
            }

            if (value == null || value is string && string.IsNullOrEmpty(value.ToString()) || value is IList && (value as IList).Count == 0)
            {
                return isInverted ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return isInverted ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
