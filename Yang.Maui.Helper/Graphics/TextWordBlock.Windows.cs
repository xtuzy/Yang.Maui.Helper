using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Platform;
using Windows.UI.Text;
using HorizontalAlignment = Microsoft.Maui.Graphics.HorizontalAlignment;
using VerticalAlignment = Microsoft.Maui.Graphics.VerticalAlignment;

namespace Yang.Maui.Helper.Graphics
{
    public class TextWordBlock
    {
        CanvasTextLayout platformTextLayoutHandler;
        public CanvasTextLayout PlatformTextLayoutHandler => platformTextLayoutHandler;

        int? maxWidth;

        MauiFont font;

        float fontSize;

        Color textColor = Colors.Red;

        string text;

        public TextWordBlock(string text, MauiFont font, float fontSize, Color textColor, int? maxWidth)
        {
            this.maxWidth = maxWidth;
            this.font = font;
            this.fontSize = fontSize;
            this.text = text;
            this.textColor = textColor;
            Init();
        }

        private void Init()
        {
            var format = new CanvasTextFormat
            {
                FontSize = fontSize,
                WordWrapping = CanvasWordWrapping.NoWrap
            };
            if (font != null)
            {
                format.FontFamily = font.GetPlatformFont()?.Source;
                format.FontWeight = new Windows.UI.Text.FontWeight { Weight = (ushort)font.VirtualFont.Weight };
                format.FontStyle = font.VirtualFont.ToFontStyle();
            }
            
            var device = CanvasDevice.GetSharedDevice();
            platformTextLayoutHandler = TextLayoutUtils.GetCanvasTextLayout(text, device, maxWidth == null ? 0 : maxWidth.Value, format, HorizontalAlignment.Left, VerticalAlignment.Top);
        }

        public SizeF MeasuredSize
        {
            get
            {
                return new SizeF((float)platformTextLayoutHandler.LayoutBounds.Width, (float)platformTextLayoutHandler.LayoutBounds.Height); //LayoutBounds貌似和iOS匹配
            }
        }

        public void Paint(ICanvas iCanvas, float x, float y)
        {
            var session = (iCanvas as W2DCanvas).Session;
            session.DrawTextLayout(platformTextLayoutHandler,
                new System.Numerics.Vector2(x, y),
                new CanvasSolidColorBrush(session, textColor.AsColor(Colors.Black)));
        }
    }

    internal static class TextLayoutUtils
    {
        public static CanvasTextLayout GetCanvasTextLayout(string value, CanvasDevice device, float limiteWidth, CanvasTextFormat format, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            var textLayout = new CanvasTextLayout(device, value, format, limiteWidth, 0.0f);
            textLayout.VerticalAlignment = verticalAlignment switch
            {
                VerticalAlignment.Top => CanvasVerticalAlignment.Top,
                VerticalAlignment.Center => CanvasVerticalAlignment.Center,
                VerticalAlignment.Bottom => CanvasVerticalAlignment.Bottom,
                _ => CanvasVerticalAlignment.Top
            };
            textLayout.HorizontalAlignment = horizontalAlignment switch
            {
                HorizontalAlignment.Left => CanvasHorizontalAlignment.Left,
                HorizontalAlignment.Center => CanvasHorizontalAlignment.Center,
                HorizontalAlignment.Right => CanvasHorizontalAlignment.Right,
                HorizontalAlignment.Justified => CanvasHorizontalAlignment.Justified,
                _ => CanvasHorizontalAlignment.Left,
            };
            return textLayout;
        }

        public static FontStyle ToFontStyle(Microsoft.Maui.Graphics.IFont font) =>
                font.StyleType switch
                {
                    FontStyleType.Italic => FontStyle.Italic,
                    FontStyleType.Oblique => FontStyle.Oblique,
                    _ => FontStyle.Normal,
                };
    }
}
