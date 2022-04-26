using Microsoft.Maui.Devices;
using UIKit;

namespace Yang.Maui.Helper.Devices.Screen
{
    public partial class ScreenHelper
    {
        public static float GetScreenDensity()
        {
            return (float)UIScreen.MainScreen.Scale;
        }

        public static float GetScreenHeight(object arg = null)
        {
            return GetScreenDensity() * (float)UIScreen.MainScreen.Bounds.Height;
        }

        public static DisplayOrientation GetScreenOrientation()
        {
            var orientation = UIApplication.SharedApplication.StatusBarOrientation;

            if (orientation.IsLandscape())
                return DisplayOrientation.Landscape;

            return DisplayOrientation.Portrait;
        }

        public static DisplayRotation GetScreenRotation()
        {
            var orientation = UIApplication.SharedApplication.StatusBarOrientation;

            switch (orientation)
            {
                case UIInterfaceOrientation.Portrait:
                    return DisplayRotation.Rotation0;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    return DisplayRotation.Rotation180;
                case UIInterfaceOrientation.LandscapeLeft:
                    return DisplayRotation.Rotation270;
                case UIInterfaceOrientation.LandscapeRight:
                    return DisplayRotation.Rotation90;
            }

            return DisplayRotation.Unknown;
        }

        public static float GetScreenWidth()
        {
            return GetScreenDensity() * (float)UIScreen.MainScreen.Bounds.Width;
        }
    }
}