
namespace Maui.Platform.Helper.Devices.Screen
{
    public partial class ScreenHelper:IDisposable
    {
        public static ScreenHelper Instance = new ScreenHelper();
        private ScreenHelper() { }

        public void Dispose()
        {
            Instance = null;
        }
    }

#if !__IOS__ && !__ANDROID__ && !WINDOWS
    public partial class ScreenHelper : IScreenHelper
    {
        public float GetScreenDensity()
        {
            throw new NotImplementedException();
        }

        public float GetScreenHeight(object arg = null)
        {
            throw new NotImplementedException();
        }

        public DisplayOrientation GetScreenOrientation(object arg = null)
        {
            throw new NotImplementedException();
        }

        public DisplayRotation GetScreenRotation(object arg = null)
        {
            throw new NotImplementedException();
        }

        public float GetScreenWidth(object arg = null)
        {
            throw new NotImplementedException();
        }
    }
#endif
}
