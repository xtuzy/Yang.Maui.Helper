using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.Canvas
{
    public enum RegionOp
    {
        DIFFERENCE = SKRegionOperation.Difference,
        INTERSECT = SKRegionOperation.Intersect,
        UNION = SKRegionOperation.Union,
        XOR = SKRegionOperation.XOR,
        REVERSE_DIFFERENCE = SKRegionOperation.ReverseDifference,
        REPLACE = SKRegionOperation.Replace
    }
}
