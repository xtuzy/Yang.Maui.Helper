#if WINDOWS

using Microsoft.Maui.Devices;

namespace Yang.Maui.Helper.Devices.Screen
{
    public partial class ScreenHelper
    {
        public static float GetScreenDensity()
        {
            return (float)DeviceDisplay.MainDisplayInfo.Density;
        }

        public static float GetScreenHeight()
        {
            return (float)DeviceDisplay.MainDisplayInfo.Height;
        }

        public static DisplayOrientation GetScreenOrientation()
        {
            return DeviceDisplay.MainDisplayInfo.Orientation switch
            {
                DisplayOrientation.Portrait => DisplayOrientation.Portrait,
                DisplayOrientation.Landscape => DisplayOrientation.Landscape,
                DisplayOrientation.Unknown => DisplayOrientation.Unknown,
            };
        }

        public static DisplayRotation GetScreenRotation()
        {
            return DeviceDisplay.MainDisplayInfo.Rotation;
        }

        public static float GetScreenWidth()
        {
            return (float)DeviceDisplay.MainDisplayInfo.Width;
        }

    }
}
#endif