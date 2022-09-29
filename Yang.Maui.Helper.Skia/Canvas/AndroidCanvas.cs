using SkiaSharp;
using System;
using Bitmap = SkiaSharp.SKBitmap;
using BlendMode = SkiaSharp.SKBlendMode;
using Color = SkiaSharp.SKColor;
using EdgeType = System.Object;
using Matrix = SkiaSharp.SKMatrix;
using MeasuredText = System.Object;
using NinePatch = System.Object;
using Paint = SkiaSharp.SKPaint;
using Path = SkiaSharp.SKPath;
using Picture = SkiaSharp.SKPicture;
using Rect = SkiaSharp.SKRectI;
using RectF = SkiaSharp.SKRect;
using RenderNode = System.Object;
using Region = System.Object;
using SaveFlags = System.Int32;
using VertexMode = SkiaSharp.SKVertexMode;

namespace Yang.Maui.Helper.Skia.Canvas
{
    using Font = SkiaSharp.SKFont;
    /// <summary>
    /// The Canvas class holds the "draw" calls.
    /// </summary>    
    public class AndroidCanvas : IDisposable
    {
        public SKCanvas SKCanvas { get; private set; }
        public AndroidCanvas(SKCanvas canvas)
        {
            SKCanvas = canvas;
        }

        /// <summary>
        ///  Set the clip to the difference of the current clip and the specified path.
        /// </summary>
        /// <param name="path">The path used in the difference operation</param>
        /// <returns>true if the resulting clip is non-empty</returns>
        public virtual void ClipOutPath(Path path)
        {
            ClipPath(path, RegionOp.DIFFERENCE);
        }

        /// <summary>
        /// Set the clip to the difference of the current clip and the specified rectangle, which is expressed in local coordinates.
        /// </summary>
        /// <param name="left">The left side of the rectangle used in the difference operation</param>
        /// <param name="top">The top of the rectangle used in the difference operation</param>
        /// <param name="right">The right side of the rectangle used in the difference operation</param>
        /// <param name="bottom">The bottom of the rectangle used in the difference operation</param>
        public virtual void ClipOutRect(int left, int top, int right, int bottom)
        {
            SKCanvas.ClipRect(new RectF(left, top, right, bottom), SKClipOperation.Difference, true);
        }

        /// <summary>
        /// Set the clip to the difference of the current clip and the specified rectangle, which is expressed in local coordinates.
        /// </summary>
        /// <param name="left">The left side of the rectangle used in the difference operation</param>
        /// <param name="top">The top of the rectangle used in the difference operation</param>
        /// <param name="right">The right side of the rectangle used in the difference operation</param>
        /// <param name="bottom">The bottom of the rectangle used in the difference operation</param>
        public virtual void ClipOutRect(float left, float top, float right, float bottom)
        {
            SKCanvas.ClipRect(new RectF(left, top, right, bottom), SKClipOperation.Difference);
        }

        /// <summary>
        /// Set the clip to the difference of the current clip and the specified rectangle, which is expressed in local coordinates.
        /// </summary>
        /// <param name="rect">The rectangle to perform a difference op with the current clip.</param>
        public virtual void ClipOutRect(RectF rect)
        {
            SKCanvas.ClipRect(rect, SKClipOperation.Difference);
        }

        /// <summary>
        /// Set the clip to the difference of the current clip and the specified rectangle, which is expressed in local coordinates.
        /// </summary>
        /// <param name="rect">The rectangle to perform a difference op with the current clip.</param>
        public virtual void ClipOutRect(Rect rect)
        {
            SKCanvas.ClipRect(rect, SKClipOperation.Difference);
        }
        [Obsolete("deprecated")]
        public virtual void ClipPath(Path path, RegionOp op) { throw new NotImplementedException(nameof(ClipRect)); }

        /// <summary>
        /// Intersect the current clip with the specified path.
        /// </summary>
        /// <param name="path">The path to intersect with the current clip</param>
        public virtual void ClipPath(Path path)
        {
            SKCanvas.ClipPath(path, SKClipOperation.Intersect);
        }

        /// <summary>
        /// Intersect the current clip with the specified rectangle, which is expressed in local coordinates.
        /// </summary>
        /// <param name="left">The left side of the rectangle to intersect with the current clip</param>
        /// <param name="top">The top of the rectangle to intersect with the current clip</param>
        /// <param name="right">The right side of the rectangle to intersect with the current clip</param>
        /// <param name="bottom">The bottom of the rectangle to intersect with the current clip</param>
        public virtual void ClipRect(int left, int top, int right, int bottom)
        {
            SKCanvas.ClipRect(new Rect(left, top, right, bottom), SKClipOperation.Intersect);
        }

        /// <summary>
        /// Intersect the current clip with the specified rectangle, which is expressed in local coordinates.
        /// </summary>
        /// <param name="left">The left side of the rectangle to intersect with the current clip</param>
        /// <param name="top">The top of the rectangle to intersect with the current clip</param>
        /// <param name="right">The right side of the rectangle to intersect with the current clip</param>
        /// <param name="bottom">The bottom of the rectangle to intersect with the current clip</param>
        public virtual void ClipRect(float left, float top, float right, float bottom)
        {
            SKCanvas.ClipRect(new RectF(left, top, right, bottom), SKClipOperation.Intersect);
        }
        [Obsolete("deprecated")]
        public virtual bool ClipRect(RectF rect, RegionOp op) { throw new NotImplementedException(nameof(ClipRect)); }

        /// <summary>
        /// Intersect the current clip with the specified rectangle, which is expressed inlocal coordinates.
        /// </summary>
        /// <param name="rect">The rectangle to intersect with the current clip.</param>
        /// <returns>true if the resulting clip is non-empty</returns>
        public virtual void ClipRect(RectF rect)
        {
            SKCanvas.ClipRect(rect, SKClipOperation.Intersect);
        }

        [Obsolete("deprecated")]
        public virtual bool ClipRect(Rect rect, RegionOp op) { throw new NotImplementedException(nameof(ClipRect)); }

        /// <summary>
        /// Intersect the current clip with the specified rectangle, which is expressed in local coordinates.
        /// </summary>
        /// <param name="rect">The rectangle to intersect with the current clip.</param>
        public virtual void ClipRect(Rect rect)
        {
            SKCanvas.ClipRect(rect, SKClipOperation.Intersect);
        }
        [Obsolete("deprecated")]
        public virtual bool ClipRect(float left, float top, float right, float bottom, RegionOp op) { throw new NotImplementedException(nameof(ClipRect)); }
        [Obsolete("deprecated")]
        public virtual bool ClipRegion(Region? region, RegionOp? op) { throw new NotImplementedException(nameof(ClipRegion)); }

