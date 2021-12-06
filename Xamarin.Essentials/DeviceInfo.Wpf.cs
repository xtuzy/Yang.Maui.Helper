using System;
using System.Management;

namespace Xamarin.Essentials
{
    public static partial class DeviceInfo
    {
        static string GetModel() 
        {
            ///https://docs.microsoft.com/en-us/dotnet/api/system.management?redirectedfrom=MSDN&view=dotnet-plat-ext-5.0
            SelectQuery query = new System.Management.SelectQuery(@"Select * from Win32_ComputerSystem");

            //initialize the searcher with the query it is supposed to execute
            using (System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(query))
            {
                //execute the query
                foreach (System.Management.ManagementObject process in searcher.Get())
                {
                    //print system info
                    process.Get();
                    //Console.WriteLine("/*********Operating System Information ***************/");
                    //Console.WriteLine("{0}{1}", "System Manufacturer:", process["Manufacturer"]);
                    //Console.WriteLine("{0}{1}", " System Model:", process["Model"]);
                    return process["Model"].ToString();
                }
            }
            return string.Empty;
        }

        static string GetManufacturer() 
        {
            ///https://docs.microsoft.com/en-us/dotnet/api/system.management?redirectedfrom=MSDN&view=dotnet-plat-ext-5.0
            SelectQuery query = new System.Management.SelectQuery(@"Select * from Win32_ComputerSystem");

            //initialize the searcher with the query it is supposed to execute
            using (System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(query))
            {
                //execute the query
                foreach (System.Management.ManagementObject process in searcher.Get())
                {
                    //print system info
                    process.Get();
                    //Console.WriteLine("/*********Operating System Information ***************/");
                    //Console.WriteLine("{0}{1}", "System Manufacturer:", process["Manufacturer"]);
                    //Console.WriteLine("{0}{1}", " System Model:", process["Model"]);
                    return process["Manufacturer"].ToString();
                }
            }

            return String.Empty;//这个多
        }

        static string GetDeviceName()
        {

            return Environment.MachineName;
        }

        /// <summary>
        /// <see href="https://stackoverflow.com/questions/2819934/detect-windows-version-in-net">detect-windows-version-in-net</see>
        /// </summary>
        /// <returns></returns>
        static string GetVersionString() => Environment.OSVersion.VersionString;

        static DevicePlatform GetPlatform() => DevicePlatform.WPF;

        static DeviceIdiom GetIdiom()
        {
            var currentIdiom = DeviceIdiom.Desktop;//TODO:判断平板

            // hope we got it somewhere
            return currentIdiom;
        }

        static DeviceType GetDeviceType()
        {
            var isEmulator = false; //貌似没有WPF模拟器

            if (isEmulator)
                return DeviceType.Virtual;

            return DeviceType.Physical;
        }
    }
}