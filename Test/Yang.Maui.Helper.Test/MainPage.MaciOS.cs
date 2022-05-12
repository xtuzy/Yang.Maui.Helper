using CoreGraphics;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Yang.Maui.Helper.Test
{
    public class MainPage
    {
        public UIView Page;
        public MainPage(CGRect frame)
        {
            var label = new FPS.FPSHelper(new CGRect(frame.Right - 100, 100, 100, 200))
            {
                TextAlignment = UITextAlignment.Center,
            };

            var button = new UIButton(new CGRect(100, 200, 100, 50))
            {
            };
            button.SetTitle("Click", UIControlState.Normal);
            button.TouchUpInside += (sender, e) =>
            {
                button.BackgroundColor = UIColor.LightGray;
            };

            Page = new UIView(frame);
            var scrollView =new UIScrollView(frame) { ContentSize = new CGSize(frame.Width, frame.Height * 2) };
            Page.AddSubview(scrollView);
            Page.AddSubview(label);
            
            var layout = new UIStackView(frame)
            {
                BackgroundColor = UIColor.SystemYellow,
                Axis = UILayoutConstraintAxis.Vertical,
            };
            scrollView.Add(layout);
            layout.Add(button);
            layout.Add(new UIView() { Frame = new CGRect(0, 0, 100, 1000), BackgroundColor = UIColor.Blue });
            
        }

        async Task LoadMauiAsset()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("favorite_black_24dp.svg");
            Debug.WriteLine($"svg size:{stream.Length}");
        }

        public UIView GetPage()
        {
            return Page;
        }
    }
}