        /// <summary>
        /// Intersect the current clip with the specified region.
        /// </summary>
        /// <param name="region">The region to operate on the current clip, based on op</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Obsolete("deprecated")]
        public virtual bool ClipRegion(Region? region) { throw new NotImplementedException(nameof(ClipRegion)); }

        /// <summary>
        /// Preconcat the current matrix with the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix to preconcatenate with the current matrix</param>
        public virtual void Concat(ref Matrix matrix)
        {
            SKCanvas.Concat(ref matrix);
        }

        //
        // 摘要:
        //     Disables Z support, preventing any RenderNodes drawn after this point from being
        //     visually reordered or having shadows rendered.
        public virtual void DisableZ() { throw new NotImplementedException(); }

        /// <summary>
        /// Draw the specified arc, which will be scaled to fit inside the specified oval.
        /// </summary>
        /// <param name="oval">The bounds of oval used to define the shape and size of the arc</param>
        /// <param name="startAngle">Starting angle (in degrees) where the arc begins</param>
        /// <param name="sweepAngle">Sweep angle (in degrees) measured clockwise</param>
        /// <param name="useCenter">If true, include the center of the oval in the arc, and close it if it is being stroked. This will draw a wedge</param>
        /// <param name="paint">The paint used to draw the arc</param>
        public virtual void DrawArc(RectF oval, float startAngle, float sweepAngle, bool useCenter, Paint paint)
        {
            SKCanvas.DrawArc(oval, startAngle, sweepAngle, useCenter, paint);
        }
          
        /// <summary>
        /// Draw the specified arc, which will be scaled to fit inside the specified oval.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="startAngle">Starting angle (in degrees) where the arc begins</param>
        /// <param name="sweepAngle">Sweep angle (in degrees) measured clockwise</param>
        /// <param name="useCenter">If true, include the center of the oval in the arc, and close it if it is being stroked. This will draw a wedge</param>
        /// <param name="paint">The paint used to draw the arc</param>
        public virtual void DrawArc(float left, float top, float right, float bottom, float startAngle, float sweepAngle, bool useCenter, Paint paint)
        {
            DrawArc(new RectF(left, top, right, bottom), startAngle, sweepAngle, useCenter, paint);
        }
  
        /// <summary>
        /// Fill the entire canvas' bitmap (restricted to the current clip) with the specified ARGB color, using srcover porterduff mode.
        /// </summary>
        /// <param name="a">alpha component (0..255) of the color to draw onto the canvas</param>
        /// <param name="r">red component (0..255) of the color to draw onto the canvas</param>
        /// <param name="g">green component (0..255) of the color to draw onto the canvas</param>
        /// <param name="b">blue component (0..255) of the color to draw onto the canvas</param>
        public virtual void DrawARGB(int a, int r, int g, int b)
        {
            DrawColor(new SKColor((byte)r, (byte)g, (byte)b, (byte)a));
        }
  
        /// <summary>
        /// Draw the specified bitmap, scaling/translating automatically to fill the destination rectangle.
        /// </summary>
        /// <param name="bitmap">The bitmap to be drawn</param>
        /// <param name="src">May be null. The subset of the bitmap to be drawn</param>
        /// <param name="dst">The rectangle that the bitmap will be scaled/translated to fit into</param>
        /// <param name="paint">May be null. The paint used to draw the bitmap</param>
        public virtual void DrawBitmap(Bitmap bitmap, Rect src, RectF dst, Paint? paint)
        {
            SKCanvas.DrawBitmap(bitmap, src, dst, paint);
        }

        /// <summary>
        /// Legacy version of drawBitmap(int[] colors, .
        /// </summary>
        /// <param name="colors"></param>
        /// <param name="offset"></param>
        /// <param name="stride"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hasAlpha"></param>
        /// <param name="paint"></param>
        /// <exception cref="NotImplementedException"></exception>
        [Obsolete("deprecated")]
        public virtual void DrawBitmap(int[] colors, int offset, int stride, int x, int y, int width, int height, bool hasAlpha, Paint? paint) { throw new NotImplementedException(nameof(DrawBitmap)); }
        //
        // 摘要:
        //     Draw the specified bitmap, with its top/left corner at (x,y), using the specified
        //     paint, transformed by the current matrix.
        //
        // 参数:
        //   colors:
        //     Array of colors representing the pixels of the bitmap
        //
        //   offset:
        //     Offset into the array of colors for the first pixel
        //
        //   stride:
        //     The number of colors in the array between rows (must be >= width or <= -width).
        //
        //   x:
        //     The X coordinate for where to draw the bitmap
        //
        //   y:
        //     The Y coordinate for where to draw the bitmap
        //
        //   width:
        //     The width of the bitmap
        //
        //   height:
        //     The height of the bitmap
        //
        //   hasAlpha:
        //     True if the alpha channel of the colors contains valid values. If false, the
        //     alpha byte is ignored (assumed to be 0xFF for every pixel).
        //
        //   paint:
        //     The paint used to draw the bitmap (may be null)
        [Obsolete("deprecated")]
        public virtual void DrawBitmap(int[] colors, int offset, int stride, float x, float y, int width, int height, bool hasAlpha, Paint? paint) { throw new NotImplementedException(nameof(DrawBitmap)); }

        /// <summary>
        /// Draw the specified bitmap, with its top/left corner at (x,y), using the specified paint, transformed by the current matrix.
        /// </summary>
        /// <param name="bitmap">The bitmap to be drawn</param>
        /// <param name="left">The position of the left side of the bitmap being drawn</param>
        /// <param name="top">The position of the top side of the bitmap being drawn</param>
        /// <param name="paint">The paint used to draw the bitmap (may be null)</param>
        public virtual void DrawBitmap(Bitmap bitmap, float left, float top, Paint? paint)
        {
            SKCanvas.DrawBitmap(bitmap, left, top, paint);
        }
 
