using SkiaSharp;
using System;
using Bitmap = SkiaSharp.SKBitmap;
using BlendMode = SkiaSharp.SKBlendMode;
using Color = SkiaSharp.SKColor;
using EdgeType = System.Object;
using Font = SkiaSharp.SKFont;
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

namespace Yang.Maui.Helper.SkiaExtension
{
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

        //
        // 摘要:
        //     Set the clip to the difference of the current clip and the specified rectangle,
        //     which is expressed in local coordinates.
        //
        // 参数:
        //   left:
        //     The left side of the rectangle used in the difference operation
        //
        //   top:
        //     The top of the rectangle used in the difference operation
        //
        //   right:
        //     The right side of the rectangle used in the difference operation
        //
        //   bottom:
        //     The bottom of the rectangle used in the difference operation
        //
        // 返回结果:
        //     true if the resulting clip is non-empty
        public virtual void ClipOutRect(int left, int top, int right, int bottom)
        {
            SKCanvas.ClipRect(new RectF(left, top, right, bottom), SKClipOperation.Difference);
        }
        //
        // 摘要:
        //     Set the clip to the difference of the current clip and the specified rectangle,
        //     which is expressed in local coordinates.
        //
        // 参数:
        //   left:
        //     The left side of the rectangle used in the difference operation
        //
        //   top:
        //     The top of the rectangle used in the difference operation
        //
        //   right:
        //     The right side of the rectangle used in the difference operation
        //
        //   bottom:
        //     The bottom of the rectangle used in the difference operation
        //
        // 返回结果:
        //     true if the resulting clip is non-empty
        public virtual void ClipOutRect(float left, float top, float right, float bottom)
        {
            SKCanvas.ClipRect(new RectF(left, top, right, bottom), SKClipOperation.Difference);
        }
        //
        // 摘要:
        //     Set the clip to the difference of the current clip and the specified rectangle,
        //     which is expressed in local coordinates.
        //
        // 参数:
        //   rect:
        //     The rectangle to perform a difference op with the current clip.
        //
        // 返回结果:
        //     true if the resulting clip is non-empty
        public virtual void ClipOutRect(RectF rect)
        {
            SKCanvas.ClipRect(rect, SKClipOperation.Difference);
        }
        //
        // 摘要:
        //     Set the clip to the difference of the current clip and the specified rectangle,
        //     which is expressed in local coordinates.
        //
        // 参数:
        //   rect:
        //     The rectangle to perform a difference op with the current clip.
        //
        // 返回结果:
        //     true if the resulting clip is non-empty
        public virtual void ClipOutRect(Rect rect)
        {
            SKCanvas.ClipRect(rect, SKClipOperation.Difference);
        }
        [Obsolete("deprecated")]
        public virtual void ClipPath(Path path, RegionOp op) { throw new NotImplementedException(nameof(ClipRect)); }
        //
        // 摘要:
        //     Intersect the current clip with the specified path.
        //
        // 参数:
        //   path:
        //     The path to intersect with the current clip
        //
        // 返回结果:
        //     true if the resulting clip is non-empty
        public virtual void ClipPath(Path path)
        {
            SKCanvas.ClipPath(path, SKClipOperation.Intersect);
        }
        //
        // 摘要:
        //     Intersect the current clip with the specified rectangle, which is expressed in
        //     local coordinates.
        //
        // 参数:
        //   left:
        //     The left side of the rectangle to intersect with the current clip
        //
        //   top:
        //     The top of the rectangle to intersect with the current clip
        //
        //   right:
        //     The right side of the rectangle to intersect with the current clip
        //
        //   bottom:
        //     The bottom of the rectangle to intersect with the current clip
        //
        // 返回结果:
        //     true if the resulting clip is non-empty
        public virtual void ClipRect(int left, int top, int right, int bottom)
        {
            SKCanvas.ClipRect(new Rect(left, top, right, bottom), SKClipOperation.Intersect);
        }
        //
        // 摘要:
        //     Intersect the current clip with the specified rectangle, which is expressed in
        //     local coordinates.
        //
        // 参数:
        //   left:
        //     The left side of the rectangle to intersect with the current clip
        //
        //   top:
        //     The top of the rectangle to intersect with the current clip
        //
        //   right:
        //     The right side of the rectangle to intersect with the current clip
        //
        //   bottom:
        //     The bottom of the rectangle to intersect with the current clip
        //
        // 返回结果:
        //     true if the resulting clip is non-empty
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
        //
        // 摘要:
        //     Intersect the current clip with the specified rectangle, which is expressed in
        //     local coordinates.
        //
        // 参数:
        //   rect:
        //     The rectangle to intersect with the current clip.
        //
        // 返回结果:
        //     true if the resulting clip is non-empty
        public virtual void ClipRect(Rect rect)
        {
            SKCanvas.ClipRect(rect, SKClipOperation.Intersect);
        }
        [Obsolete("deprecated")]
        public virtual bool ClipRect(float left, float top, float right, float bottom, RegionOp op) { throw new NotImplementedException(nameof(ClipRect)); }
        [Obsolete("deprecated")]
        public virtual bool ClipRegion(Region? region, RegionOp? op) { throw new NotImplementedException(nameof(ClipRegion)); }
        //
        // 摘要:
        //     Intersect the current clip with the specified region.
        //
        // 参数:
        //   region:
        //     The region to operate on the current clip, based on op
        //
        // 返回结果:
        //     To be added.
        [Obsolete("deprecated")]
        public virtual bool ClipRegion(Region? region) { throw new NotImplementedException(nameof(ClipRegion)); }
        //
        // 摘要:
        //     Preconcat the current matrix with the specified matrix.
        //
        // 参数:
        //   matrix:
        //     The matrix to preconcatenate with the current matrix
        public virtual void Concat(ref Matrix matrix)
        {
            SKCanvas.Concat(ref matrix);
        }

