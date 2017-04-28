using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Common.Helpers;
using Windows.UI.Xaml.Data;

namespace Tuuto.Common.Converters
{
    class StringFormatFromResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            if (parameter == null)
            {
                return value;
            }

            return string.Format(ResourceHelper.GetString((string)parameter), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
