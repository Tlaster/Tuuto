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

namespace Tuuto
{
    sealed partial class App : Application
    {
        private static Compositor _compositor;
        private static SpriteVisual _hostSprite;

        public static IStatusActionHandler StatusAcionHandler { get; internal set; }
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            RequestedTheme = Settings.IsDarkTheme ? ApplicationTheme.Dark : ApplicationTheme.Light;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null && e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                if (DeviceHelper.GetDeviceFormFactorType() == DeviceFormFactorType.Phone)
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
            }
            Window.Current.Activate();
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
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
                else if (DeviceHelper.GetDeviceFormFactorType() == DeviceFormFactorType.Phone)
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