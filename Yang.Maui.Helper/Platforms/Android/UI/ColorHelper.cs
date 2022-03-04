using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics;
using AndroidX.Core.Content;

namespace Yang.Maui.Helper.Platforms.Android.UI
{
    public static class ColorHelper
    {
        /// <summary>
        /// https://stackoverflow.com/a/59845266/13254773
        /// </summary>
        /// <param name="activity"></param>
        public static void SetStatusBarColor(this Activity activity, Color color)
        {

            activity.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            activity.Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            //activity.Window.DecorView.SystemUiVisibility=(View.SYSTEM_UI_FLAG_FULLSCREEN);
            //activity.Window.DecorView.setSystemUiVisibility(View.SYSTEM_UI_FLAG_LIGHT_STATUS_BAR);
            activity.Window.SetStatusBarColor(color);
        }


        /// <summary>
        /// https://stackoverflow.com/a/22192691/13254773
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static int GetStatusBarColor(this Activity activity)
        {
            return ContextCompat.GetColor(activity,activity.Window.StatusBarColor);
        }
    }
}
