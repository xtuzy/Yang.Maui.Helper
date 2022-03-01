using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yang.Maui.Helper.Platforms.Android.Design
{
    public static class LayoutGuide
    {
        /// <summary>
        /// ToolBar 标准高度(像素)<br/>
        /// <see href="https://stackoverflow.com/questions/30570904/whats-the-height-of-the-android-toolbar">参考:whats-the-height-of-the-android-toolbar</see>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int GetToolBarHeight(Context context)
        {
            int[] attrs = new int[] { Resource.Attribute.actionBarSize };
            TypedArray ta = context.ObtainStyledAttributes(attrs);
            int toolBarHeight = ta.GetDimensionPixelSize(0, -1);
            ta.Recycle();
            return toolBarHeight;
        }
    }
}