using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using PropertyChanged;
using FontAwesome.UWP;

namespace Tuuto.Model
{
    [ImplementPropertyChanged]
    public class PivotHeaderModel
    {
        public int Badge { get; set; }
        public FontAwesomeIcon Icon { get; set; }
        public string Text { get; set; }
    }
}
