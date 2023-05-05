using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    public class MyScrollView : UIKit.UIScrollView
    {
        public MyScrollView()
        {
            this.Scrolled += MyScrollView_Scrolled;
        }

        

        private void MyScrollView_Scrolled(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ScrollRectToVisible(CGRect rect, bool animated)
        {
            base.ScrollRectToVisible(rect, animated);
        }
    }
}