        /// <summary>
        /// Draw the specified bitmap, scaling/translating automatically to fill the destination rectangle.
        /// </summary>
        /// <param name="bitmap">The bitmap to be drawn</param>
        /// <param name="src">May be null. The subset of the bitmap to be drawn</param>
        /// <param name="dst">The rectangle that the bitmap will be scaled/translated to fit into</param>
        /// <param name="paint">May be null. The paint used to draw the bitmap</param>
        public virtual void DrawBitmap(Bitmap bitmap, Rect? src, Rect dst, Paint? paint)
        {
            int left, top, right, bottom;
            if (src == null)
            {
                left = top = 0;
                right = bitmap.Width;
                bottom = bitmap.Height;
            }
            else
            {
                left = src.Value.Left;
                right = src.Value.Right;
                top = src.Value.Top;
                bottom = src.Value.Bottom;
            }
            SKCanvas.DrawBitmap(bitmap, new RectF(left, top, right, bottom), dst, paint);
        }
        //
        // 摘要:
        //     Draw the bitmap using the specified matrix.
        //
        // 参数:
        //   bitmap:
        //     The bitmap to draw
        //
        //   matrix:
        //     The matrix used to transform the bitmap when it is drawn
        //
        //   paint:
        //     May be null. The paint used to draw the bitmap
        public virtual void DrawBitmap(Bitmap bitmap, Matrix matrix, Paint? paint) { throw new NotImplementedException(nameof(DrawBitmap)); }
        //
        // 摘要:
        //     Draw the bitmap through the mesh, where mesh vertices are evenly distributed
        //     across the bitmap.
        //
        // 参数:
        //   bitmap:
        //     The bitmap to draw using the mesh
        //
        //   meshWidth:
        //     The number of columns in the mesh. Nothing is drawn if this is 0
        //
        //   meshHeight:
        //     The number of rows in the mesh. Nothing is drawn if this is 0
        //
        //   verts:
        //     Array of x,y pairs, specifying where the mesh should be drawn. There must be
        //     at least (meshWidth+1) * (meshHeight+1) * 2 + vertOffset values in the array
        //
        //   vertOffset:
        //     Number of verts elements to skip before drawing
        //
        //   colors:
        //     May be null. Specifies a color at each vertex, which is interpolated across the
        //     cell, and whose values are multiplied by the corresponding bitmap colors. If
        //     not null, there must be at least (meshWidth+1) * (meshHeight+1) + colorOffset
        //     values in the array.
        //
        //   colorOffset:
        //     Number of color elements to skip before drawing
        //
        //   paint:
        //     May be null. The paint used to draw the bitmap
        public virtual void DrawBitmapMesh(Bitmap bitmap, int meshWidth, int meshHeight, float[] verts, int vertOffset, int[]? colors, int colorOffset, Paint? paint) { throw new NotImplementedException(); }
 
        /// <summary>
        /// Draw the specified circle using the specified paint.
        /// </summary>
        /// <param name="cx">The x-coordinate of the center of the cirle to be drawn</param>
        /// <param name="cy">The y-coordinate of the center of the cirle to be drawn</param>
        /// <param name="radius">The radius of the cirle to be drawn</param>
        /// <param name="paint">The paint used to draw the circle</param>
        public virtual void DrawCircle(float cx, float cy, float radius, Paint paint)
        {
            SKCanvas.DrawCircle(cx, cy, radius, paint);
        }
        //
        // 摘要:
        //     Fill the entire canvas' bitmap (restricted to the current clip) with the specified
        //     color and blendmode.
        //
        // 参数:
        //   color:
        //     the ColorLong to draw onto the canvas. See the Color class for details about
        //     ColorLongs.
        //
        //   mode:
        //     the blendmode to apply to the color
        public virtual void DrawColor(long color, BlendMode mode) { throw new NotImplementedException(nameof(DrawColor)); }
 
        /// <summary>
        /// Fill the entire canvas' bitmap (restricted to the current clip) with the specified color, using srcover porterduff mode.
        /// </summary>
        /// <param name="color">the color to draw onto the canvas</param>
        public virtual void DrawColor(Color color)
        {
            SKCanvas.DrawColor(color, BlendMode.SrcOver);
        }
  
        /// <summary>
        /// Fill the entire canvas' bitmap (restricted to the current clip) with the specified color and blendmode.
        /// </summary>
        /// <param name="color">the color to draw onto the canvas</param>
        /// <param name="mode">the blendmode to apply to the color</param>
        public virtual void DrawColor(Color color, BlendMode mode)
        {
            SKCanvas.DrawColor(color, mode);
        }
        public virtual void DrawColor(Color color, PorterDuffMode mode)
        {
            SKCanvas.DrawColor(color, (BlendMode)(int)mode);
        }

        //
        // 摘要:
        //     Fill the entire canvas' bitmap (restricted to the current clip) with the specified
        //     color, using srcover porterduff mode.
        //
        // 参数:
        //   color:
        //     the ColorLong to draw onto the canvas. See the Color class for details about
        //     ColorLongs.
        public virtual void DrawColor(long color) { throw new NotImplementedException(nameof(DrawColor)); }
        //
        // 摘要:
        //     Draws a double rounded rectangle using the specified paint.
        //
        // 参数:
        //   outer:
        //     The outer rectangular bounds of the roundRect to be drawn
        //
        //   outerRx:
        //     The x-radius of the oval used to round the corners on the outer rectangle
        //
        //   outerRy:
        //     The y-radius of the oval used to round the corners on the outer rectangle
        //
        //   inner:
        //     The inner rectangular bounds of the roundRect to be drawn
        //
        //   innerRx:
        //     The x-radius of the oval used to round the corners on the inner rectangle
        //
        //   innerRy:
        //     The y-radius of the oval used to round the corners on the outer rectangle
        //
        //   paint:
        //     The paint used to draw the double roundRect
        public virtual void DrawDoubleRoundRect(RectF outer, float outerRx, float outerRy, RectF inner, float innerRx, float innerRy, Paint paint) { throw new NotImplementedException(nameof(DrawDoubleRoundRect)); }
        //
        // 摘要:
        //     Draws a double rounded rectangle using the specified paint.
        //
        // 参数:
        //   outer:
        //     The outer rectangular bounds of the roundRect to be drawn
        //
        //   outerRadii:
        //     Array of 8 float representing the x, y corner radii for top left, top right,
        //     bottom right, bottom left corners respectively on the outer rounded rectangle
        //
        //   inner:
        //     The inner rectangular bounds of the roundRect to be drawn
        //
        //   innerRadii:
        //     Array of 8 float representing the x, y corner radii for top left, top right,
        //     bottom right, bottom left corners respectively on the outer rounded rectangle
        //
        //   paint:
        //     The paint used to draw the double roundRect
        public virtual void DrawDoubleRoundRect(RectF outer, float[] outerRadii, RectF inner, float[] innerRadii, Paint paint) { throw new NotImplementedException(nameof(DrawDoubleRoundRect)); }
        //
        // 摘要:
        //     Draw array of glyphs with specified font.
        //
        // 参数:
        //   glyphIds:
        //     Array of glyph IDs. The length of array must be greater than or equal to glyphIdOffset
        //     + glyphCount.
        //
        //   glyphIdOffset:
        //     Number of elements to skip before drawing in <code>glyphIds</code> array.
        //
        //   positions:
        //     A flattened X and Y position array. The first glyph X position must be stored
        //     at positionOffset. The first glyph Y position must be stored at positionOffset
        //     + 1, then the second glyph X position must be stored at positionOffset + 2. The
        //     length of array must be greater than or equal to positionOffset + glyphCount
        //     * 2.
        //
        //   positionOffset:
        //     Number of elements to skip before drawing in positions. The first glyph X position
        //     must be stored at positionOffset. The first glyph Y position must be stored at
        //     positionOffset + 1, then the second glyph X position must be stored at positionOffset
        //     + 2.
        //
        //   glyphCount:
        //     Number of glyphs to be drawn.
        //
        //   font:
        //     Font used for drawing.
        //
        //   paint:
        //     Paint used for drawing. The typeface set to this paint is ignored.
        public virtual void DrawGlyphs(int[] glyphIds, int glyphIdOffset, float[] positions, int positionOffset, int glyphCount, Font font, Paint paint) { throw new NotImplementedException(nameof(DrawGlyphs)); }
 
