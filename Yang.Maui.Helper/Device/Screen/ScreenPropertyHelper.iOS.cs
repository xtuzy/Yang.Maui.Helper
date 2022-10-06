using Microsoft.Maui.Devices;
using UIKit;

namespace Yang.Maui.Helper.Device.Screen
{
    public partial class ScreenPropertyHelper
    {
        public static double GetScreenDensity()
        {
            return UIScreen.MainScreen.Scale.Value;
        }

        public static double GetScreenHeight(object arg = null)
        {
            return GetScreenDensity() * UIScreen.MainScreen.Bounds.Height.Value;
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

        public static double GetScreenWidth()
        {
            return GetScreenDensity() * UIScreen.MainScreen.Bounds.Width.Value;
        }
    }
}