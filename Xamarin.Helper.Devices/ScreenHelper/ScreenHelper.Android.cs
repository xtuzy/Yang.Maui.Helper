/*
 * 作者：
 * 创建日期：
 * 修改记录：
 */
#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using System;
using PM = Android.Content.PM;
using Res = Android.Content.Res;
/**
 * <pre>
 *     author: Blankj
 *     blog  : http://blankj.com
 *     time  : 2016/08/02
 *     desc  : utils about screen
 * </pre>
 */

namespace Xamarin.Helper.Devices
{
    /// <summary>
    /// 屏幕适配相关方法
    /// 1.屏幕密度获取
    /// 2.屏幕大小获取
    /// 3.屏幕单位换算
    /// <a href="https://github.com/Blankj/AndroidUtilCode/blob/master/lib/utilcode/src/main/java/com/blankj/utilcode/util/ScreenUtils.java">从AndroidUtilCode ScreenUtils修改</a>
    /// </summary>
    public static partial class ScreenHelper
    {
        /// <summary>
        /// Return the width of screen, in pixel.
        /// <a href="https://stackoverflow.com/questions/4743116/get-screen-width-and-height-in-android">Get screen width and height in Android</a>
        /// </summary>
        public static int GetScreenPixelWidth(Context context)
        {
            DisplayMetrics displayMetrics;
            Display display;
#if __ANDROID_30__
            if ((int)Build.VERSION.SdkInt >= 30)//BuildVersionCodes.R,在bushu目标小于Android11时没有R
            {
                displayMetrics = new DisplayMetrics();
                display = context.Display;
                if (display != null)
                {
                    display.GetRealMetrics(displayMetrics);
                    return displayMetrics.WidthPixels;
                }
            }
#endif

            displayMetrics = new DisplayMetrics();
            IWindowManager wm = (IWindowManager)context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            display = wm.DefaultDisplay;
            display.GetRealMetrics(displayMetrics);

            //int height = displayMetrics.heightPixels;
            //int width = displayMetrics.widthPixels;
            return displayMetrics.WidthPixels;
        }

        /// <summary>
        /// Return the height of screen, in pixel.
        /// <a href="https://stackoverflow.com/questions/4743116/get-screen-width-and-height-in-android">Get screen width and height in Android</a>
        /// </summary>
        /// <returns></returns>
        public static int GetScreenPixelHeight(Context context)
        {
            DisplayMetrics displayMetrics;
            Display display;
#if __ANDROID_30__
            if ((int)Build.VERSION.SdkInt >= 30)//API30
            {
                displayMetrics = new DisplayMetrics();
                display = context.Display;
                if (display != null)
                {
                    display.GetRealMetrics(displayMetrics);
                    return displayMetrics.HeightPixels;
                }
            }
#endif
            displayMetrics = new DisplayMetrics();
            IWindowManager wm = (IWindowManager)context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            display = wm.DefaultDisplay;
            display.GetRealMetrics(displayMetrics);

            //int height = displayMetrics.heightPixels;
            //int width = displayMetrics.widthPixels;
            return displayMetrics.HeightPixels;
        }

        ///**
        // * Return the application's width of screen, in pixel.
        // *
        // * @return the application's width of screen, in pixel
        // */
        //public static int getAppScreenWidth()
        //{
        //    WindowManager wm = (WindowManager)Utils.getApp().getSystemService(Context.WINDOW_SERVICE);
        //    if (wm == null)
        //        return -1;
        //    Point point = new Point();
        //    wm.getDefaultDisplay().getSize(point);
        //    return point.x;
        //}

        ///**
        // * Return the application's height of screen, in pixel.
        // *
        // * @return the application's height of screen, in pixel
        // */
        //public static int getAppScreenHeight()
        //{
        //    WindowManager wm = (WindowManager)Utils.getApp().getSystemService(Context.WINDOW_SERVICE);
        //    if (wm == null)
        //        return -1;
        //    Point point = new Point();
        //    wm.getDefaultDisplay().getSize(point);
        //    return point.y;
        //}


        /// <summary>
        /// Return the density of screen.
        /// </summary>
        public static float GetScreenDensity()
        {
            return Resources.System.DisplayMetrics.Density;
        }

        /// <summary>
        /// Return the screen density expressed as dots-per-inch.
        /// </summary>
        /// <returns></returns>
        public static int GetScreenDensityDpi()
        {
            return (int)Resources.System.DisplayMetrics.DensityDpi;
        }

        /// <summary>
        /// Set full screen.
        /// </summary>
        /// <param name="activity"></param>
        public static void SetFullScreen(Activity activity)
        {
            activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }

        /// <summary>
        /// Set non full screen.
        /// </summary>
        /// <param name="activity"></param>
        public static void SetNonFullScreen(Activity activity)
        {
            activity.Window.ClearFlags(WindowManagerFlags.Fullscreen);
        }

        /// <summary>
        /// Toggle full screen.
        /// </summary>
        /// <param name="activity"></param>
        public static void ToggleFullScreen(Activity activity)
        {
            bool isFullScreen = IsFullScreen(activity);
            if (isFullScreen)
            {
                SetNonFullScreen(activity);
            }
            else
            {
                SetFullScreen(activity);
            }
        }

        /// <summary>
        /// Return whether screen is full.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool IsFullScreen(Activity activity)
        {
            var fullScreenFlag = WindowManagerFlags.Fullscreen;
            return (activity.Window.Attributes.Flags & fullScreenFlag) == fullScreenFlag;
        }

