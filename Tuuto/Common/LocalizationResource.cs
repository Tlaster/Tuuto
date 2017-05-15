using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Common.Helpers;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Resources;

namespace Tuuto.Common
{
    public class LocalizationResource : CustomXamlResourceLoader
    {
        protected override object GetResource(string resourceId, string objectType, string propertyName, string propertyType)
        {
            return ResourceHelper.GetString(resourceId);
        }
    }
}
