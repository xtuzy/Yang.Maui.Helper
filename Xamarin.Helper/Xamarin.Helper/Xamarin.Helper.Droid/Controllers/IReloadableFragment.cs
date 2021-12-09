using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamarin.Helper.Controllers
{
    public interface IReloadableFragment
    {
        void OnStart();
        void OnPause();
        void OnStop();
        void OnSDestroy();
    }
}