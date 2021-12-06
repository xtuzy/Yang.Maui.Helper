using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace Xamarin.Essentials
{
    /// <summary>
    /// Windows Taskbar have Task,Recent Action.<br/>
    /// 参考: <see cref="https://www.cnblogs.com/Sunwayking/articles/1598161.html"/>
    /// </summary>
    public static partial class AppActions
    {
        internal static bool PlatformIsSupported
            => true;

        /// <summary>
        /// <see cref="https://blog.walterlv.com/post/wpf-application-with-jumplist.html"/>
        /// </summary>
        /// <returns></returns>
        static async Task<IEnumerable<AppAction>> PlatformGetAsync()
        {
            var jumpList = JumpList.GetJumpList(Application.Current);
            
            var actions = new List<AppAction>();
            foreach (var item in jumpList.JumpItems)
                actions.Add(item.ToAction());

            return actions;
        }


        /// <summary>
        /// <see cref="https://www.cnblogs.com/Sunwayking/articles/1598161.html"/>
        /// </summary>
        /// <param name="actions"></param>
        /// <returns></returns>
        static async Task PlatformSetAsync(IEnumerable<AppAction> actions)
        {
            var jumpList = new JumpList();
            JumpList.SetJumpList(Application.Current, jumpList);
            foreach(var item in actions)
            {
                jumpList.JumpItems.Add(item.ToJumpItem());
            }
            jumpList.Apply();
        }

        internal static async Task OnLaunched()
        {
            // PlatformGetAsync();
            var jumpList = new JumpList();
            JumpList.SetJumpList(Application.Current, jumpList);
            var actions = await PlatformGetAsync();
            foreach (var item in actions)
            {
                jumpList.JumpItems.Add(item.ToJumpItem());
            }

            jumpList.ShowRecentCategory = true;//显示最近
            jumpList.ShowFrequentCategory = true;//显示常用
            jumpList.Apply();
        }

        static AppAction ToAction(this JumpItem jumpItem)
        {
            var item = jumpItem as JumpTask;
            return new AppAction(item.Arguments, item.Title, item.Description);
        }

        public static string IconDirectory { get; set; } = "Assets";
        public static string IconExtension { get; set; } = "png";
        static JumpItem ToJumpItem(this AppAction action)
        {
            //var id = appActionPrefix + Convert.ToBase64String(Encoding.Default.GetBytes(action.Id));
            //var item = JumpListItem.CreateWithArguments(id, action.Title);
            
            var item = new JumpTask();
            item.Arguments = action.Id;
            item.Title = action.Title;
            //参考:https://www.cnblogs.com/jonet007/archive/2011/11/09/2242540.html
            item.ApplicationPath = Path.Combine(Application.Current.StartupUri.AbsolutePath, Process.GetCurrentProcess().MainModule.FileName);
            if (!string.IsNullOrEmpty(action.Subtitle))
                item.Description = action.Subtitle;

            if (!string.IsNullOrEmpty(action.Icon))
            {
                var dir = IconDirectory.Trim('/', '\\').Replace('\\', '/');
                var ext = IconExtension;
                if (!string.IsNullOrEmpty(ext) && !ext.StartsWith("."))
                    ext = "." + ext;
                //item.Logo = new Uri($"ms-appx:///{dir}/{action.Icon}{ext}");
            }
            return item;
        }
    }
}
