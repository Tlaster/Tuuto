using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tuuto.Common.Selectors
{
    enum Versions
    {
        RTM = 10240,
        NovemberUpdate = 10586,
        AnniversaryUpdate = 14393,
        CreatorsUpdate = 15063,
    }
    class DataTemplateByVersionSelector : DataTemplateSelector
    {
        public DataTemplate Before { get; set; }
        public DataTemplate After { get; set; }
        public Versions Version { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (SystemInformation.OperatingSystemVersion.Build < (ushort)Version)
            {
                return Before;
            }
            else
            {
                return After;
            }
        }
    }
}
