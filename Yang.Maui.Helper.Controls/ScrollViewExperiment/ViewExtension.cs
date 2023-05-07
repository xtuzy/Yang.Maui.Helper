using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    internal static class ViewExtension
    {
        public static void RemoveFromSuperview(this View view)
        {
            (view.Parent as Layout)?.Remove(view);
        }
    }
}