        /// <summary>
        /// Draw a line segment with the specified start and stop x,y coordinates, using the specified paint.
        /// </summary>
        /// <param name="startX">The x-coordinate of the start point of the line</param>
        /// <param name="startY">The y-coordinate of the start point of the line</param>
        /// <param name="stopX"></param>
        /// <param name="stopY"></param>
        /// <param name="paint">The paint used to draw the line</param>
        public virtual void DrawLine(float startX, float startY, float stopX, float stopY, Paint paint)
        {
            SKCanvas.DrawLine(startX, startY, stopX, stopY, paint);
        }

        /// <summary>
        /// Draw a series of lines.
        /// </summary>
        /// <param name="pts">Array of points to draw [x0 y0 x1 y1 x2 y2 ...]</param>
        /// <param name="paint">The paint used to draw the points</param>
        public virtual void DrawLines(float[] pts, Paint paint)
        {
            var points = new SKPoint[pts.Length / 2];
            for (int i = 0; i < pts.Length / 2; i++)
            {
                points[i] = new SKPoint(pts[2 * i], pts[2 * i + 1]);
            }
            SKCanvas.DrawPoints(SKPointMode.Lines, points, paint);
        }
        //
        // 摘要:
        //     Draw a series of lines.
        //
        // 参数:
        //   pts:
        //     Array of points to draw [x0 y0 x1 y1 x2 y2 ...]
        //
        //   offset:
        //     Number of values in the array to skip before drawing.
        //
        //   count:
        //     The number of values in the array to process, after skipping "offset" of them.
        //     Since each line uses 4 values, the number of "lines" that are drawn is really
        //     (count >> 2).
        //
        //   paint:
        //     The paint used to draw the points
        public virtual void DrawLines(float[] pts, int offset, int count, Paint paint) { throw new NotImplementedException(nameof(DrawLines)); }

        /// <summary>
        /// Draw the specified oval using the specified paint.
        /// </summary>
        /// <param name="oval"></param>
        /// <param name="paint">The rectangle bounds of the oval to be drawn</param>
        public virtual void DrawOval(RectF oval, Paint paint)
        {
            SKCanvas.DrawOval(oval, paint);
        }

        /// <summary>
        /// Draw the specified oval using the specified paint.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="paint"></param>
        public virtual void DrawOval(float left, float top, float right, float bottom, Paint paint)
        {
            SKCanvas.DrawOval(new RectF(left, right, top, bottom), paint);
        }
 
        /// <summary>
        /// Fill the entire canvas' bitmap (restricted to the current clip) with the specified paint.
        /// </summary>
        /// <param name="paint">The paint used to draw onto the canvas</param>
        public virtual void DrawPaint(Paint paint)
        {
            SKCanvas.DrawPaint(paint);
        }
        //
        // 摘要:
        //     Draws the specified bitmap as an N-patch (most often, a 9-patch.
        //
        // 参数:
        //   patch:
        //     The ninepatch object to render
        //
        //   dst:
        //     The destination rectangle.
        //
        //   paint:
        //     The paint to draw the bitmap with. may be null
        public virtual void DrawPatch(NinePatch patch, RectF dst, Paint? paint) { throw new NotImplementedException(nameof(DrawPatch)); }
        //
        // 摘要:
        //     Draws the specified bitmap as an N-patch (most often, a 9-patch.
        //
        // 参数:
        //   patch:
        //     The ninepatch object to render
        //
        //   dst:
        //     The destination rectangle.
        //
        //   paint:
        //     The paint to draw the bitmap with. may be null
        public virtual void DrawPatch(NinePatch patch, Rect dst, Paint? paint) { throw new NotImplementedException(nameof(DrawPatch)); }
  
        /// <summary>
        /// Draw the specified path using the specified paint.
        /// </summary>
        /// <param name="path">The path to be drawn</param>
        /// <param name="paint">The paint used to draw the path</param>
        public virtual void DrawPath(Path path, Paint paint)
        {
            SKCanvas.DrawPath(path, paint);
        }
   
        /// <summary>
        /// Save the canvas state, draw the picture, and restore the canvas state.
        /// </summary>
        /// <param name="picture">The picture to be drawn</param>
        public virtual void DrawPicture(Picture picture)
        {
            SKCanvas.DrawPicture(picture);
        }
        //
        // 摘要:
        //     Draw the picture, stretched to fit into the dst rectangle.
        //
        // 参数:
        //   picture:
        //     The picture to be drawn
        //
        //   dst:
        //     To be added.
        public virtual void DrawPicture(Picture picture, Rect dst) { throw new NotImplementedException(nameof(DrawPicture)); }
        //
        // 摘要:
        //     Draw the picture, stretched to fit into the dst rectangle.
        //
        // 参数:
        //   picture:
        //     The picture to be drawn
        //
        //   dst:
        //     To be added.
        public virtual void DrawPicture(Picture picture, RectF dst) { throw new NotImplementedException(nameof(DrawPicture)); }

