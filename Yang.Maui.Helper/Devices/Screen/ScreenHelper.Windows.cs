#if WINDOWS
using Microsoft.Maui.Essentials;

namespace Yang.Maui.Helper.Devices.Screen
{
    public partial class ScreenHelper : IScreenHelper
    {
        #region Interface
        public float GetScreenDensity()
        {
           return (float)DeviceDisplay.MainDisplayInfo.Density;
        }

        public float GetScreenHeight(object arg = null)
        {
            return (float)DeviceDisplay.MainDisplayInfo.Height;
        }

        public DisplayOrientation GetScreenOrientation(object arg = null)
        {
            return DeviceDisplay.MainDisplayInfo.Orientation switch
            {
                Microsoft.Maui.Essentials.DisplayOrientation.Portrait => DisplayOrientation.Portrait,
                Microsoft.Maui.Essentials.DisplayOrientation.Landscape => DisplayOrientation.Landscape,
                Microsoft.Maui.Essentials.DisplayOrientation.Unknown => DisplayOrientation.Unknown,
            };
        }

        public DisplayRotation GetScreenRotation(object arg = null)
        {
            return DeviceDisplay.MainDisplayInfo.Rotation switch
            {
                Microsoft.Maui.Essentials.DisplayRotation.Rotation0 => DisplayRotation.Rotation0,
                Microsoft.Maui.Essentials.DisplayRotation.Rotation90 => DisplayRotation.Rotation90,
                Microsoft.Maui.Essentials.DisplayRotation.Rotation180 => DisplayRotation.Rotation180,
                Microsoft.Maui.Essentials.DisplayRotation.Rotation270 => DisplayRotation.Rotation270,
                Microsoft.Maui.Essentials.DisplayRotation.Unknown => DisplayRotation.Unknown,
            };
        }

        public float GetScreenWidth(object arg = null)
        {
            return (float)DeviceDisplay.MainDisplayInfo.Width;
        }

        #endregion
    }
}
#endif