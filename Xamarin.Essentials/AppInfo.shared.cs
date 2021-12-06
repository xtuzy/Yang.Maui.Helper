using System;

namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        public static string PackageName => PlatformGetPackageName();

        public static string Name => PlatformGetName();

        public static string VersionString => PlatformGetVersionString();

        public static Version Version => ParseVersion(VersionString);

        public static string BuildString => PlatformGetBuild();

        public static void ShowSettingsUI() => PlatformShowSettingsUI();

        public static AppTheme RequestedTheme => PlatformRequestedTheme();

        internal static Version ParseVersion(string version)
        {
            if (Version.TryParse(version, out var number))
                return number;

            if (int.TryParse(version, out var major))
                return new Version(major, 0);

            return new Version(0, 0);
        }
    }
}
