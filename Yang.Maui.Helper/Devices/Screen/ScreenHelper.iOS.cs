using UIKit;

namespace Yang.Maui.Helper.Devices.Screen
{
    public partial class ScreenHelper:IScreenHelper
    {
        public float GetScreenDensity()
        {
            return (float)UIScreen.MainScreen.Scale;
        }

        public float GetScreenHeight(object arg = null)
        {
            return GetScreenDensity()* (float)UIScreen.MainScreen.Bounds.Height;
        }

        public DisplayOrientation GetScreenOrientation(object arg = null)
        {
            var orientation = UIApplication.SharedApplication.StatusBarOrientation;

            if (orientation.IsLandscape())
                return DisplayOrientation.Landscape;

            return DisplayOrientation.Portrait;
        }

        public DisplayRotation GetScreenRotation(object arg = null)
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

        public float GetScreenWidth(object arg = null)
        {
            return GetScreenDensity()* (float)UIScreen.MainScreen.Bounds.Width;
        }
    }
}