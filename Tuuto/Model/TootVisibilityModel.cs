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
    public class TootVisibilityList
    {
        public static List<TootVisibilityModel> VisibilityList { get; } = new List<TootVisibilityModel>
        {
            new TootVisibilityModel
            {
                Icon = Symbol.World,
                Title = ResourceHelper.GetString("TootVisibility_Public_Title"),
                Description = ResourceHelper.GetString("TootVisibility_Public_Description"),
                VisibilityCode = StatusModel.STATUSVISIBILITY_PUBLIC,
            },
            new TootVisibilityModel
            {
                Icon = Symbol.List,
                Title = ResourceHelper.GetString("TootVisibility_Unlisted_Title"),
                Description = ResourceHelper.GetString("TootVisibility_Unlisted_Description"),
                VisibilityCode = StatusModel.STATUSVISIBILITY_UNLISTED,
            },
            new TootVisibilityModel
            {
                Icon = Symbol.People,
                Title = ResourceHelper.GetString("TootVisibility_Private_Title"),
                Description = ResourceHelper.GetString("TootVisibility_Private_Description"),
                VisibilityCode = StatusModel.STATUSVISIBILITY_PRIVATE,
            },
            new TootVisibilityModel
            {
                Icon = Symbol.Message,
                Title = ResourceHelper.GetString("TootVisibility_Direct_Title"),
                Description = ResourceHelper.GetString("TootVisibility_Direct_Description"),
                VisibilityCode = StatusModel.STATUSVISIBILITY_DIRECT,
            }
        };
    }
    public class TootVisibilityModel
    {
        public Symbol Icon { get; internal set; }
        public string Title { get; internal set; }
        public string Description { get; internal set; }
        public string VisibilityCode { get; set; }
    }
}
