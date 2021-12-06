using Microsoft.Win32;
using System.Windows;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
       
        static string PlatformGetPackageName() => Application.ResourceAssembly.GetName().FullName;

        /// <summary>
        /// <see cref="https://stackoverflow.com/a/32964097/13254773"/>
        /// </summary>
        /// <returns></returns>
        static string PlatformGetName() => Application.ResourceAssembly.GetName().Name;

        static string PlatformGetVersionString() => Application.ResourceAssembly.GetName().Version.ToString();

        static string PlatformGetBuild() => Application.ResourceAssembly.GetName().Version.Build.ToString();

        /// <summary>
        /// <see cref="https://stackoverflow.com/a/37020706/13254773"/><br/>
        /// This is not correct, wpf seems no setting page 
        /// </summary>
        static void PlatformShowSettingsUI()
        {
            System.Diagnostics.Process.Start("ms-settings:privacy-webcam");
        }


        /// <summary>
        /// <see cref="https://stackoverflow.com/a/68845708/13254773"/>
        /// </summary>
        /// <returns></returns>
        static AppTheme PlatformRequestedTheme()
        {
            using var key = Registry.CurrentUser.OpenSubKey(ThemeRegistryKeyPath);
            var registryValueObject = key?.GetValue(ThemeRegistryValueName);
            if (registryValueObject == null)
            {
                return AppTheme.Light;
            }
            var registryValue = (int)registryValueObject;

            return registryValue > 0 ? AppTheme.Light : AppTheme.Dark;
        }

        private const string ThemeRegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        private const string ThemeRegistryValueName = "AppsUseLightTheme";

        
    }
}
