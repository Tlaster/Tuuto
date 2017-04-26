using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Tuuto.Common.Converters
{
    public class HtmlToMarkdownConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Html2Markdown.Converter.Convert(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
