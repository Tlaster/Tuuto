using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Tuuto.Common.Helpers
{
    internal static class ResourceHelper
    {
        public static string GetString(string resource)
        {
            return new ResourceLoader().GetString(resource);
        }
    }
}
