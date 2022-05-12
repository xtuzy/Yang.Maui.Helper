//参考:https://github.com/ibireme/YYText/blob/master/Demo/YYTextDemo/YYFPSLabel.m

using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
namespace Yang.Maui.Helper.FPS
{
    public class FPSHelper : UILabel, IDisposable
    {
        CADisplayLink displayLink;
        private double lastTime;
        private int count;

        public FPSHelper(CGRect frame) : base(frame)
        {
            if (frame.Size.Width == 0 && frame.Size.Height == 0)
            {
                this.Frame = new CGRect(frame.X, frame.Y, 20, 50);
            }
            this.BackgroundColor = UIColor.Green;
            this.TextColor = UIColor.Red;
            //displayLink = CADisplayLink.Create(this,new ObjCRuntime.Selector("tick:"));
            displayLink = CADisplayLink.Create(() => tick());
            displayLink.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Common);
        }

        //[Export("tick:")]
        void tick()
        {
            if (lastTime == 0)
            {
                lastTime = displayLink.Timestamp;
                return;
            }

            count++;
            var delta = displayLink.Timestamp - lastTime;
            if (delta < 1) return;
            lastTime = displayLink.Timestamp;
            var fps = count / delta;
            count = 0;

            var prograss = fps / 60.0;
            this.Text = string.Format("{0:0.0}", fps);
        }

        public new void Dispose()
        {
            displayLink?.Invalidate();
            base.Dispose();
        }
    }
}