using Yang.Maui.Helper.Controls.DrawableView.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Yang.Maui.Helper.Controls.DrawableView
{
    public class DrawableView : View, IDrawableView
    {
        /// <summary>
        /// <paramref name="sender"/>:
        /// Android:<see cref="Android.Views.View"/>,
        /// iOS:<see cref="UIKit.UIView"/>,
        /// Windows:<see cref="Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl"/>,
        /// <br/>
        /// <paramref name="e"/>:
        /// Android:<see cref="Android.Graphics.Canvas"/>,
        /// iOS:<see cref="CoreGraphics.CGRect"/>,
        /// Windows <see cref="Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs"/>,
        /// </summary>
        public event EventHandler<PlatformDrawEventArgs> PaintSurface;
        public event EventHandler Loaded;
        public event EventHandler Unloaded;
        public event EventHandler TouchDown;
        public event EventHandler TouchMove;
        public event EventHandler TouchUp;

        public event EventHandler Invalidated;
        public void Invalidate()
        {
            Handler?.Invoke(nameof(IDrawableView.Invalidate));

            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Load()
        {
            Loaded?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Unload()
        {
            Unloaded?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnTouchDown(Point point)
        {
            TouchDown?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnTouchMove(Point point)
        {
            TouchMove?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnTouchUp(Point point)
        {
            TouchUp?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// <paramref name="sender"/>:
        /// Android:<see cref="Android.Views.View"/>,
        /// iOS:<see cref="UIKit.UIView"/>,
        /// Windows:<see cref="Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl"/>,
        /// <br/>
        /// <paramref name="e"/>:
        /// Android:<see cref="Android.Graphics.Canvas"/>,
        /// iOS:<see cref="CoreGraphics.CGRect"/>,
        /// Windows <see cref="Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs"/>,
        /// </summary>
        public virtual void OnDraw(object sender, PlatformDrawEventArgs e)
        {
            PaintSurface?.Invoke(sender, e);
        }

        public virtual Size CustomMeasuredSize(double widthConstraint, double heightConstraint)
        {
            return Size.Zero;
        }
    }
}
