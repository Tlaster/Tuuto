using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastodon.Model;
using static Tuuto.Common.Helpers.SettingHelper;

namespace Tuuto.Common
{
    internal class Settings
    {
        public static bool EnableWaterFall
        {
            get => GetSetting(nameof(EnableWaterFall), false);
            set => SetSetting(nameof(EnableWaterFall), value);
        }
        public static int SelectedUserIndex
        {
            get => GetSetting(nameof(SelectedUserIndex), 0);
            set => SetSetting(nameof(SelectedUserIndex), value);
        }

        public static (string Domain, string AccessToken, int Id)[] Account
        {
            get => GetListSetting<string>(nameof(Account)).Select(item => (item.Split(';')[0], item.Split(';')[1], int.Parse(item.Split(';')[2]))).ToArray();
            set => SetListSetting(nameof(Account), value.Select(item => $"{item.Domain};{item.AccessToken};{item.Id}"));
        }
        public static (string Domain, string AccessToken, int Id) CurrentAccount => Account[SelectedUserIndex];

        public static AccountModel CurrentAccountModel { get; internal set; }
    }
}
