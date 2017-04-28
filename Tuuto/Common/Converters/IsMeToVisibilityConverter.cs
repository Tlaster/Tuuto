using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Tuuto.Common.Converters
{
    class IsMeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isInverted = false;

            if (parameter is string)
            {
                bool.TryParse(parameter.ToString(), out isInverted);
            }

            if (int.TryParse(value.ToString(), out int result))
            {
                var boolValue = result == Settings.CurrentAccount.Id;

                boolValue = isInverted ? !boolValue : boolValue;

                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return isInverted ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
