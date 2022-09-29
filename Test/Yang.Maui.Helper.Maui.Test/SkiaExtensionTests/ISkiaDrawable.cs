using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yang.Maui.Helper.Skia.Canvas;

namespace Yang.Maui.Helper.Maui.Test.SkiaExtensionTests
{
    internal interface ISkiaDrawable
    {
        void OnDraw(AndroidCanvas canvas);
    }
}
