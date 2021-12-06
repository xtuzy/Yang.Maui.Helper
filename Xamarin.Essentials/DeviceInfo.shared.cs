using System;

namespace Xamarin.Essentials
{
    public static partial class DeviceInfo
    {
        public static string Model => GetModel();

        public static string Manufacturer => GetManufacturer();

        public static string Name => GetDeviceName();

        public static string VersionString => GetVersionString();

        public static Version Version => ParseVersion(VersionString);

        public static DevicePlatform Platform => GetPlatform();

        public static DeviceIdiom Idiom => GetIdiom();

        public static DeviceType DeviceType => GetDeviceType();

        static Version ParseVersion(string version)
        {
            if (Version.TryParse(version, out var number))
                return number;

            if (int.TryParse(version, out var major))
                return new Version(major, 0);

            return new Version(0, 0);
        }
    }

    public enum DeviceType
    {
        Unknown = 0,
        Physical = 1,
        Virtual = 2
    }
}