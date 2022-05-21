#if __ANDROID__
using Android.Content;
using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View = Android.Views.View;
namespace Yang.Maui.Helper.CustomControls.Platform
{

    public class PlatformDrawableView : View
    {
        public PlatformDrawableView(Context context) : base(context)
        {
        }

        public event EventHandler<PlatformDrawEventArgs> PlatformDraw;

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            PlatformDraw?.Invoke(this, new PlatformDrawEventArgs(canvas));
        }
    }
}
#endif