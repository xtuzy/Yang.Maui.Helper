#if WINDOWS
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Controls.DrawableView.Platform
{

    public class PlatformDrawableView : UserControl
    {
        CanvasControl canvasControl;
        public PlatformDrawableView()
        {
            base.Loaded += UserControl_Loaded;
            base.Unloaded += UserControl_Unloaded;
        }

        public void Invalidate()
        {
            canvasControl?.Invalidate();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            canvasControl = new CanvasControl();
            canvasControl.Draw += OnDraw;
            base.Content = canvasControl;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (canvasControl != null && !canvasControl.IsLoaded)
            {
                canvasControl.RemoveFromVisualTree();
                canvasControl = null;
            }
        }

        public event EventHandler<PlatformDrawEventArgs> PlatformDraw;
        private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            PlatformDraw?.Invoke(sender, new PlatformDrawEventArgs(args));
        }
    }
}
#endif