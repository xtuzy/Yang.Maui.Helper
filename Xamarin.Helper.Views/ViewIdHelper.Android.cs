#if __ANDROID__
using Android.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Helper.Views
{
    public static class ViewIdHelper
    { 
        public static int ResourceId(this View view)
        {
            if (view.Id == -1)
            {
                view.Id = View.GenerateViewId();
            }
            return view.Id;
        }
    }
}

#endif
