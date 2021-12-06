using Xamarin.Essentials;

using System;

namespace Xamarin.Helper.Devices
{
    /// <summary>
    /// 获得屏幕宽高,朝向,在App启动时初始化监听,确保获取的数据实时更新
    /// 注:使用Xamarin.Essentials,适用于Xamarin.iOS,Android,Mac,不适用于WPF
    /// </summary>
    public static partial class ScreenHelper
    {
        /// <summary>
        /// 尽量在App初始化时使用,确保整体能响应更改 
        /// </summary>
        public static event EventHandler<DisplayInfoChangedEventArgs> ScreenInforChanged {
            add
            {
                DeviceDisplay.MainDisplayInfoChanged+= value;
            }
            remove
            {
                DeviceDisplay.MainDisplayInfoChanged -= value;
            }
        }

        /// <summary>
        /// 默认处理屏幕信息改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                //iOS中需要在Ui线程中调用,参考https://docs.microsoft.com/zh-cn/xamarin/essentials/device-display?tabs=ios
                pixelWidth = (float)DeviceDisplay.MainDisplayInfo.Width;
                pixelHeight = (float)DeviceDisplay.MainDisplayInfo.Height;
                orientation = DeviceDisplay.MainDisplayInfo.Orientation;
            });
        }

        
        private static float pixelWidth = 0;

        /// <summary>
        /// 屏幕像素宽
        /// </summary>
        public static float PixelWidth
        {
            get
            {
                if (pixelWidth == 0)
                {
                    ScreenInforChanged += DeviceDisplay_MainDisplayInfoChanged;

                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        //iOS中需要在Ui线程中调用,参考https://docs.microsoft.com/zh-cn/xamarin/essentials/device-display?tabs=ios
                        pixelWidth = (float)DeviceDisplay.MainDisplayInfo.Width;
                    });
                }
                return pixelWidth;
            }
        }


        private static float pixelHeight = 0;


        /// <summary>
        /// 屏幕像素高度
        /// </summary>
        public static float PixelHeight
        {
            get
            {
                if (pixelHeight == 0)
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        //iOS中需要在Ui线程中调用,参考https://docs.microsoft.com/zh-cn/xamarin/essentials/device-display?tabs=ios
                        pixelHeight = (float)DeviceDisplay.MainDisplayInfo.Height;
                    });
                }
                return pixelHeight;
            }
        }


        private static DisplayOrientation orientation = 0;

        /// <summary>
        /// 朝向
        /// </summary>
        public static DisplayOrientation Orientation
        {
            get
            {
                if (orientation == 0)
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        //iOS中需要在Ui线程中调用,参考https://docs.microsoft.com/zh-cn/xamarin/essentials/device-display?tabs=ios
                        orientation = DeviceDisplay.MainDisplayInfo.Orientation;
                    });
                }
                return orientation;
            }
        }
    }
}