        /// <summary>
        /// Helper for drawPoints() for drawing a single point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="paint"></param>
        public virtual void DrawPoint(float x, float y, Paint paint)
        {
            SKCanvas.DrawPoint(x, y, paint);
        }

        /// <summary>
        /// Helper for drawPoints() that assumes you want to draw the entire array
        /// </summary>
        /// <param name="pts">Array of points to draw [x0 y0 x1 y1 x2 y2 ...]</param>
        /// <param name="paint">The paint used to draw the points</param>
        public virtual void DrawPoints(float[] pts, Paint paint)
        {
            var points = new SKPoint[pts.Length / 2];
            for (int i = 0; i < pts.Length / 2; i++)
            {
                points[i] = new SKPoint(pts[2 * i], pts[2 * i + 1]);
            }
            SKCanvas.DrawPoints(SKPointMode.Points, points, paint);
        }
        //
        // 摘要:
        //     Draw a series of points.
        //
        // 参数:
        //   pts:
        //     Array of points to draw [x0 y0 x1 y1 x2 y2 ...]
        //
        //   offset:
        //     Number of values to skip before starting to draw.
        //
        //   count:
        //     The number of values to process, after skipping offset of them. Since one point
        //     uses two values, the number of "points" that are drawn is really (count >> 1).
        //
        //   paint:
        //     The paint used to draw the points
        public virtual void DrawPoints(float[]? pts, int offset, int count, Paint paint) { throw new NotImplementedException(nameof(DrawPoints)); }
        //
        // 摘要:
        //     Draw the text in the array, with each character's origin specified by the pos
        //     array.
        //
        // 参数:
        //   text:
        //     The text to be drawn
        //
        //   pos:
        //     Array of [x,y] positions, used to position each character
        //
        //   paint:
        //     The paint used for the text (e.g. color, size, style)
        [Obsolete("deprecated")]
        public virtual void DrawPosText(string text, float[] pos, Paint paint) { throw new NotImplementedException(nameof(DrawPosText)); }
        //
        // 摘要:
        //     Draw the text in the array, with each character's origin specified by the pos
        //     array.
        //
        // 参数:
        //   text:
        //     The text to be drawn
        //
        //   index:
        //     The index of the first character to draw
        //
        //   count:
        //     The number of characters to draw, starting from index.
        //
        //   pos:
        //     Array of [x,y] positions, used to position each character
        //
        //   paint:
        //     The paint used for the text (e.g. color, size, style)
        [Obsolete("deprecated")]
        public virtual void DrawPosText(char[] text, int index, int count, float[] pos, Paint paint) { throw new NotImplementedException(nameof(DrawPosText)); }
   
        /// <summary>
        /// Draw the specified Rect using the specified paint.
        /// </summary>
        /// <param name="left">The left side of the rectangle to be drawn</param>
        /// <param name="top">The top side of the rectangle to be drawn</param>
        /// <param name="right">The right side of the rectangle to be drawn</param>
        /// <param name="bottom">The bottom side of the rectangle to be drawn</param>
        /// <param name="paint">The paint used to draw the rect</param>
        public virtual void DrawRect(float left, float top, float right, float bottom, Paint paint)
        {
            SKCanvas.DrawRect(new RectF(left, top, right, bottom), paint);
        }
 
        /// <summary>
        /// Draw the specified Rect using the specified paint.
        /// </summary>
        /// <param name="rect">The rect to be drawn</param>
        /// <param name="paint">The paint used to draw the rect</param>
        public virtual void DrawRect(RectF rect, Paint paint)
        {
            SKCanvas.DrawRect(rect, paint);
        }
   
        /// <summary>
        /// Draw the specified Rect using the specified Paint.
        /// </summary>
        /// <param name="r">The rectangle to be drawn.</param>
        /// <param name="paint">The paint used to draw the rectangle</param>
        public virtual void DrawRect(Rect r, Paint paint)
        {
            SKCanvas.DrawRect(r, paint);
        }
        //
        // 摘要:
        //     Draws the given RenderNode.
        //
        // 参数:
        //   renderNode:
        //     The RenderNode to draw, must be non-null.
        public virtual void DrawRenderNode(RenderNode renderNode) { throw new NotImplementedException(nameof(DrawRenderNode)); }

        /// <summary>
        /// Fill the entire canvas' bitmap (restricted to the current clip) with the specified RGB color, using srcover porterduff mode.
        /// </summary>
        /// <param name="r">red component (0..255) of the color to draw onto the canvas</param>
        /// <param name="g">green component (0..255) of the color to draw onto the canvas</param>
        /// <param name="b">blue component (0..255) of the color to draw onto the canvas</param>
        public virtual void DrawRGB(int r, int g, int b)
        {
            SKCanvas.DrawColor(new SKColor((byte)r, (byte)g, (byte)b));
        }
  
        /// <summary>
        /// Draw the specified round-rect using the specified paint.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="rx">The x-radius of the oval used to round the corners</param>
        /// <param name="ry">The y-radius of the oval used to round the corners</param>
        /// <param name="paint">The paint used to draw the roundRect</param>
        public virtual void DrawRoundRect(float left, float top, float right, float bottom, float rx, float ry, Paint paint)
        {
            SKCanvas.DrawRoundRect(new SKRect(left, top, right, bottom), rx, ry, paint);
        }
 
        /// <summary>
        /// Draw the specified round-rect using the specified paint.
        /// </summary>
        /// <param name="rect">The rectangular bounds of the roundRect to be drawn</param>
        /// <param name="rx">The x-radius of the oval used to round the corners</param>
        /// <param name="ry">The y-radius of the oval used to round the corners</param>
        /// <param name="paint">The paint used to draw the roundRect</param>
        public virtual void DrawRoundRect(RectF rect, float rx, float ry, Paint paint)
        {
            SKCanvas.DrawRoundRect(rect, rx, ry, paint);
        }

