using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Profile;
using Windows.UI.ViewManagement;

namespace Tuuto.Common.Helpers
{
    internal static class DeviceHelper
    {
        public static DeviceFormFactorType DeviceFormFactorType
        {
            get
            {
                switch (AnalyticsInfo.VersionInfo.DeviceFamily)
                {
                    case "Windows.Mobile":
                        return DeviceFormFactorType.Phone;
                    case "Windows.Desktop":
                        return UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Mouse
                            ? DeviceFormFactorType.Desktop
                            : DeviceFormFactorType.Tablet;
                    case "Windows.Universal":
                        return DeviceFormFactorType.IoT;
                    case "Windows.Team":
                        return DeviceFormFactorType.SurfaceHub;
                    default:
                        return DeviceFormFactorType.Other;
                }
            }
        }

        public static WindowsVersions CurrentVersion => (WindowsVersions)SystemInformation.OperatingSystemVersion.Build;

    }
    public enum WindowsVersions
    {
        RTM = 10240,
        NovemberUpdate = 10586,
        AnniversaryUpdate = 14393,
        CreatorsUpdate = 15063,
        Insider = 16193,
    }
    enum DeviceFormFactorType
    {
        Phone,
        Desktop,
        Tablet,
        IoT,
        SurfaceHub,
        Other
    }
}
