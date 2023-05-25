using Android.Content;
using Android.Graphics;
using Android.Hardware.Lights;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Platform;
using ScrollView = Android.Widget.ScrollView;
using View = Android.Views.View;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    public class MyScrollView : MauiScrollView
    {
        public MyScrollView(Context context) : base(context)
        {
        }

        public MyScrollView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public MyScrollView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected MyScrollView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
            var view = GetChildAt(0);
            view.Layout(0, -ScrollY, view.Width, view.Height - ScrollY);
        }

        protected override void DispatchDraw(Canvas canvas)
        {
            var count = canvas.Save();
            canvas.Translate(ScrollX, ScrollY);
            base.DispatchDraw(canvas);
            canvas.RestoreToCount(count);
        }
    }
}
