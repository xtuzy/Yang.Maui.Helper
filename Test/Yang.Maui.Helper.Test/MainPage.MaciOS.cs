using CoreGraphics;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Yang.Maui.Helper.Test.Tests;

namespace Yang.Maui.Helper.Test
{
    public class MainPage
    {
        public UIView Page;
        public MainPage(CGRect frame)
        {
            Page = new UIStackView(frame)
            {
                Axis = UILayoutConstraintAxis.Vertical,
            };

            var fileHeperTestButton = new UIButton(new CGRect(100, 200, 100, 50));
            fileHeperTestButton.SetTitle("FileHeperTest", UIControlState.Normal);
            fileHeperTestButton.TouchUpInside += (sender, e) =>
            {
                new FileHelperTest();
            };

            Page.AddSubview(fileHeperTestButton);
        }

        public UIView GetPage()
        {
            return Page;
        }
    }
}
