using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Mastodon.Api;
using Tuuto.Common;
using Tuuto.Common.Controls;
using Tuuto.Common.Helpers;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Tuuto.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public string Domain { get; set; }
        public LoginPage()
        {
            this.InitializeComponent();
        }

        public async void LoginClick()
        {
            using (new ProgressDialog())
            {
                try
                {
                    if (string.IsNullOrEmpty(Domain))
                        throw new UriFormatException();
                    var match = Regex.Match(Domain, "(http://|https://)?([^/]*)(/)?").Groups[2];
                    if (!match.Success)
                        throw new UriFormatException();
                    var domain = match.Value;
                    var auth = await Apps.Register(domain, "Tuuto", "https://github.com/Tlaster/Tuuto", "https://github.com/Tlaster/Tuuto", Apps.SCOPE_READ, Apps.SCOPE_WRITE, Apps.SCOPE_FOLLOW);
                    var url = $"https://{domain}/oauth/authorize?client_id={auth.ClientId}&response_type=code&redirect_uri={auth.RedirectUri}&scope={Apps.SCOPE_READ} {Apps.SCOPE_WRITE} {Apps.SCOPE_FOLLOW}";
                    var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(url), new Uri(auth.RedirectUri));
                    if (result.ResponseStatus != WebAuthenticationStatus.Success) return;
                    var code = Regex.Match(result.ResponseData, "code=(.*)").Groups[1].Value;
                    if (string.IsNullOrEmpty(code))
                        throw new UnauthorizedAccessException();
                    var token = await OAuth.GetAccessTokenByCode(domain, auth.ClientId, auth.ClientSecret, auth.RedirectUri, code, Apps.SCOPE_READ, Apps.SCOPE_WRITE, Apps.SCOPE_FOLLOW);
                    var account = await Accounts.VerifyCredentials(domain, token.AccessToken);
                    Settings.Account = new List<(string, string, int)>(Settings.Account)
                    {
                        (domain, token.AccessToken, account.Id) 
                    }.ToArray();
                    Settings.SelectedUserIndex = Settings.Account.Length - 1;
                    Window.Current.Content = new ExtendedSplash(null);
                }
                catch (UriFormatException)
                {
                    Notification.Show(ResourceHelper.GetString("LoginPage_DomainError"));
                }
                catch (UnauthorizedAccessException)
                {
                    Notification.Show(ResourceHelper.GetString("LoginPage_LoginFailed"));
                }
                catch (Exception e) when(e is FileNotFoundException /*user can not connect to the auth page and close the auth dialog*/ || e is HttpRequestException || e is WebException)
                {
                    
                }
            }

        }
    }
}