        /// <summary>
        /// Draw the text, with origin at (x,y), using the specified paint.
        /// </summary>
        /// <param name="text">The text to be drawn</param>
        /// <param name="x">The x-coordinate of the origin of the text being drawn</param>
        /// <param name="y">The y-coordinate of the baseline of the text being drawn</param>
        /// <param name="paint">The paint used for the text (e.g. color, size, style)</param>
        public virtual void DrawText(string text, float x, float y, Paint paint)
        {
            SKCanvas.DrawText(text, x, y, paint);
        }
        //
        // 摘要:
        //     Draw the specified range of text, specified by start/end, with its origin at
        //     (x,y), in the specified Paint.
        //
        // 参数:
        //   text:
        //     The text to be drawn
        //
        //   start:
        //     The index of the first character in text to draw
        //
        //   end:
        //     (end - 1) is the index of the last character in text to draw
        //
        //   x:
        //     The x-coordinate of origin for where to draw the text
        //
        //   y:
        //     The y-coordinate of origin for where to draw the text
        //
        //   paint:
        //     The paint used for the text (e.g. color, size, style)
        public virtual void DrawText(ICharSequence text, int start, int end, float x, float y, Paint paint) { throw new NotImplementedException(nameof(DrawText)); }
        //
        // 摘要:
        //     Draw the text, with origin at (x,y), using the specified paint.
        //
        // 参数:
        //   index:
        //     To be added.
        //
        //   count:
        //     To be added.
        //
        //   text:
        //     The text to be drawn
        //
        //   x:
        //     The x-coordinate of the origin of the text being drawn
        //
        //   y:
        //     The y-coordinate of the baseline of the text being drawn
        //
        //   paint:
        //     The paint used for the text (e.g. color, size, style)
        public virtual void DrawText(char[] text, int index, int count, float x, float y, Paint paint) { throw new NotImplementedException(nameof(DrawText)); }

        /// <summary>
        /// Draw the text, with origin at (x,y), using the specified paint.
        /// </summary>
        /// <param name="text">The text to be drawn</param>
        /// <param name="start">The index of the first character in text to draw</param>
        /// <param name="end">(end - 1) is the index of the last character in text to draw</param>
        /// <param name="x">The x-coordinate of the origin of the text being drawn</param>
        /// <param name="y">The y-coordinate of the baseline of the text being drawn</param>
        /// <param name="paint">The paint used for the text (e.g. color, size, style)</param>
        public virtual void DrawText(string text, int start, int end, float x, float y, Paint paint)
        {
            SKCanvas.DrawText(text.Substring(start, end - start), x, y, paint);
        }
        //
        // 摘要:
        //     Draw the text, with origin at (x,y), using the specified paint, along the specified
        //     path.
        //
        // 参数:
        //   text:
        //     The text to be drawn
        //
        //   index:
        //     The starting index within the text to be drawn
        //
        //   count:
        //     Starting from index, the number of characters to draw
        //
        //   path:
        //     The path the text should follow for its baseline
        //
        //   hOffset:
        //     The distance along the path to add to the text's starting position
        //
        //   vOffset:
        //     The distance above(-) or below(+) the path to position the text
        //
        //   paint:
        //     The paint used for the text (e.g. color, size, style)
        public virtual void DrawTextOnPath(char[] text, int index, int count, Path path, float hOffset, float vOffset, Paint paint) { throw new NotImplementedException(nameof(DrawTextOnPath)); }

        /// <summary>
        /// Draw the text, with origin at (x,y), using the specified paint, along the specified path.
        /// </summary>
        /// <param name="text">The text to be drawn</param>
        /// <param name="path">The path the text should follow for its baseline</param>
        /// <param name="hOffset">The distance along the path to add to the text's starting position</param>
        /// <param name="vOffset">The distance above(-) or below(+) the path to position the text</param>
        /// <param name="paint">The paint used for the text (e.g. color, size, style)</param>
        public virtual void DrawTextOnPath(string text, Path path, float hOffset, float vOffset, Paint paint)
        {
            SKCanvas.DrawTextOnPath(text, path, hOffset, vOffset, paint);
        }
        //
        // 摘要:
        //     Draw a run of text, all in a single direction, with optional context for complex
        //     text shaping.
        //
        // 参数:
        //   text:
        //     the text to render
        //
        //   start:
        //     the start of the text to render. Data before this position can be used for shaping
        //     context.
        //
        //   end:
        //     the end of the text to render. Data at or after this position can be used for
        //     shaping context.
        //
        //   contextStart:
        //     the index of the start of the shaping context
        //
        //   contextEnd:
        //     the index of the end of the shaping context
        //
        //   x:
        //     the x position at which to draw the text
        //
        //   y:
        //     the y position at which to draw the text
        //
        //   isRtl:
        //     whether the run is in RTL direction
        //
        //   paint:
        //     the paint
        public virtual void DrawTextRun(MeasuredText text, int start, int end, int contextStart, int contextEnd, float x, float y, bool isRtl, Paint paint) { throw new NotImplementedException(nameof(DrawTextRun)); }
        //
        // 摘要:
        //     Draw a run of text, all in a single direction, with optional context for complex
        //     text shaping.
        //
        // 参数:
        //   text:
        //     the text to render
        //
        //   index:
        //     the start of the text to render
        //
        //   count:
        //     the count of chars to render
        //
        //   contextIndex:
        //     the start of the context for shaping. Must be no greater than index.
        //
        //   contextCount:
        //     the number of characters in the context for shaping. contexIndex + contextCount
        //     must be no less than index + count.
        //
        //   x:
        //     the x position at which to draw the text
        //
        //   y:
        //     the y position at which to draw the text
        //
        //   isRtl:
        //     whether the run is in RTL direction
        //
        //   paint:
        //     the paint
        public virtual void DrawTextRun(char[] text, int index, int count, int contextIndex, int contextCount, float x, float y, bool isRtl, Paint paint) { throw new NotImplementedException(nameof(DrawTextRun)); }
        //
        // 摘要:
        //     Draw a run of text, all in a single direction, with optional context for complex
        //     text shaping.
        //
        // 参数:
        //   text:
        //     the text to render
        //
        //   start:
        //     the start of the text to render. Data before this position can be used for shaping
        //     context.
        //
        //   end:
        //     the end of the text to render. Data at or after this position can be used for
        //     shaping context.
        //
        //   contextStart:
        //     the index of the start of the shaping context
        //
        //   contextEnd:
        //     the index of the end of the shaping context
        //
        //   x:
        //     the x position at which to draw the text
        //
        //   y:
        //     the y position at which to draw the text
        //
        //   isRtl:
        //     whether the run is in RTL direction
        //
        //   paint:
        //     the paint
        public virtual void DrawTextRun(ICharSequence text, int start, int end, int contextStart, int contextEnd, float x, float y, bool isRtl, Paint paint) { throw new NotImplementedException(nameof(DrawTextRun)); }
        //
        // 摘要:
        //     Draw a run of text, all in a single direction, with optional context for complex
        //     text shaping.
        //
        // 参数:
        //   text:
        //     the text to render
        //
        //   start:
        //     the start of the text to render. Data before this position can be used for shaping
        //     context.
        //
        //   end:
        //     the end of the text to render. Data at or after this position can be used for
        //     shaping context.
        //
        //   contextStart:
        //     the index of the start of the shaping context
        //
        //   contextEnd:
        //     the index of the end of the shaping context
        //
        //   x:
        //     the x position at which to draw the text
        //
        //   y:
        //     the y position at which to draw the text
        //
        //   isRtl:
        //     whether the run is in RTL direction
        //
        //   paint:
        //     the paint
        public virtual void DrawTextRun(string text, int start, int end, int contextStart, int contextEnd, float x, float y, bool isRtl, Paint paint) { throw new NotImplementedException(nameof(DrawTextRun)); }
        public virtual void DrawVertices(VertexMode mode, int vertexCount, float[] verts, int vertOffset, float[]? texs, int texOffset, int[]? colors, int colorOffset, short[]? indices, int indexOffset, int indexCount, Paint paint) { throw new NotImplementedException(nameof(DrawVertices)); }
        //
        // 摘要:
        //     To be added.
        //
        // 言论：
        //     To be added.
        public virtual void EnableZ() { throw new NotImplementedException(nameof(EnableZ)); }
 
