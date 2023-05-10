using CoreGraphics;
using CoreText;
using Foundation;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
using System;

namespace Yang.Maui.Helper.Graphics
{
    public class TextWordBlock
    {
        protected const int MaxDPWidth = 1500;
        protected const int MaxDPHeight = 1500;

        protected NSMutableAttributedString mutableAttributedString;
        protected CTFramesetter frameSetter;
        protected CGPath path;
        protected CTFrame platformTextLayoutHandler;
        public CTFrame PlatformTextLayoutHandler => platformTextLayoutHandler;

        protected int? maxWidth;

        protected MauiFont font;

        protected float fontSize;

        protected Color textColor = Colors.Red;

        protected string text;

        protected float x = 20;
        protected float y = -20;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCanvas"></param>
        /// <param name="x">dp</param>
        /// <param name="y">dp</param>
        public virtual void Paint(ICanvas iCanvas, float x, float y)
        {
            var platformCanvas = iCanvas as Microsoft.Maui.Graphics.Platform.PlatformCanvas;
            var canvas = platformCanvas.Context;
            if (this.x != x && this.y != -y)
            {
                path?.Dispose();
                platformTextLayoutHandler?.Dispose();

                this.x = x;
                this.y = -y;

                path = new CGPath();
                path.AddRect(new RectF(x, -y, maxWidth != null ? maxWidth.Value : MaxDPWidth, MaxDPHeight));
                //path.CloseSubpath();
                platformTextLayoutHandler = TextLayoutUtils.GetCTFrame(frameSetter, path);
            }
            canvas.SaveState();

            canvas.TranslateCTM(0, path.BoundingBox.Height);
            canvas.ScaleCTM(1, -1f);//缩放x，y轴方向缩放，－1.0为反向1.0倍,坐标系转换,沿x轴翻转180度

            canvas.SetStrokeColor(textColor.ToPlatform().CGColor);
            canvas.TextMatrix = CGAffineTransform.MakeIdentity();
            canvas.TextMatrix.Translate(0, 0);
            //canvas.TranslateCTM(0, TextLayoutUtils.GetTextSize(platformTextLayoutHandler).Y * -2);
            platformTextLayoutHandler.Draw(canvas);

            canvas.RestoreState();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="fontSize">dp</param>
        /// <param name="maxWidth">dp</param>
        public TextWordBlock(string text, MauiFont font, float fontSize, Color textColor, int? maxWidth)
        {
            this.maxWidth = maxWidth;
            this.font = font;
            this.fontSize = fontSize;
            this.text = text;
            this.textColor = textColor;
            Init();
        }

        protected virtual void Init()
        {
            mutableAttributedString = new NSMutableAttributedString(text);

            frameSetter = TextLayoutUtils.GetCTFramesetter(mutableAttributedString, font, fontSize, textColor, HorizontalAlignment.Left, VerticalAlignment.Top);

            path = new CGPath();
            if (maxWidth != null)
                path.AddRect(new CGRect(20, -20, maxWidth.Value, MaxDPHeight));
            else
                path.AddRect(new CGRect(20, -20, MaxDPWidth, MaxDPHeight));
            //path.CloseSubpath();
            platformTextLayoutHandler = TextLayoutUtils.GetCTFrame(frameSetter, path);
        }

        /// <summary>
        /// unit is dp.
        /// </summary>
        public SizeF MeasuredSize
        {
            get
            {
                var textBounds = TextLayoutUtils.GetTextSize(platformTextLayoutHandler);
                return textBounds.Size;
            }
        }

        /// <summary>
        /// unit is dp
        /// </summary>
        public int MaxWidth
        {
            set
            {
                maxWidth = value;
                platformTextLayoutHandler?.Dispose();
                var path = new CGPath();
                path.AddRect(new RectF(x, -y, value, MaxDPHeight));
                path.CloseSubpath();
                platformTextLayoutHandler = TextLayoutUtils.GetCTFrame(frameSetter, path);
            }
        }

        public string Text
        {
            set
            {
                mutableAttributedString.Dispose();
                frameSetter?.Dispose();
                platformTextLayoutHandler?.Dispose();

                text = value;

                Init();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateFont(MauiFont font, float? fontSize, Color textColor)
        {
            if (font != null) this.font = font;
            if (fontSize != null) this.fontSize = fontSize.Value;
            if (textColor != null) this.textColor = textColor;
            frameSetter?.Dispose();
            platformTextLayoutHandler?.Dispose();

            frameSetter = TextLayoutUtils.GetCTFramesetter(mutableAttributedString, this.font, this.fontSize, this.textColor, HorizontalAlignment.Left, VerticalAlignment.Top);
            platformTextLayoutHandler = TextLayoutUtils.GetCTFrame(frameSetter, path);
        }
    }

    public static class TextLayoutUtils
    {
        public static CTFramesetter GetCTFramesetter(NSMutableAttributedString attributedString, MauiFont font, float fontSize, Color fontColor, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            var attributes = new CTStringAttributes();

            // Create a color and add it as an attribute to the string.
            attributes.ForegroundColor = new CGColor(fontColor.Red, fontColor.Green, fontColor.Blue, fontColor.Alpha);

            attributes.Font = font?.GetPlatformFont_CTFont(fontSize) ?? Microsoft.Maui.Graphics.Platform.FontExtensions.GetDefaultCTFont(fontSize);

            // Set the horizontal alignment
            var paragraphSettings = new CTParagraphStyleSettings();
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    paragraphSettings.Alignment = CTTextAlignment.Left;
                    break;
                case HorizontalAlignment.Center:
                    paragraphSettings.Alignment = CTTextAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    paragraphSettings.Alignment = CTTextAlignment.Right;
                    break;
                case HorizontalAlignment.Justified:
                    paragraphSettings.Alignment = CTTextAlignment.Justified;
                    break;
            }

            var paragraphStyle = new CTParagraphStyle(paragraphSettings);
            attributes.ParagraphStyle = paragraphStyle;

            // Set the attributes for the complete length of the string
            attributedString.SetAttributes(attributes, new NSRange(0, attributedString.Length));

            // Create the framesetter with the attributed string.
            var frameSetter = new CTFramesetter(attributedString);

            return frameSetter;

            /*frameSetter.Dispose();
            attributedString.Dispose();
            paragraphStyle.Dispose();
            path.Dispose();*/
        }

        public static CTFrame GetCTFrame(CTFramesetter frameSetter, CGPath path)
        {
            var frame = frameSetter.GetFrame(new NSRange(0, 0), path, null);
            return frame;
        }

        public static RectF GetTextSize(CTFrame frame)
        {
            var minY = float.MaxValue;
            var maxY = float.MinValue;
            float width = 0;

            var lines = frame.GetLines();
            var origins = new CGPoint[lines.Length];
            frame.GetLineOrigins(new NSRange(0, 0), origins);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var lineWidth = (float)line.GetTypographicBounds(out var ascent, out var descent, out var leading);

                if (lineWidth > width)
                    width = lineWidth;

                var origin = origins[i];

                minY = (float)Math.Min(minY, origin.Y - ascent);
                maxY = (float)Math.Max(maxY, origin.Y + descent);

                lines[i].Dispose();
            }

            return new RectF(0f, minY, width, Math.Max(0, maxY - minY));
        }
    }
}
