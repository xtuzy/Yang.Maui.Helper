#if __IOS__ || __MACCATALYST__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Yang.Maui.Helper.CustomControls.Platform
{

    public class PlatformDrawableView : UIView
    {
        public event EventHandler<PlatformDrawEventArgs> PlatformDraw;

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);
            PlatformDraw?.Invoke(this, new PlatformDrawEventArgs(rect));
        }
    }
}
#endif