        /// <summary>
        /// Return the bounds of the current clip (in local coordinates) in the bounds parameter, and return true if it is non-empty.
        /// </summary>
        /// <param name="bounds">Return the clip bounds here. If it is null, ignore it but still return true if the current clip is non-empty.</param>
        /// <returns>true if the current clip is non-empty.</returns>
        public virtual bool GetClipBounds(out RectF bounds)
        {
            return SKCanvas.GetLocalClipBounds(out bounds);
        }
        //
        // 摘要:
        //     Return, in ctm, the current transformation matrix.
        //
        // 参数:
        //   ctm:
        //     To be added.
        [Obsolete("deprecated")]
        public virtual void GetMatrix(Matrix ctm) { throw new NotImplementedException(nameof(GetMatrix)); }

        /// <summary>
        /// Return true if the specified path, after being transformed by the current matrix, would lie completely outside of the current clip.
        /// </summary>
        /// <param name="path">The path to compare with the current clip</param>
        /// <returns>true if the path (transformed by the canvas' matrix) does not intersect with the canvas' clip</returns>
        public virtual bool QuickReject(Path path)
        {
            return SKCanvas.QuickReject(path);
        }
        [Obsolete("deprecated")]
        public virtual bool QuickReject(Path path, EdgeType type) { throw new NotImplementedException(nameof(QuickReject)); }
 
        /// <summary>
        /// Return true if the specified rectangle, after being transformed by the current matrix, would lie completely outside of the current clip.
        /// </summary>
        /// <param name="rect">the rect to compare with the current clip</param>
        /// <returns>true if the rect (transformed by the canvas' matrix) does not intersect with the canvas' clip</returns>
        public virtual bool QuickReject(RectF rect)
        {
            return SKCanvas.QuickReject(rect);
        }
        [Obsolete("deprecated")]
        public virtual bool QuickReject(RectF rect, EdgeType type) { throw new NotImplementedException(nameof(QuickReject)); }
  
        /// <summary>
        /// Return true if the specified rectangle, after being transformed by the current matrix, would lie completely outside of the current clip.
        /// </summary>
        /// <param name="left">The left side of the rectangle to compare with the current clip</param>
        /// <param name="top">The top of the rectangle to compare with the current clip</param>
        /// <param name="right">The right side of the rectangle to compare with the current clip</param>
        /// <param name="bottom">The bottom of the rectangle to compare with the current clip</param>
        /// <returns>true if the rect (transformed by the canvas' matrix) does not intersect with the canvas' clip</returns>
        public virtual bool QuickReject(float left, float top, float right, float bottom)
        {
            return SKCanvas.QuickReject(new RectF(left, top, right, bottom));
        }
        [Obsolete("deprecated")]
        public virtual bool QuickReject(float left, float top, float right, float bottom, EdgeType type) { throw new NotImplementedException(nameof(QuickReject)); }
 
        /// <summary>
        /// This call balances a previous call to save(), and is used to remove all modifications to the matrix/clip state since the last save call.
        /// </summary>
        public virtual void Restore()
        {
            SKCanvas.Restore();
        }
   
        /// <summary>
        /// Efficient way to pop any calls to save() that happened after the save count reached saveCount.
        /// </summary>
        /// <param name="saveCount">The save level to restore to.</param>
        public virtual void RestoreToCount(int saveCount)
        {
            SKCanvas.RestoreToCount(saveCount);
        }
 
        /// <summary>
        /// Preconcat the current matrix with the specified rotation.
        /// </summary>
        /// <param name="degrees">The amount to rotate, in degrees</param>
        /// <param name="px">The x-coord for the pivot point (unchanged by the rotation)</param>
        /// <param name="py">The y-coord for the pivot point (unchanged by the rotation)</param>
        public void Rotate(float degrees, float px, float py)
        {
            SKCanvas.RotateDegrees(degrees, px, py);
        }
 
        /// <summary>
        /// Preconcat the current matrix with the specified rotation.
        /// </summary>
        /// <param name="degrees">The amount to rotate, in degrees</param>
        public virtual void Rotate(float degrees)
        {
            SKCanvas.RotateDegrees(degrees);
        }
        //
        // 摘要:
        //     Saves the current matrix and clip onto a private stack.
        //
        // 参数:
        //   saveFlags:
        //     flag bits that specify which parts of the Canvas state to save/restore
        //
        // 返回结果:
        //     The value to pass to restoreToCount() to balance this save()
        [Obsolete("deprecated")]
        public virtual int Save(SaveFlags saveFlags) { throw new NotImplementedException(nameof(Save)); }
 
        /// <summary>
        /// Saves the current matrix and clip onto a private stack.
        /// </summary>
        /// <returns>The value to pass to restoreToCount() to balance this save()</returns>
        public virtual int Save()
        {
            return SKCanvas.Save();
        }

        /// <summary>
        /// Convenience for #saveLayer(RectF, Paint) that takes the four float coordinates of the bounds rectangle.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="paint"></param>
        /// <returns></returns>
        public virtual int SaveLayer(float left, float top, float right, float bottom, Paint? paint)
        {
            return SKCanvas.SaveLayer(new RectF(left, top, right, bottom), paint);
        }
 
