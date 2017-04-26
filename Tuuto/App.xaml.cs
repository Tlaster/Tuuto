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

namespace Tuuto
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
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