using FontAwesome.UWP;
using Mastodon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Common.Helpers;
using Windows.UI.Xaml.Controls;

namespace Tuuto.Model
{
    class TootVisibilityList
    {
        public static List<TootVisibilityModel> VisibilityList { get; } = new List<TootVisibilityModel>
        {
            new TootVisibilityModel
            {
                Icon = FontAwesomeIcon.Globe,
                Title = ResourceHelper.GetString("TootVisibility_Public_Title"),
                Description = ResourceHelper.GetString("TootVisibility_Public_Description"),
                VisibilityCode = StatusModel.STATUSVISIBILITY_PUBLIC,
            },
            new TootVisibilityModel
            {
                Icon = FontAwesomeIcon.Lock,
                Title = ResourceHelper.GetString("TootVisibility_Unlisted_Title"),
                Description = ResourceHelper.GetString("TootVisibility_Unlisted_Description"),
                VisibilityCode = StatusModel.STATUSVISIBILITY_UNLISTED,
            },
            new TootVisibilityModel
            {
                Icon = FontAwesomeIcon.UnlockAlt,
                Title = ResourceHelper.GetString("TootVisibility_Private_Title"),
                Description = ResourceHelper.GetString("TootVisibility_Private_Description"),
                VisibilityCode = StatusModel.STATUSVISIBILITY_PRIVATE,
            },
            new TootVisibilityModel
            {
                Icon = FontAwesomeIcon.Envelope,
                Title = ResourceHelper.GetString("TootVisibility_Direct_Title"),
                Description = ResourceHelper.GetString("TootVisibility_Direct_Description"),
                VisibilityCode = StatusModel.STATUSVISIBILITY_DIRECT,
            }
        };
    }
    class TootVisibilityModel
    {
        public FontAwesomeIcon Icon { get; internal set; }
        public string Title { get; internal set; }
        public string Description { get; internal set; }
        public string VisibilityCode { get; set; }
    }
}
