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

namespace Yang.Maui.Helper.Devices.Screen
{
    /// <summary>
    /// get Screen info
    /// 1.size
    /// 2.density
    /// <a href="https://github.com/Blankj/AndroidUtilCode/blob/master/lib/utilcode/src/main/java/com/blankj/utilcode/util/ScreenUtils.java">???AndroidUtilCode ScreenUtils??????</a>
    /// </summary>
    public partial class ScreenHelper:IScreenHelper
    {
        /// <summary>
        /// Return the width of screen, in pixel.
        /// <a href="https://stackoverflow.com/questions/4743116/get-screen-width-and-height-in-android">Get screen width and height in Android</a>
        /// </summary>
        /// <returns>px</returns>
        public int GetScreenPixelWidth(Context context)
        {
            DisplayMetrics displayMetrics;
            Display display;
#if __ANDROID_30__
            if ((int)Build.VERSION.SdkInt >= 30)//BuildVersionCodes.R,???bushu????????????Android11?????????R
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
        /// <returns>px</returns>
        public int GetScreenPixelHeight(Context context)
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
        /// Return the screen density expressed as dots-per-inch,
        /// such as 120,160,180,240...
        /// </summary>
        /// <returns></returns>
        public int GetScreenDensityDpi()
        {
            return (int)Resources.System.DisplayMetrics.DensityDpi;
        }

        /// <summary>
        /// Set full screen.
        /// </summary>
        /// <param name="activity"></param>
        public void SetFullScreen(Activity activity)
        {
            activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }

        /// <summary>
        /// Set non full screen.
        /// </summary>
        /// <param name="activity"></param>
        public void SetNoFullScreen(Activity activity)
        {
            activity.Window.ClearFlags(WindowManagerFlags.Fullscreen);
        }

        /// <summary>
        /// Toggle full screen.
        /// </summary>
        /// <param name="activity"></param>
        public void ToggleFullScreen(Activity activity)
        {
            bool isFullScreen = IsFullScreen(activity);
            if (isFullScreen)
            {
                SetNoFullScreen(activity);
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
        public bool IsFullScreen(Activity activity)
        {
            var fullScreenFlag = WindowManagerFlags.Fullscreen;
            return (activity.Window.Attributes.Flags & fullScreenFlag) == fullScreenFlag;
        }

        /// <summary>
        /// Set the screen to landscape.
        /// </summary>
        /// <param name="activity"></param>
        public void SetLandscape(Activity activity)
        {
            activity.RequestedOrientation = PM.ScreenOrientation.Landscape;
        }

        /// <summary>
        /// Set the screen to portrait.
        /// </summary>
        public void SetPortrait(Activity activity)
        {
            activity.RequestedOrientation = PM.ScreenOrientation.Portrait;
        }

        /// <summary>
        /// Return whether screen is landscape.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsLandscape(Context context)
        {
            return context.Resources.Configuration.Orientation
                    == Res.Orientation.Landscape;
        }

        /// <summary>
        /// Return whether screen is portrait.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsPortrait(Context context)
        {
            return context.Resources.Configuration.Orientation
                    == Res.Orientation.Portrait;
        }

        
        /// <summary>
        /// Return the bitmap of screen.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        [Obsolete("deprecated", true)]
        public Bitmap ScreenShot(Activity activity)
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
        public Bitmap ScreenShot(Activity activity, bool isDeleteStatusBar)
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
        public int GetStatusBarHeight(Context context)
        {
            Resources resources = context.Resources;
            int resourceId = resources.GetIdentifier("status_bar_height", "dimen", "android");
            try
            {
                return resources.GetDimensionPixelSize(resourceId);
            }
            catch (Exception e)
            {
                return 0;//if can't find 0
            }
        }


        [Obsolete("deprecated", true)]
        private Bitmap View2Bitmap(View view)
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
        public bool IsScreenLock(Context context)
        {
            KeyguardManager km =
                    (KeyguardManager)context.GetSystemService(Context.KeyguardService);
            if (km == null)
                return false;
            return km.InKeyguardRestrictedInputMode();
        }

        #region Interface
        /// <summary>
        /// Return the density of screen,
        /// such as 1,1.5,2...
        /// </summary>
        public float GetScreenDensity()
        {
            UnitHelper.density = Resources.System.DisplayMetrics.Density;
            return UnitHelper.density;
        }


        /// <summary>
        /// Return the rotation of screen.
        /// </summary>
        /// <param name="final"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public DisplayOrientation GetScreenOrientation(object arg)
        {
            var activity = arg as Context;
            return activity.Resources.Configuration.Orientation switch
            {
                Res.Orientation.Landscape => DisplayOrientation.Landscape,
                Res.Orientation.Portrait => DisplayOrientation.Portrait,
                Res.Orientation.Square => DisplayOrientation.Portrait,
                _ => DisplayOrientation.Unknown
            };
        }

        public DisplayRotation GetScreenRotation(object activity)
        {
            var context = activity as Activity;
            return context.WindowManager.DefaultDisplay.Rotation switch
            {
                SurfaceOrientation.Rotation270 => DisplayRotation.Rotation270,
                SurfaceOrientation.Rotation180 => DisplayRotation.Rotation180,
                SurfaceOrientation.Rotation90 => DisplayRotation.Rotation90,
                SurfaceOrientation.Rotation0 => DisplayRotation.Rotation0,
                _ => DisplayRotation.Unknown,
            };
        }

        /// <summary>
        /// https://stackoverflow.com/questions/63407883/getting-screen-width-on-api-level-30-android-11-getdefaultdisplay-and-getme
        /// </summary>
        /// <param name="Activity"></param>
        /// <param name=""></param>
        /// <returns>px</returns>
        public float GetScreenWidth(object arg)
        {
            var activity = arg as Activity;
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.R)
            {
                WindowMetrics windowMetrics = activity.WindowManager.CurrentWindowMetrics;
                Rect bounds = windowMetrics.Bounds;
                Insets insets = windowMetrics.WindowInsets.GetInsetsIgnoringVisibility(
                        WindowInsets.Type.SystemBars()
                );

                if (activity.Resources.Configuration.Orientation
                        == Res.Orientation.Landscape
                        && activity.Resources.Configuration.SmallestScreenWidthDp < 600
                )
                { // landscape and phone
                    int navigationBarSize = insets.Right + insets.Left;
                    return bounds.Width() - navigationBarSize;
                }
                else
                { // portrait or tablet
                    return bounds.Width();
                }
            }
            else
            {
                DisplayMetrics outMetrics = new DisplayMetrics();
                activity.WindowManager.DefaultDisplay.GetMetrics(outMetrics);
                return outMetrics.WidthPixels;
            }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/63407883/getting-screen-width-on-api-level-30-android-11-getdefaultdisplay-and-getme
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>px</returns>
        public float GetScreenHeight(object arg)
        {
            var activity = arg  as Activity;
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.R)
            {
                WindowMetrics windowMetrics = activity.WindowManager.CurrentWindowMetrics;
                Rect bounds = windowMetrics.Bounds;
                Insets insets = windowMetrics.WindowInsets.GetInsetsIgnoringVisibility(
                        WindowInsets.Type.SystemBars()
                );

                if (activity.Resources.Configuration.Orientation
                        == Android.Content.Res.Orientation.Landscape
                        && activity.Resources.Configuration.SmallestScreenWidthDp < 600
                )
                { // landscape and phone
                    return bounds.Height();
                }
                else
                { // portrait or tablet
                    int navigationBarSize = insets.Bottom;
                    return bounds.Height() - navigationBarSize;
                }
            }
            else
            {
                DisplayMetrics outMetrics = new DisplayMetrics();
                activity.WindowManager.DefaultDisplay.GetMetrics(outMetrics);
                return outMetrics.HeightPixels;
            }
        }

        #endregion
    }
}
