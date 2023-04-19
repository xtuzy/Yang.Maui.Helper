using Android.Opengl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.SKGpuView.Handlers
{
    public partial class SKGLViewHandler
    {
        protected partial void SetupRenderLoop(bool oneShot, SKGpuView view)
        {
            if (oneShot)
            {
                PlatformView.RequestRender();
            }
            
            PlatformView.RenderMode = view.HasRenderLoop
                ? Rendermode.Continuously
                : Rendermode.WhenDirty;
        }
    }
}
