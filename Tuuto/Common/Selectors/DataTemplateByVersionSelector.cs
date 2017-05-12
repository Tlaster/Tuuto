using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Common.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tuuto.Common.Selectors
{
    class DataTemplateByVersionSelector : DataTemplateSelector
    {
        public DataTemplate Before { get; set; }
        public DataTemplate After { get; set; }
        public WindowsVersions Version { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (DeviceHelper.GetCurrentVersion() < Version)
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
