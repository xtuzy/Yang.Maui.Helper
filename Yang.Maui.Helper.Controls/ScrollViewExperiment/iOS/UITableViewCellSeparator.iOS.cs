using CoreGraphics;
using UIKit;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS
{
    public class UITableViewCellSeparator : UIView
    {
        UITableViewCellSeparatorStyle _style;
        UIColor _color;

        public UITableViewCellSeparator()
        {
            init();
        }

        public UITableViewCellSeparator(CGRect frame) : base(frame)
        {
            init();
        }

        void init()
        {
            _style = UITableViewCellSeparatorStyle.None;
            this.Hidden = true;
        }

        public void setSeparatorStyle(UITableViewCellSeparatorStyle theStyle, UIColor theColor)
        {
            if (_style != theStyle)
            {
                _style = theStyle;
                this.SetNeedsDisplay();
            }

            if (_color != theColor)
            {
                _color = theColor;
                this.SetNeedsDisplay();
            }

            this.Hidden = (_style == UITableViewCellSeparatorStyle.None);
        }

        public override void Draw(CGRect rect)
        {
            if (_color != null)
            {
                if (_style == UITableViewCellSeparatorStyle.SingleLine)
                {
                    _color.SetFill();
                    UIGraphics.GetCurrentContext().FillRect(new CGRect(0, 0, this.Bounds.Size.Width, 1));
                }
            }
            base.Draw(rect);
        }
    }
}
