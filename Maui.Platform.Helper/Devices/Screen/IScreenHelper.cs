namespace Maui.Platform.Helper.Devices.Screen
{
    public interface IScreenHelper
    {
        /// <summary>
        /// get px w
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>px</returns>
        float GetScreenWidth(object arg = null);

        /// <summary>
        /// get px h
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>px</returns>
        float GetScreenHeight(object arg = null);

        DisplayOrientation GetScreenOrientation(object arg = null);

        DisplayRotation GetScreenRotation(object arg = null);

        /// <summary>
        /// get density such as 1,1.5,2...
        /// </summary>
        /// <returns></returns>
        float GetScreenDensity();
    }
}