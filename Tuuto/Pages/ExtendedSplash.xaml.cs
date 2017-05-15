using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Tuuto.Common;
using Tuuto.Common.Helpers;
using Tuuto.Model;
using Microsoft.EntityFrameworkCore;
using Tuuto.View;
using Windows.UI.Xaml.Media;
using FluentScheduler;
using Tuuto.Common.Notifications;

namespace Tuuto.Pages
{
    public sealed partial class ExtendedSplash
    {
        private static SplashScreen _splash;
        private readonly Frame _rootFrame = new Frame();
        private readonly double _scaleFactor;
        private Rect _splashImageRect;

        public ExtendedSplash(SplashScreen splashscreen)
        {
            InitializeComponent();
            Window.Current.SizeChanged += ExtendedSplash_OnResize;
            if (_splash == null && splashscreen != null)
                _splash = splashscreen;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Collapsed;
            _scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            if (_splash != null)
            {
                _splash.Dismissed += DismissedEventHandler;
                _splashImageRect = _splash.ImageLocation;
                PositionImage();
            }
            InitTransitions();
        }


        private void InitTransitions()
        {
            _rootFrame.ContentTransitions = new TransitionCollection
            {
                new NavigationThemeTransition { DefaultNavigationTransitionInfo = new DrillInNavigationTransitionInfo() }
            };
        }

        private void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, _splashImageRect.Left);
            extendedSplashImage.SetValue(Canvas.TopProperty, _splashImageRect.Top);
            if (DeviceHelper.DeviceFormFactorType == DeviceFormFactorType.Phone)
            {
                extendedSplashImage.Height = _splashImageRect.Height / _scaleFactor;
                extendedSplashImage.Width = _splashImageRect.Width / _scaleFactor;
            }
            else
            {
                extendedSplashImage.Height = _splashImageRect.Height;
                extendedSplashImage.Width = _splashImageRect.Width;
            }
        }


        private void ExtendedSplash_OnResize(object sender, WindowSizeChangedEventArgs e)
        {
            if (_splash == null) return;
            _splashImageRect = _splash.ImageLocation;
            PositionImage();
        }

        private async void DismissedEventHandler(SplashScreen sender, object e)
        {
            _splash.Dismissed -= DismissedEventHandler;
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Window.Current.SizeChanged -= ExtendedSplash_OnResize;
                    DismissExtendedSplash();
                });
        }

        private async void DismissExtendedSplash()
        {
            using (var db = new DraftDbContext())
            {
                await db.Database.MigrateAsync();
            }

            if (await CheckForLogin())
            {
                JobManager.Initialize(new NotificationRegistry());
                if (Settings.EnableBladeView)
                {
                    _rootFrame.Navigate(typeof(BladePage));
                }
                else
                {
                    App.StatusAcionHandler = new StatusActionHandler(_rootFrame);
                    _rootFrame.Navigate(typeof(MainPage));
                }
            }
            else
            {
                _rootFrame.Navigate(typeof(LoginPage));
            }

            if (false)//Blur Effect
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                var rootGrid = new Grid();
                var blurBorder = new Border();
                rootGrid.Children.Add(blurBorder);
                rootGrid.Children.Add(_rootFrame);
                Window.Current.Content = rootGrid;
                App.InitBlurEffect(blurBorder);
            }
            else
            {
                Window.Current.Content = _rootFrame;
            }
            App.HandleBackButton(_rootFrame);

        }

        private async Task<bool> CheckForLogin()
        {
            try
            {
                do
                {
                    var account = Settings.Account[Settings.SelectedUserIndex];
                    if (string.IsNullOrEmpty(account.AccessToken))
                        throw new ArgumentNullException();
                    var accountModel = await Mastodon.Api.Accounts.VerifyCredentials(account.Domain, account.AccessToken);
                    if (accountModel != null)
                    {
                        Settings.CurrentAccountModel = accountModel;
                        break;
                    }
                    var list = SettingHelper.GetListSetting<string>(nameof(Settings.Account), isThrowException: true).ToList();
                    list.RemoveAt(Settings.SelectedUserIndex);
                    Settings.SelectedUserIndex = 0;
                    SettingHelper.SetListSetting(nameof(Settings.Account), list);
                    if (list.Count == 0)
                        return false;
                } while (true);
                return true;
            }
            catch (Exception e) when (e is WebException || e is HttpRequestException)
            {
                return false;
            }
            catch (Exception e)// when (e is SettingException)
            {
                return false;
            }
        }
    }
}