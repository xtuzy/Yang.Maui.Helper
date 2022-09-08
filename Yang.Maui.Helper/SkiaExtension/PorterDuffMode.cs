using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlendMode = SkiaSharp.SKBlendMode;
namespace Yang.Maui.Helper.SkiaExtension
{
    public enum PorterDuffMode
    {
        Clear = BlendMode.Clear,
        SRC = BlendMode.Src,
        DST = BlendMode.Dst,
        SRC_OVER = BlendMode.SrcOver,
        DST_OVER = BlendMode.DstOver,
        SRC_IN = BlendMode.SrcIn,
        DST_IN = BlendMode.DstIn,
        SRC_OUT = BlendMode.SrcOut,
        DST_OUT = BlendMode.DstOut,
        SRC_ATOP = BlendMode.SrcATop,
        DST_ATOP = BlendMode.DstATop,
        XOR = BlendMode.Xor,
        DARKEN = BlendMode.Darken,
        LIGHTEN = BlendMode.Lighten,
        MULTIPLY = BlendMode.Multiply,
        SCREEN = BlendMode.Screen,
        ADD = BlendMode.Plus,
        OVERLAY = BlendMode.Overlay,
    }
}