        //
        // 摘要:
        //     Disables Z support, preventing any RenderNodes drawn after this point from being
        //     visually reordered or having shadows rendered.
        public virtual void DisableZ() { throw new NotImplementedException(); }

        //
        // 摘要:
        //     Draw the specified arc, which will be scaled to fit inside the specified oval.
        //
        // 参数:
        //   oval:
        //     The bounds of oval used to define the shape and size of the arc
        //
        //   startAngle:
        //     Starting angle (in degrees) where the arc begins
        //
        //   sweepAngle:
        //     Sweep angle (in degrees) measured clockwise
        //
        //   useCenter:
        //     If true, include the center of the oval in the arc, and close it if it is being
        //     stroked. This will draw a wedge
        //
        //   paint:
        //     The paint used to draw the arc
        public virtual void DrawArc(RectF oval, float startAngle, float sweepAngle, bool useCenter, Paint paint)
        {
            SKCanvas.DrawArc(oval, startAngle, sweepAngle, useCenter, paint);
        }
        //
        // 摘要:
        //     Draw the specified arc, which will be scaled to fit inside the specified oval.
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
        //   startAngle:
        //     Starting angle (in degrees) where the arc begins
        //
        //   sweepAngle:
        //     Sweep angle (in degrees) measured clockwise
        //
        //   useCenter:
        //     If true, include the center of the oval in the arc, and close it if it is being
        //     stroked. This will draw a wedge
        //
        //   paint:
        //     The paint used to draw the arc
        public virtual void DrawArc(float left, float top, float right, float bottom, float startAngle, float sweepAngle, bool useCenter, Paint paint)
        {
            DrawArc(new RectF(left, top, right, bottom), startAngle, sweepAngle, useCenter, paint);
        }
        //
        // 摘要:
        //     Fill the entire canvas' bitmap (restricted to the current clip) with the specified
        //     ARGB color, using srcover porterduff mode.
        //
        // 参数:
        //   a:
        //     alpha component (0..255) of the color to draw onto the canvas
        //
        //   r:
        //     red component (0..255) of the color to draw onto the canvas
        //
        //   g:
        //     green component (0..255) of the color to draw onto the canvas
        //
        //   b:
        //     blue component (0..255) of the color to draw onto the canvas
        public virtual void DrawARGB(int a, int r, int g, int b)
        {
            DrawColor(new SKColor((byte)r, (byte)g, (byte)b, (byte)a));
        }
        //
        // 摘要:
        //     Draw the specified bitmap, scaling/translating automatically to fill the destination
        //     rectangle.
        //
        // 参数:
        //   bitmap:
        //     The bitmap to be drawn
        //
        //   src:
        //     May be null. The subset of the bitmap to be drawn
        //
        //   dst:
        //     The rectangle that the bitmap will be scaled/translated to fit into
        //
        //   paint:
        //     May be null. The paint used to draw the bitmap
        public virtual void DrawBitmap(Bitmap bitmap, Rect src, RectF dst, Paint? paint)
        {
            SKCanvas.DrawBitmap(bitmap, src, dst, paint);
        }
        //
        // 摘要:
        //     Legacy version of drawBitmap(int[] colors, .
        //
        // 参数:
        //   colors:
        //     To be added.
        //
        //   offset:
        //     To be added.
        //
        //   stride:
        //     To be added.
        //
        //   x:
        //     To be added.
        //
        //   y:
        //     To be added.
        //
        //   width:
        //     To be added.
        //
        //   height:
        //     To be added.
        //
        //   hasAlpha:
        //     To be added.
        //
        //   paint:
        //     The paint used to draw the bitmap (may be null)
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
        //
        // 摘要:
        //     Draw the specified bitmap, with its top/left corner at (x,y), using the specified
        //     paint, transformed by the current matrix.
        //
        // 参数:
        //   bitmap:
        //     The bitmap to be drawn
        //
        //   left:
        //     The position of the left side of the bitmap being drawn
        //
        //   top:
        //     The position of the top side of the bitmap being drawn
        //
        //   paint:
        //     The paint used to draw the bitmap (may be null)
        public virtual void DrawBitmap(Bitmap bitmap, float left, float top, Paint? paint)
        {
            SKCanvas.DrawBitmap(bitmap, left, top, paint);
        }
        //
        // 摘要:
        //     Draw the specified bitmap, scaling/translating automatically to fill the destination
        //     rectangle.
        //
        // 参数:
        //   bitmap:
        //     The bitmap to be drawn
        //
        //   src:
        //     May be null. The subset of the bitmap to be drawn
        //
        //   dst:
        //     The rectangle that the bitmap will be scaled/translated to fit into
        //
        //   paint:
        //     May be null. The paint used to draw the bitmap
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
        //
        // 摘要:
        //     Draw the specified circle using the specified paint.
        //
        // 参数:
        //   cx:
        //     The x-coordinate of the center of the cirle to be drawn
        //
        //   cy:
        //     The y-coordinate of the center of the cirle to be drawn
        //
        //   radius:
        //     The radius of the cirle to be drawn
        //
        //   paint:
        //     The paint used to draw the circle
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
        //
        // 摘要:
        //     Fill the entire canvas' bitmap (restricted to the current clip) with the specified
        //     color, using srcover porterduff mode.
        //
        // 参数:
        //   color:
        //     the color to draw onto the canvas
        public virtual void DrawColor(Color color)
        {
            SKCanvas.DrawColor(color, BlendMode.SrcOver);
        }
        //
        // 摘要:
        //     Fill the entire canvas' bitmap (restricted to the current clip) with the specified
        //     color and blendmode.
        //
        // 参数:
        //   color:
        //     the color to draw onto the canvas
        //
        //   mode:
        //     the blendmode to apply to the color
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
        //
        // 摘要:
        //     Draw a line segment with the specified start and stop x,y coordinates, using
        //     the specified paint.
        //
        // 参数:
        //   stopX:
        //     To be added.
        //
        //   stopY:
        //     To be added.
        //
        //   startX:
        //     The x-coordinate of the start point of the line
        //
        //   startY:
        //     The y-coordinate of the start point of the line
        //
        //   paint:
        //     The paint used to draw the line
        public virtual void DrawLine(float startX, float startY, float stopX, float stopY, Paint paint)
        {
            SKCanvas.DrawLine(startX, startY, stopX, stopY, paint);
        }
        //
        // 摘要:
        //     Draw a series of lines.
        //
        // 参数:
        //   pts:
        //     Array of points to draw [x0 y0 x1 y1 x2 y2 ...]
        //
        //   paint:
        //     The paint used to draw the points
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
        //
        // 摘要:
        //     Draw the specified oval using the specified paint.
        //
        // 参数:
        //   paint:
        //     To be added.
        //
        //   oval:
        //     The rectangle bounds of the oval to be drawn
        public virtual void DrawOval(RectF oval, Paint paint)
        {
            SKCanvas.DrawOval(oval, paint);
        }
        //
        // 摘要:
        //     Draw the specified oval using the specified paint.
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
        public virtual void DrawOval(float left, float top, float right, float bottom, Paint paint)
        {
            SKCanvas.DrawOval(new RectF(left, right, top, bottom), paint);
        }
        //
        // 摘要:
        //     Fill the entire canvas' bitmap (restricted to the current clip) with the specified
        //     paint.
        //
        // 参数:
        //   paint:
        //     The paint used to draw onto the canvas
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
        //
        // 摘要:
        //     Draw the specified path using the specified paint.
        //
        // 参数:
        //   path:
        //     The path to be drawn
        //
        //   paint:
        //     The paint used to draw the path
        public virtual void DrawPath(Path path, Paint paint)
        {
            SKCanvas.DrawPath(path, paint);
        }
        //
        // 摘要:
        //     Save the canvas state, draw the picture, and restore the canvas state.
        //
        // 参数:
        //   picture:
        //     The picture to be drawn
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
        //
        // 摘要:
        //     Helper for drawPoints() for drawing a single point.
        //
        // 参数:
        //   x:
        //     To be added.
        //
        //   y:
        //     To be added.
        //
        //   paint:
        //     To be added.
        public virtual void DrawPoint(float x, float y, Paint paint)
        {
            SKCanvas.DrawPoint(x, y, paint);
        }
        //
        // 摘要:
        //     Helper for drawPoints() that assumes you want to draw the entire array
        //
        // 参数:
        //   pts:
        //     Array of points to draw [x0 y0 x1 y1 x2 y2 ...]
        //
        //   paint:
        //     The paint used to draw the points
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
        //
        // 摘要:
        //     Draw the specified Rect using the specified paint.
        //
        // 参数:
        //   left:
        //     The left side of the rectangle to be drawn
        //
        //   top:
        //     The top side of the rectangle to be drawn
        //
        //   right:
        //     The right side of the rectangle to be drawn
        //
        //   bottom:
        //     The bottom side of the rectangle to be drawn
        //
        //   paint:
        //     The paint used to draw the rect
        public virtual void DrawRect(float left, float top, float right, float bottom, Paint paint)
        {
            SKCanvas.DrawRect(new RectF(left, top, right, bottom), paint);
        }
        //
        // 摘要:
        //     Draw the specified Rect using the specified paint.
        //
        // 参数:
        //   rect:
        //     The rect to be drawn
        //
        //   paint:
        //     The paint used to draw the rect
        public virtual void DrawRect(RectF rect, Paint paint)
        {
            SKCanvas.DrawRect(rect, paint);
        }
        //
        // 摘要:
        //     Draw the specified Rect using the specified Paint.
        //
        // 参数:
        //   r:
        //     The rectangle to be drawn.
        //
        //   paint:
        //     The paint used to draw the rectangle
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
        //
        // 摘要:
        //     Fill the entire canvas' bitmap (restricted to the current clip) with the specified
        //     RGB color, using srcover porterduff mode.
        //
        // 参数:
        //   r:
        //     red component (0..255) of the color to draw onto the canvas
        //
        //   g:
        //     green component (0..255) of the color to draw onto the canvas
        //
        //   b:
        //     blue component (0..255) of the color to draw onto the canvas
        public virtual void DrawRGB(int r, int g, int b)
        {
            SKCanvas.DrawColor(new SKColor((byte)r, (byte)g, (byte)b));
        }
        //
        // 摘要:
        //     Draw the specified round-rect using the specified paint.
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
        //   rx:
        //     The x-radius of the oval used to round the corners
        //
        //   ry:
        //     The y-radius of the oval used to round the corners
        //
        //   paint:
        //     The paint used to draw the roundRect
        public virtual void DrawRoundRect(float left, float top, float right, float bottom, float rx, float ry, Paint paint)
        {
            SKCanvas.DrawRoundRect(new SKRect(left, top, right, bottom), rx, ry, paint);
        }
        //
        // 摘要:
        //     Draw the specified round-rect using the specified paint.
        //
        // 参数:
        //   rect:
        //     The rectangular bounds of the roundRect to be drawn
        //
        //   rx:
        //     The x-radius of the oval used to round the corners
        //
        //   ry:
        //     The y-radius of the oval used to round the corners
        //
        //   paint:
        //     The paint used to draw the roundRect
        public virtual void DrawRoundRect(RectF rect, float rx, float ry, Paint paint)
        {
            SKCanvas.DrawRoundRect(rect, rx, ry, paint);
        }
        //
        // 摘要:
        //     Draw the text, with origin at (x,y), using the specified paint.
        //
        // 参数:
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
        //
        // 摘要:
        //     Draw the text, with origin at (x,y), using the specified paint.
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
        //     The x-coordinate of the origin of the text being drawn
        //
        //   y:
        //     The y-coordinate of the baseline of the text being drawn
        //
        //   paint:
        //     The paint used for the text (e.g. color, size, style)
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
        //
        // 摘要:
        //     Draw the text, with origin at (x,y), using the specified paint, along the specified
        //     path.
        //
        // 参数:
        //   text:
        //     The text to be drawn
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
        //
        // 摘要:
        //     Return the bounds of the current clip (in local coordinates) in the bounds parameter,
        //     and return true if it is non-empty.
        //
        // 参数:
        //   bounds:
        //     Return the clip bounds here. If it is null, ignore it but still return true if
        //     the current clip is non-empty.
        //
        // 返回结果:
        //     true if the current clip is non-empty.
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
        //
        // 摘要:
        //     Return true if the specified path, after being transformed by the current matrix,
        //     would lie completely outside of the current clip.
        //
        // 参数:
        //   path:
        //     The path to compare with the current clip
        //
        // 返回结果:
        //     true if the path (transformed by the canvas' matrix) does not intersect with
        //     the canvas' clip
        public virtual bool QuickReject(Path path)
        {
            return SKCanvas.QuickReject(path);
        }
        [Obsolete("deprecated")]
        public virtual bool QuickReject(Path path, EdgeType type) { throw new NotImplementedException(nameof(QuickReject)); }
        //
        // 摘要:
        //     Return true if the specified rectangle, after being transformed by the current
        //     matrix, would lie completely outside of the current clip.
        //
        // 参数:
        //   rect:
        //     the rect to compare with the current clip
        //
        // 返回结果:
        //     true if the rect (transformed by the canvas' matrix) does not intersect with
        //     the canvas' clip
        public virtual bool QuickReject(RectF rect)
        {
            return SKCanvas.QuickReject(rect);
        }
        [Obsolete("deprecated")]
        public virtual bool QuickReject(RectF rect, EdgeType type) { throw new NotImplementedException(nameof(QuickReject)); }
        //
        // 摘要:
        //     Return true if the specified rectangle, after being transformed by the current
        //     matrix, would lie completely outside of the current clip.
        //
        // 参数:
        //   left:
        //     The left side of the rectangle to compare with the current clip
        //
        //   top:
        //     The top of the rectangle to compare with the current clip
        //
        //   right:
        //     The right side of the rectangle to compare with the current clip
        //
        //   bottom:
        //     The bottom of the rectangle to compare with the current clip
        //
        // 返回结果:
        //     true if the rect (transformed by the canvas' matrix) does not intersect with
        //     the canvas' clip
        public virtual bool QuickReject(float left, float top, float right, float bottom)
        {
            return SKCanvas.QuickReject(new RectF(left, top, right, bottom));
        }
        [Obsolete("deprecated")]
        public virtual bool QuickReject(float left, float top, float right, float bottom, EdgeType type) { throw new NotImplementedException(nameof(QuickReject)); }
        //
        // 摘要:
        //     This call balances a previous call to save(), and is used to remove all modifications
        //     to the matrix/clip state since the last save call.
        public virtual void Restore()
        {
            SKCanvas.Restore();
        }
        //
        // 摘要:
        //     Efficient way to pop any calls to save() that happened after the save count reached
        //     saveCount.
        //
        // 参数:
        //   saveCount:
        //     The save level to restore to.
        public virtual void RestoreToCount(int saveCount)
        {
            SKCanvas.RestoreToCount(saveCount);
        }
        //
        // 摘要:
        //     Preconcat the current matrix with the specified rotation.
        //
        // 参数:
        //   degrees:
        //     The amount to rotate, in degrees
        //
        //   px:
        //     The x-coord for the pivot point (unchanged by the rotation)
        //
        //   py:
        //     The y-coord for the pivot point (unchanged by the rotation)
        public void Rotate(float degrees, float px, float py)
        {
            SKCanvas.RotateDegrees(degrees, px, py);
        }
        //
        // 摘要:
        //     Preconcat the current matrix with the specified rotation.
        //
        // 参数:
        //   degrees:
        //     The amount to rotate, in degrees
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
        //
        // 摘要:
        //     Saves the current matrix and clip onto a private stack.
        //
        // 返回结果:
        //     The value to pass to restoreToCount() to balance this save()
        public virtual int Save()
        {
            return SKCanvas.Save();
        }
        //
        // 摘要:
        //     Convenience for #saveLayer(RectF, Paint) that takes the four float coordinates
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
        //   paint:
        //     To be added.
        //
        // 返回结果:
        //     To be added.
        public virtual int SaveLayer(float left, float top, float right, float bottom, Paint? paint)
        {
            return SKCanvas.SaveLayer(new RectF(left, top, right, bottom), paint);
        }
        //
        // 摘要:
        //     This behaves the same as save(), but in addition it allocates and redirects drawing
        //     to an offscreen rendering target.
        //
        // 参数:
        //   bounds:
        //     May be null. The maximum size the offscreen render target needs to be (in local
        //     coordinates)
        //
        //   paint:
        //     This is copied, and is applied to the offscreen when restore() is called.
        //
        // 返回结果:
        //     value to pass to restoreToCount() to balance this save()
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
        //
        // 摘要:
        //     Preconcat the current matrix with the specified scale.
        //
        // 参数:
        //   sx:
        //     The amount to scale in X
        //
        //   sy:
        //     The amount to scale in Y
        public virtual void Scale(float sx, float sy)
        {
            SKCanvas.Scale(sx, sy);
        }
        //
        // 摘要:
        //     Preconcat the current matrix with the specified scale.
        //
        // 参数:
        //   sx:
        //     The amount to scale in X
        //
        //   sy:
        //     The amount to scale in Y
        //
        //   px:
        //     The x-coord for the pivot point (unchanged by the scale)
        //
        //   py:
        //     The y-coord for the pivot point (unchanged by the scale)
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
        //
        // 摘要:
        //     Preconcat the current matrix with the specified skew.
        //
        // 参数:
        //   sx:
        //     The amount to skew in X
        //
        //   sy:
        //     The amount to skew in Y
        public virtual void Skew(float sx, float sy) 
        { 
            SKCanvas.Skew(sx, sy);
        }
        //
        // 摘要:
        //     Preconcat the current matrix with the specified translation
        //
        // 参数:
        //   dx:
        //     The distance to translate in X
        //
        //   dy:
        //     The distance to translate in Y
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