        /// <summary>
        /// This behaves the same as save(), but in addition it allocates and redirects drawing to an offscreen rendering target.
        /// </summary>
        /// <param name="bounds">May be null. The maximum size the offscreen render target needs to be (in local coordinates)</param>
        /// <param name="paint">This is copied, and is applied to the offscreen when restore() is called.</param>
        /// <returns>value to pass to restoreToCount() to balance this save()</returns>
        public virtual int SaveLayer(RectF bounds, Paint? paint)
        {
            return SKCanvas.SaveLayer(bounds, paint);
        }
        //
        // 摘要:
        //     Helper version of saveLayer() that takes 4 values rather than a RectF.
        //
        // 参数:
        //   left:
        //     To be added.
        //
        //   top:
        //     To be added.
        //
        //   right:
        //     To be added.
        //
        //   bottom:
        //     To be added.
        //
        //   paint:
        //     To be added.
        //
        //   saveFlags:
        //     To be added.
        //
        // 返回结果:
        //     To be added.
        [Obsolete("deprecated")]
        public virtual int SaveLayer(float left, float top, float right, float bottom, Paint? paint, SaveFlags saveFlags) { throw new NotImplementedException(nameof(SaveLayer)); }
        //
        // 摘要:
        //     This behaves the same as save(), but in addition it allocates and redirects drawing
        //     to an offscreen bitmap.
        //
        // 参数:
        //   bounds:
        //     May be null. The maximum size the offscreen bitmap needs to be (in local coordinates)
        //
        //   paint:
        //     This is copied, and is applied to the offscreen when restore() is called.
        //
        //   saveFlags:
        //     see _SAVE_FLAG constants, generally #ALL_SAVE_FLAG is recommended for performance
        //     reasons.
        //
        // 返回结果:
        //     value to pass to restoreToCount() to balance this save()
        [Obsolete("deprecated")]
        public virtual int SaveLayer(RectF? bounds, Paint? paint, SaveFlags saveFlags) { throw new NotImplementedException(nameof(SaveLayer)); }
        //
        // 摘要:
        //     Helper for saveLayerAlpha() that takes 4 values instead of a RectF.
        //
        // 参数:
        //   left:
        //     To be added.
        //
        //   top:
        //     To be added.
        //
        //   right:
        //     To be added.
        //
        //   bottom:
        //     To be added.
        //
        //   alpha:
        //     To be added.
        //
        //   saveFlags:
        //     To be added.
        //
        // 返回结果:
        //     To be added.
        [Obsolete("deprecated")]
        public virtual int SaveLayerAlpha(float left, float top, float right, float bottom, int alpha, SaveFlags saveFlags) { throw new NotImplementedException(nameof(SaveLayerAlpha)); }

        //
        // 摘要:
        //     Convenience for #saveLayerAlpha(RectF, int) that takes the four float coordinates
        //     of the bounds rectangle.
        //
        // 参数:
        //   left:
        //     To be added.
        //
        //   top:
        //     To be added.
        //
        //   right:
        //     To be added.
        //
        //   bottom:
        //     To be added.
        //
        //   alpha:
        //     To be added.
        //
        // 返回结果:
        //     To be added.
        public virtual int SaveLayerAlpha(float left, float top, float right, float bottom, int alpha) { throw new NotImplementedException(nameof(SaveLayerAlpha)); }

        //
        // 摘要:
        //     This behaves the same as save(), but in addition it allocates and redirects drawing
        //     to an offscreen bitmap.
        //
        // 参数:
        //   bounds:
        //     The maximum size the offscreen bitmap needs to be (in local coordinates)
        //
        //   alpha:
        //     The alpha to apply to the offscreen when it is drawn during restore()
        //
        //   saveFlags:
        //     see _SAVE_FLAG constants, generally #ALL_SAVE_FLAG is recommended for performance
        //     reasons.
        //
        // 返回结果:
        //     value to pass to restoreToCount() to balance this call
        [Obsolete("deprecated")]
        public virtual int SaveLayerAlpha(RectF? bounds, int alpha, SaveFlags saveFlags) { throw new NotImplementedException(nameof(SaveLayerAlpha)); }

        //
        // 摘要:
        //     Convenience for #saveLayer(RectF, Paint) but instead of taking a entire Paint
        //     object it takes only the alpha parameter.
        //
        // 参数:
        //   bounds:
        //     The maximum size the offscreen bitmap needs to be (in local coordinates)
        //
        //   alpha:
        //     The alpha to apply to the offscreen when it is drawn during restore()
        public virtual int SaveLayerAlpha(RectF? bounds, int alpha) { throw new NotImplementedException(nameof(SaveLayerAlpha)); }

        /// <summary>
        /// Preconcat the current matrix with the specified scale.
        /// </summary>
        /// <param name="sx">The amount to scale in X</param>
        /// <param name="sy">The amount to scale in Y</param>
        public virtual void Scale(float sx, float sy)
        {
            SKCanvas.Scale(sx, sy);
        }
 
        /// <summary>
        /// Preconcat the current matrix with the specified scale.
        /// </summary>
        /// <param name="sx">The amount to scale in X</param>
        /// <param name="sy">The amount to scale in Y</param>
        /// <param name="px">The x-coord for the pivot point (unchanged by the scale)</param>
        /// <param name="py">The y-coord for the pivot point (unchanged by the scale)</param>
        public void Scale(float sx, float sy, float px, float py)
        {
            SKCanvas.Scale(sx, sy, px, py);
        }
        //
        // 摘要:
        //     Specify a bitmap for the canvas to draw into.
        //
        // 参数:
        //   bitmap:
        //     Specifies a mutable bitmap for the canvas to draw into.
        public virtual void SetBitmap(Bitmap? bitmap) { throw new NotImplementedException(nameof(SetBitmap)); }
        //
        // 摘要:
        //     To be added.
        //
        // 参数:
        //   width:
        //     To be added.
        public virtual void SetViewport(int width, int height) { throw new NotImplementedException(nameof(SetViewport)); }
  
        /// <summary>
        /// Preconcat the current matrix with the specified skew.
        /// </summary>
        /// <param name="sx">The amount to skew in X</param>
        /// <param name="sy">The amount to skew in Y</param>
        public virtual void Skew(float sx, float sy) 
        { 
            SKCanvas.Skew(sx, sy);
        }

        /// <summary>
        /// Preconcat the current matrix with the specified translation
        /// </summary>
        /// <param name="dx">The distance to translate in X</param>
        /// <param name="dy">The distance to translate in Y</param>
        public virtual void Translate(float dx, float dy) 
        { 
            SKCanvas.Translate(dx, dy);
        }

        public void Dispose()
        {
            SKCanvas = null;
        }
    }
}
