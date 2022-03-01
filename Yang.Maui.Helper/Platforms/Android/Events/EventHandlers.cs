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

namespace Yang.Maui.Helper.Platforms.Android.Events
{
    public delegate Java.Lang.Object GetItemEventHandler(int position);
    public delegate long GetItemIdEventHandler(int position);
    public delegate View GetViewEventHandler(int position, View convertView, ViewGroup parent);
}