        /// <summary>
        /// Set the screen to landscape横屏.
        /// </summary>
        /// <param name="activity"></param>
        public static void SetLandscape(Activity activity)
        {
            activity.RequestedOrientation = PM.ScreenOrientation.Landscape;
        }

        /// <summary>
        /// Set the screen to portrait竖屏.
        /// </summary>
        public static void SetPortrait(Activity activity)
        {
            activity.RequestedOrientation = PM.ScreenOrientation.Portrait;
        }

        /// <summary>
        /// Return whether screen is landscape.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsLandscape(Context context)
        {
            return context.Resources.Configuration.Orientation
                    == Res.Orientation.Landscape;
        }

        /// <summary>
        /// Return whether screen is portrait.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsPortrait(Context context)
        {
            return context.Resources.Configuration.Orientation
                    == Res.Orientation.Portrait;
        }

        /// <summary>
        /// Return the rotation of screen.
        /// </summary>
        /// <param name="final"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static int GetScreenRotation(Activity activity)
        {
            switch (activity.WindowManager.DefaultDisplay.Rotation)
            {
                case SurfaceOrientation.Rotation0:
                    return 0;

                case SurfaceOrientation.Rotation90:
                    return 90;

                case SurfaceOrientation.Rotation180:
                    return 180;

                case SurfaceOrientation.Rotation270:
                    return 270;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Return the bitmap of screen.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        [Obsolete("deprecated", true)]
        public static Bitmap ScreenShot(Activity activity)
        {
            return ScreenShot(activity, false);
        }

        /**
         *
         * Return the bitmap of screen.
         * @param activity          The activity.
         * @param isDeleteStatusBar True to delete status bar, false otherwise.
         * @return the bitmap of screen
         */

        [Obsolete("deprecated", true)]
        public static Bitmap ScreenShot(Activity activity, bool isDeleteStatusBar)
        {
            View decorView = activity.Window.DecorView;
            Bitmap bmp = View2Bitmap(decorView);
            DisplayMetrics dm = new DisplayMetrics
            {
                WidthPixels = GetScreenPixelWidth(activity),
                HeightPixels = GetScreenPixelHeight(activity)
            };
            if (isDeleteStatusBar)
            {
                int statusBarHeight = GetStatusBarHeight((Context)activity);
                return Bitmap.CreateBitmap(
                        bmp,
                        0,
                        statusBarHeight,
                        dm.WidthPixels,
                        dm.HeightPixels - statusBarHeight
                );
            }
            else
            {
                return Bitmap.CreateBitmap(bmp, 0, 0, dm.WidthPixels, dm.HeightPixels);
            }
        }

        /// <summary>
        ///  Return the status bar's height.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static int GetStatusBarHeight(Context context)
        {
            Resources resources = context.Resources;
            int resourceId = resources.GetIdentifier("status_bar_height", "dimen", "android");
            try
            {
                return resources.GetDimensionPixelSize(resourceId);
            }
            catch (Exception e)
            {
                return 0;//找不到状态栏就返回0
            }
        }


        [Obsolete("deprecated", true)]
        private static Bitmap View2Bitmap(View view)
        {
            if (view == null)
                return null;
            bool drawingCacheEnabled = view.DrawingCacheEnabled;
            bool willNotCacheDrawing = view.WillNotCacheDrawing();
            view.DrawingCacheEnabled = (true);
            view.SetWillNotCacheDrawing(false);
            Bitmap drawingCache = view.GetDrawingCache(true);
            Bitmap bitmap;
            if (null == drawingCache || drawingCache.IsRecycled)
            {
                view.Measure(View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
                        View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                view.Layout(0, 0, view.MeasuredWidth, view.MeasuredHeight);
                view.BuildDrawingCache();
                drawingCache = view.GetDrawingCache(true);
                if (null == drawingCache || drawingCache.IsRecycled)
                {
                    bitmap = Bitmap.CreateBitmap(view.MeasuredWidth, view.MeasuredHeight, Bitmap.Config.Rgb565);
                    Canvas canvas = new Canvas(bitmap);
                    view.Draw(canvas);
                }
                else
                {
                    bitmap = Bitmap.CreateBitmap(drawingCache);
                }
            }
            else
            {
                bitmap = Bitmap.CreateBitmap(drawingCache);
            }
            view.SetWillNotCacheDrawing(willNotCacheDrawing);
            view.DrawingCacheEnabled = (drawingCacheEnabled);
            return bitmap;
        }

        /// <summary>
        /// Return whether screen is locked.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Obsolete("deprecated", true)]
        public static bool IsScreenLock(Context context)
        {
            KeyguardManager km =
                    (KeyguardManager)context.GetSystemService(Context.KeyguardService);
            if (km == null)
                return false;
            return km.InKeyguardRestrictedInputMode();
        }

        /**
         * Set the duration of sleep.
         * <p>Must hold {@code <uses-permission android:name="android.permission.WRITE_SETTINGS" />}</p>
         *
         * @param duration The duration.
         */
        /*public static void setSleepDuration(int duration)
        {
            Settings.System.putInt(
                    Utils.getApp().getContentResolver(),
                    Settings.System.SCREEN_OFF_TIMEOUT,
                    duration
            );
        }*/

        /**
         * Return the duration of sleep.
         *
         * @return the duration of sleep.
         */
        /*public static int getSleepDuration()
        {
            try
            {
                return Settings.System.getInt(
                        Utils.getApp().getContentResolver(),
                        Settings.System.SCREEN_OFF_TIMEOUT
                );
            }
            catch (Settings.SettingNotFoundException e)
            {
                e.printStackTrace();
                return -123;
            }
        }*/
    }
}
#endif
