using Android.Content;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Devices.Screen
{
    public class FPSHelper
    {
        public static float GetFPS(Context context)
        {
            var windowsManager = (IWindowManager)context.GetSystemService(Context.WindowService);
            return windowsManager.DefaultDisplay.RefreshRate;
        }
    }
}
