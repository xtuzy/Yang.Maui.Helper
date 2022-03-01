#if __ANDROID__
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Platform.Helper.Layouts
{
    public static class LayoutParamsHelper
    {
        public static ViewGroup.MarginLayoutParams SetMargins(this ViewGroup.MarginLayoutParams lp, int left, int top = 0, int right = 0, int bottom = 0)
        {
            lp.SetMargins(left, top, right, bottom);
            return lp;
        }

        public static ViewGroup.MarginLayoutParams SetLeftMargins(this ViewGroup.MarginLayoutParams lp, int leftMargin)
        {
            lp.LeftMargin = leftMargin;
            return lp;
        }

        public static ViewGroup.MarginLayoutParams SetTopMargins(this ViewGroup.MarginLayoutParams lp, int topMargin)
        {
            lp.TopMargin = topMargin;
            return lp;
        }

        public static ViewGroup.MarginLayoutParams SetRightMargins(this ViewGroup.MarginLayoutParams lp, int rightpMargin)
        {
            lp.RightMargin = rightpMargin;
            return lp;
        }
        public static ViewGroup.MarginLayoutParams SetBottomMargins(this ViewGroup.MarginLayoutParams lp, int bottomMargin)
        {
            lp.BottomMargin = bottomMargin;
            return lp;
        }

        public static void AddViews(this ViewGroup viewGroup, params View[] views)
        {
            foreach (var view in views)
            {
                viewGroup.AddView(view);
            }
        }
    }
}
#endif