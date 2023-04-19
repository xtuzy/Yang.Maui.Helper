using System;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Yang.Maui.Helper.Skia.SKGpuView
{
    public class GetPropertyValueEventArgs<T> : EventArgs
    {
        public T Value { get; set; }
    }
}