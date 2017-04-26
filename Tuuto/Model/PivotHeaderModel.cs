using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using PropertyChanged;

namespace Tuuto.Model
{
    [ImplementPropertyChanged]
    public class PivotHeaderModel
    {
        public int Badge { get; set; }
        public Symbol Icon { get; set; }
        public string Text { get; set; }
    }
}
