using CoreGraphics;
using System.Runtime.InteropServices;
using UIKit;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    internal class UITableViewSectionLabel : UILabel
    {
        private string headerTitle;

        public UITableViewSectionLabel(string title)
        {
            this.headerTitle = title;
            Text = $"  {title}";
            Font = UIFont.BoldSystemFontOfSize(17);
            TextColor = UIColor.White;
            ShadowColor = new UIColor(100 / 255.0f, green: 105 / 255.0f, blue: 110 / 255.0f, alpha: 1);
            ShadowOffset = new CGSize(0, 1);
        }

        public override void Draw(CGRect rect)
        {
            CGSize size = this.Bounds.Size;

            new UIColor(166 / 255.0f, green: 177 / 255.0f, blue: 187 / 255.0f, alpha: 1).SetFill();
            UIGraphics.RectFill(new CGRect(0f, 0f, size.Width, 1.0f));
            UIColor startColor = new UIColor(145 / 255.0f, green: 158 / 255.0f, blue: 171 / 255.0f, alpha: 1);
            UIColor endColor = new UIColor(185 / 255.0f, green: 193 / 255.0f, blue: 201 / 255.0f, alpha: 1);

            CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();
            NFloat[] locations = new NFloat[2] { 0, 1 };
            CGColor[] colors = new CGColor[2] { startColor.CGColor, endColor.CGColor };
            //CFArrayRef gradientColors = CFArrayCreate(NULL, colors, 2, NULL);
            //CGGradient gradient = new CGGradient(colorSpace, gradientColors, locations);
            CGGradient gradient = new CGGradient(colorSpace, colors, locations);

            colorSpace.Dispose();

            UIGraphics.GetCurrentContext().DrawLinearGradient(gradient, new CGPoint(0f, 1.0f), new CGPoint(0f, size.Height - 1.0f), 0);
            gradient.Dispose();
            //CFRelease(gradientColors);
            new UIColor(153 / 255.0f, green: 158 / 255.0f, blue: 165 / 255.0f, alpha: 1).SetFill();
            UIGraphics.RectFill(new CGRect(0f, size.Height - 1.0f, size.Width, 1.0f));

            base.Draw(rect);
        }
    }
}