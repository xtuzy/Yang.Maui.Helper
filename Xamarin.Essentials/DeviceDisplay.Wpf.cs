using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Xamarin.Essentials
{
    public static partial class DeviceDisplay
    {
        static bool PlatformKeepScreenOn
        {
            get => throw ExceptionUtils.NotSupportedOrImplementedException;
            set
            {
                if (value)
                {
                    PreventSleep();
                }
                else
                {

                }
            }
        }

        static DisplayInfo GetMainDisplayInfo()
        {
            var currentGraphics = Graphics.FromHwnd(new WindowInteropHelper(Application.Current.MainWindow).Handle);
            var density = currentGraphics.DpiX / 96;//注意y轴可能不同
//只提供主屏幕宽高 https://stackoverflow.com/a/54932562/13254773
            var w = ((int)SystemParameters.PrimaryScreenWidth) * density;
            var h = ((int)SystemParameters.PrimaryScreenHeight) * density;

            //var hdi = HdmiDisplayInformation.GetForCurrentView();
            //var hdm = hdi?.GetCurrentDisplayMode();

            return new DisplayInfo(
                width: w,
                height: h,
                density: density,
                orientation: CalculateOrientation(w, h),
                rotation: CalculateRotation(w, h),
                //rate: (float)(hdm?.RefreshRate ?? 0));
                rate: GetMonitorInfo());
        }

        static void StartScreenMetricsListeners()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                //参考:https://docs.microsoft.com/en-us/dotnet/api/system.windows.window.dpichanged?view=windowsdesktop-5.0
                Application.Current.MainWindow.DpiChanged += MainWindow_DpiChanged;
                //参考:https://stackoverflow.com/questions/8323318/wpf-orientation
                Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
            });
        }

        static void StopScreenMetricsListeners()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainWindow.DpiChanged -= MainWindow_DpiChanged;
                Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= new EventHandler(SystemEvents_DisplaySettingsChanged);
            });
        }

        static void MainWindow_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            var metrics = GetMainDisplayInfo();
            OnMainDisplayInfoChanged(metrics);
        }

        static void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            var metrics = GetMainDisplayInfo();
            OnMainDisplayInfoChanged(metrics);
        }

        #region Sleep 参考:https://stackoverflow.com/questions/49045701/prevent-screen-from-sleeping-with-c-sharp

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        static void PreventSleep()
        {
            // Prevent Idle-to-Sleep (monitor not affected) (see note above)
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        #endregion 

        static DisplayOrientation CalculateOrientation(float w, float h)
        {
            if (w > h)
            {
                return DisplayOrientation.Landscape;
            }
            else if (w < h)
            {
                return DisplayOrientation.Portrait;
            }
            else
            {
                return DisplayOrientation.Unknown;
            }
        }

        static DisplayRotation CalculateRotation(float w, float h)
        {
            if (w > h)
            {
                return DisplayRotation.Rotation0;
            }
            else if (w < h)
            {
                return DisplayRotation.Rotation90;
            }
            else
            {
                return DisplayRotation.Unknown;
            }
        }

        #region 获取刷新率,参考: https://github.com/rickbrew/RefreshRateWpf
        static uint GetMonitorInfo()
        {
            // 1. Get the window handle ("HWND" in Win32 parlance)
            WindowInteropHelper helper = new WindowInteropHelper(Application.Current.MainWindow);
            IntPtr hwnd = helper.Handle;

            // 2. Get a monitor handle ("HMONITOR") for the window. 
            //    If the window is straddling more than one monitor, Windows will pick the "best" one.
            IntPtr hmonitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (hmonitor == IntPtr.Zero)
            {
                return 0;// "MonitorFromWindow returned NULL ☹";
            }

            // 3. Get more information about the monitor.
            MONITORINFOEXW monitorInfo = new MONITORINFOEXW();
            monitorInfo.cbSize = (uint)Marshal.SizeOf<MONITORINFOEXW>();

            bool bResult = GetMonitorInfoW(hmonitor, ref monitorInfo);
            if (!bResult)
            {
                return 0;// "GetMonitorInfoW returned FALSE ☹";
            }

            // 4. Get the current display settings for that monitor, which includes the resolution and refresh rate.
            DEVMODEW devMode = new DEVMODEW();
            devMode.dmSize = (ushort)Marshal.SizeOf<DEVMODEW>();

            bResult = EnumDisplaySettingsW(monitorInfo.szDevice, ENUM_CURRENT_SETTINGS, out devMode);
            if (!bResult)
            {
                return 0;// "EnumDisplaySettingsW returned FALSE ☹";
            }

            // Done!
            //return string.Format("{0} x {1} @ {2}hz", devMode.dmPelsWidth, devMode.dmPelsHeight, devMode.dmDisplayFrequency);
            return devMode.dmDisplayFrequency;
        }

        // MonitorFromWindow
        private const uint MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        // RECT
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        // MONITORINFOEX
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private unsafe struct MONITORINFOEXW
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szDevice;
        }

        // GetMonitorInfo
        [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMonitorInfoW(
            IntPtr hMonitor,
            ref MONITORINFOEXW lpmi);

        // EnumDisplaySettings
        private const uint ENUM_CURRENT_SETTINGS = unchecked((uint)-1);

        [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumDisplaySettingsW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpszDeviceName,
            uint iModeNum,
            out DEVMODEW lpDevMode);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DEVMODEW
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;

            public ushort dmSpecVersion;
            public ushort dmDriverVersion;
            public ushort dmSize;
            public ushort dmDriverExtra;
            public uint dmFields;

            /*public short dmOrientation;
            public short dmPaperSize;
            public short dmPaperLength;
            public short dmPaperWidth;
            public short dmScale;
            public short dmCopies;
            public short dmDefaultSource;
            public short dmPrintQuality;*/
            // These next 4 int fields are a union with the above 8 shorts, but we don't need them right now
            public int dmPositionX;
            public int dmPositionY;
            public uint dmDisplayOrientation;
            public uint dmDisplayFixedOutput;

            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;

            public short dmLogPixels;
            public uint dmBitsPerPel;
            public uint dmPelsWidth;
            public uint dmPelsHeight;

            public uint dmNupOrDisplayFlags;
            public uint dmDisplayFrequency;

            public uint dmICMMethod;
            public uint dmICMIntent;
            public uint dmMediaType;
            public uint dmDitherType;
            public uint dmReserved1;
            public uint dmReserved2;
            public uint dmPanningWidth;
            public uint dmPanningHeight;
        }

        #endregion
    }
}
