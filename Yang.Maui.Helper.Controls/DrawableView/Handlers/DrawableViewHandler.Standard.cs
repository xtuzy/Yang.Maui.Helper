#if NET6_0_OR_GREATER && !__ANDROID__ && !__IOS__ && !WINDOWS
using Microsoft.Maui.Handlers;
using System;

namespace Yang.Maui.Helper.Controls.DrawableView.Handlers
{
    public partial class DrawableViewHandler : ViewHandler<IDrawableView, object>
    {
        protected override object CreatePlatformView() => throw new NotImplementedException();
        public static void MapInvalidate(DrawableViewHandler handler, IDrawableView drawableView, object? arg) { }
    }
}
#endif