using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Xamarin.Helper.Views
{
    /// <summary>
    /// <see href="https://github.com/lixiang1994/ViewControllerDemo">参考swift将UIView与UIViewController分离</see><br/>
    /// 代表一个页面,对接ViewController的根View,页面中的View统一由该Page管理,需要对接ViewModel数据的View作为公开属性.<br/>
    /// <br/>
    /// 集成功能:<br/>
    /// 1. 点击页面空白回收键盘<br/>
    /// </summary>
    [Register("BasePage")]
    public  class BasePage:BaseUIView
    {
        public BasePage() : base()
        {
        }

        public BasePage(IntPtr handle) : base(handle)
        {
        }

        public BasePage(CGRect frame) : base(frame)
        {
        }


        /// <summary>
        /// 点击页面空白收起键盘
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="evt"></param>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            //用来收回键盘
            this.EndEditing(true);
            //this.ResignFirstResponder();
        }
    }
}