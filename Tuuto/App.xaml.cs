using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Tuuto.Common.Helpers;
using Tuuto.View;
using Tuuto.Model;
using Microsoft.EntityFrameworkCore;
using Tuuto.Common;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Tuuto.Pages;
using Tuuto.Common.Notifications;
using Windows.ApplicationModel.Background;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml.Resources;

namespace Tuuto
{
    sealed partial class App : Application
    {
        public const string BackgroundTask_Notification = nameof(BackgroundTask_Notification);

        private static Compositor _compositor;
        private static SpriteVisual _hostSprite;

        public static IStatusActionHandler StatusAcionHandler { get; internal set; }
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            if (DeviceHelper.CurrentVersion >= WindowsVersions.AnniversaryUpdate)
            {
                EnteredBackground += App_EnteredBackground;
                LeavingBackground += App_LeavingBackground;
            }
            RequestedTheme = Settings.IsDarkTheme ? ApplicationTheme.Dark : ApplicationTheme.Light;
        }

        private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            BackgroundTaskHelper.Unregister(BackgroundTask_Notification);
        }

        private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            BackgroundTaskHelper.Register(BackgroundTask_Notification, new TimeTrigger(15, false));
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null && e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                if (DeviceHelper.DeviceFormFactorType == DeviceFormFactorType.Phone)
                {
                    StatusBar.GetForCurrentView().BackgroundColor = ((SolidColorBrush) Resources["AppTheme"]).Color;
                    StatusBar.GetForCurrentView().BackgroundOpacity = 1d;
                    StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
                }
                else
                {
                    Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
                    ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = ((SolidColorBrush) Resources["AppTheme"]).Color;
                    ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = ((SolidColorBrush) Resources["AppTheme"]).Color;
                    ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(350, 200));
                }
                Window.Current.Content = new ExtendedSplash(e.SplashScreen);
                CustomXamlResourceLoader.Current = new LocalizationResource();
            }
            Window.Current.Activate();
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            if (DeviceHelper.CurrentVersion < WindowsVersions.AnniversaryUpdate) return;
            switch (args.TaskInstance.Task.Name)
            {
                case BackgroundTask_Notification:
                    new NotificationBackgroundTask().Run(args.TaskInstance);
                    break;
                default:
                    break;
            }
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }
        public static void ReStart()
        {
            Window.Current.Content = new ExtendedSplash(null);
        }
        internal static void InitBlurEffect(FrameworkElement element)
        {
            _compositor = ElementCompositionPreview
               .GetElementVisual(element).Compositor;
            element.Loaded += (sender, e) =>
            {
                _hostSprite = _compositor.CreateSpriteVisual();
                _hostSprite.Size = new Vector2(
                    (float)element.ActualWidth,
                    (float)element.ActualHeight);
                ElementCompositionPreview.SetElementChildVisual(
                    element, _hostSprite);
                _hostSprite.Brush = _compositor.CreateHostBackdropBrush();
            };
            element.SizeChanged += (sender, e) =>
            {
                if (_hostSprite != null)
                    _hostSprite.Size = e.NewSize.ToVector2();
            };
        }
        public static void HandleBackButton(Frame rootFrame)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += (sender, e) =>
            {
                if (rootFrame.CanGoBack)
                {
                    e.Handled = true;
                    rootFrame.GoBack();
                }
                else if (DeviceHelper.DeviceFormFactorType == DeviceFormFactorType.Phone)
                {
                    Current.Exit();
                }
            };
            rootFrame.Navigated += (sender, e) =>
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = rootFrame.CanGoBack
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
            };
        }
    }
}