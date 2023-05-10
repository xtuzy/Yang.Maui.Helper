using Android.Graphics;
using Android.Text;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using Layout = Android.Text.Layout;

namespace Yang.Maui.Helper.Graphics
{
    /// <summary>
    /// more high-efficiency use Microsoft.Maui.Graphics.
    /// 在使用图形Api时, 如果需要先测量文本大小, 会造成多次创建StaticLayout, TextPaint, 这个类目的是减少它们.
    /// </summary>
    public partial class TextWordBlock
    {
        StaticLayout platformTextLayoutHandler;
        public StaticLayout PlatformTextLayoutHandler => platformTextLayoutHandler;

        //pixel
        int? maxWidth;

        MauiFont font;

        //pixel
        float fontSize;

        string text;

        private TextPaint textPaint;
        public TextPaint TextPaint => textPaint;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="fontSize">dp</param>
        /// <param name="maxWidth">dp</param>
        public TextWordBlock(string text, MauiFont font, float fontSize, Color textColor, int? maxWidth)
        {
            this.maxWidth = maxWidth != null ? (int)(maxWidth.Value * TextLayoutUtils.Density) : null;
            this.font = font;
            this.fontSize = fontSize * TextLayoutUtils.Density;
            this.text = text;

            textPaint = new TextPaint { TextSize = this.fontSize };
            textPaint.SetTypeface(font?.GetPlatformFont() ?? Typeface.Default);
            this.TextColor = textColor;

            platformTextLayoutHandler = GetPlatformTextLayoutHandler(text, textPaint, this.maxWidth);
        }

        /// <summary>
        /// dp
        /// </summary>
        public SizeF MeasuredSize
        {
            get
            {
                var size = platformTextLayoutHandler.GetTextSize(false);//只使用最宽的一行作为宽, 为true时会直接使用约束宽, 那不对
                return new SizeF(size.Width / TextLayoutUtils.Density, size.Height / TextLayoutUtils.Density);
            }
        }

        /// <summary>
        /// text top at (0,0)
        /// </summary>
        /// <param name="iCanvas"></param>
        /// <param name="x">dp</param>
        /// <param name="y">dp</param>
        public void Paint(ICanvas iCanvas, float x, float y)
        {
            PlatformCanvas platformCanvas = (iCanvas as ScalingCanvas).ParentCanvas as Microsoft.Maui.Graphics.Platform.PlatformCanvas;
            var canvas = platformCanvas.Canvas;
            canvas.Save();
            canvas.Translate(x * TextLayoutUtils.Density, y * TextLayoutUtils.Density);
            platformTextLayoutHandler.Draw(canvas);
            canvas.Restore();
        }

        /// <summary>
        /// unit is pixel.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="constrainWidth">pixel</param>
        /// <returns></returns>
        private StaticLayout GetPlatformTextLayoutHandler(string value, TextPaint textPaint, int? constrainWidth)
        {
            if (value == null)
                return null;

            var staticLayout = TextLayoutUtils.CreateLayout(value, textPaint, constrainWidth, Android.Text.Layout.Alignment.AlignNormal);
            return staticLayout;
        }

        /// <summary>
        /// dp
        /// </summary>
        public int MaxWidth
        {
            set
            {
                var newValue = (int)(value * TextLayoutUtils.Density);

                if (maxWidth == null || (maxWidth != null && maxWidth > newValue))//减小时重建
                {
                    platformTextLayoutHandler?.Dispose();
                    platformTextLayoutHandler = GetPlatformTextLayoutHandler(text, textPaint, newValue);
                }
                else
                    platformTextLayoutHandler.IncreaseWidthTo(newValue);
                maxWidth = newValue;
            }
        }

        public string Text
        {
            set
            {
                platformTextLayoutHandler?.Dispose();
                text = value;
                platformTextLayoutHandler = GetPlatformTextLayoutHandler(value, textPaint, maxWidth);
            }
        }

        public Color TextColor
        {
            set => textPaint.Color = value.ToPlatform();
        }

        /// <summary>
        /// dp
        /// </summary>
        public float FontSize
        {
            set => textPaint.TextSize = value * TextLayoutUtils.Density;
        }

        public MauiFont Font
        {
            set => textPaint.SetTypeface(value?.GetPlatformFont() ?? Typeface.Default);
        }
    }

    internal static class TextLayoutUtils
    {
        static float density;
        internal static float Density
        {
            get
            {
                if (density == default)
                    density = (float)DeviceDisplay.Current.MainDisplayInfo.Density;
                return density;
            }
        }

        public static StaticLayout CreateLayout(string text, TextPaint textPaint, int? boundedWidth, Layout.Alignment alignment)
        {
            int finalWidth = int.MaxValue;
            if (boundedWidth > 0)
                finalWidth = (int)boundedWidth;

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1422 // Validate platform compatibility
            var layout = new StaticLayout(
                text, // Text to layout
                textPaint, // Text paint (font, size, etc...) to use
                finalWidth, // The maximum width the text can be
                alignment, // The horizontal alignment of the text
                1.0f, // Spacing multiplier
                0.0f, // Additional spacing
                false); // Include padding
#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility

            return layout;
        }

        public static StaticLayout CreateLayoutForSpannedString(SpannableString spannedString, TextPaint textPaint, int? boundedWidth, Layout.Alignment alignment)
        {
            int finalWidth = int.MaxValue;
            if (boundedWidth > 0)
                finalWidth = (int)boundedWidth;

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1422 // Validate platform compatibility
            var layout = new StaticLayout(
                spannedString, // Text to layout
                textPaint, // Text paint (font, size, etc...) to use
                finalWidth, // The maximum width the text can be
                alignment, // The horizontal alignment of the text
                1.0f, // Spacing multiplier
                0.0f, // Additional spacing
                false); // Include padding
#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CS0618 // Type or member is obsolete

            return layout;
        }

        public static SizeF GetTextSize(this StaticLayout target)
        {
            // Get the text bounds and assume (the safe assumption) that the layout wasn't
            // created with a bounded width.
            return GetTextSize(target, false);
        }

        public static SizeF GetTextSize(this StaticLayout target, bool hasBoundedWidth)
        {
            // We need to know if the static layout was created with a bounded width, as this is what
            // StaticLayout.Width returns.
            if (hasBoundedWidth)
                return new SizeF(target.Width, target.Height);

            float vMaxWidth = 0;
            int vLineCount = target.LineCount;

            for (int i = 0; i < vLineCount; i++)
            {
                float vLineWidth = target.GetLineWidth(i);
                if (vLineWidth > vMaxWidth)
                    vMaxWidth = vLineWidth;
            }

            return new SizeF(vMaxWidth, target.Height);
        }
    }
}
