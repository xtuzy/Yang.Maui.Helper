using CoreGraphics;
using Foundation;
using Xamarin.Helper.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Xamarin.Helper.Views
{
    /// <summary>
    /// 可以设置最小高度,边框样式,拥有文本改变事件
    /// </summary>
    [Register("BaseUITextView")]
    public class BaseUITextView : BaseUIView, IDisposable
    {
        public BaseUITextView() : base()
        {
            BaseUITextView_Init();
        }


        public BaseUITextView(IntPtr handle) : base(handle)
        {
            BaseUITextView_Init();
        }

        public BaseUITextView(CGRect frame) : base(frame)
        {
            BaseUITextView_Init();
        }

        public UITextView ContentView { get; private set; }

        EventHandler textChanged;
        public event EventHandler TextChanged
        {
            add
            {
                textChanged += value;
                //实现文本改变事件
                if (ContentView.Delegate == null)
                    ContentView.Delegate = new TextViewChangeDelegate(textChanged);
            }
            remove => textChanged -= value;
        }
        public string Text { get => ContentView.Text; set => ContentView.Text = value; }
        public float MinHeight { set => this.HeightAnchor.ConstraintGreaterThanOrEqualTo(value).SetPriority((int)UILayoutPriority.Required).SetActive(); }//高度优先满足最小高度
        private void BaseUITextView_Init()
        {
            //内容设置
            ContentView = new UITextView()
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White,
                Editable = true,
                Selectable = true,
                ScrollEnabled = false,//设置这个属性是因为TextView可以滚动,造成AutoLayout对大小的判断错误
                Font = UIFont.SystemFontOfSize(18)
            };
            //圆角
            ContentView.Layer.CornerRadius = 5.0f;

            this.Add(ContentView);

            //边框设置
            TranslatesAutoresizingMaskIntoConstraints = false;
            Layer.CornerRadius = 5;
            Layer.BorderColor = UIColor.FromRGB(102, 102, 102).CGColor;
            Layer.BorderWidth = 2;
            BackgroundColor = ContentView.BackgroundColor;
            this.IsClickable = true;

            
            //内部布局
            ContentView.LeftToLeft(this, 10).RightToRight(this, -10).TopToTop(this, 10);
            this.BottomAnchor.ConstraintGreaterThanOrEqualTo(ContentView.BottomAnchor).SetActive();
        }

        public UIView SetBackgroundColor(UIColor color)
        {
            BackgroundColor = color;
            ContentView.BackgroundColor = color;
            return this;
        }

        public UIView SetBorderColor(UIColor color)
        {
            Layer.BorderColor = color.CGColor;
            return this;
        }
        public UIView SetBorderWidth(nfloat width)
        {
            Layer.BorderWidth = width;
            return this;
        }

        public override void HandleTapGesture(UITapGestureRecognizer gesture)
        {
            ContentView.BecomeFirstResponder();
        }

        public void Dispose()
        {
            base.Dispose();
            ContentView.Dispose();
            textChanged = null;
            ContentView = null;
        }


        public class TextViewChangeDelegate : UITextViewDelegate, IDisposable
        {
            public TextViewChangeDelegate(EventHandler handler) : base()
            {
                TextChanged = handler;
            }

            private EventHandler TextChanged;
            public override void Changed(UITextView textView)
            {
                //base.Changed(textView);
                if(TextChanged!=null)
                    TextChanged.Invoke(textView, new EventArgs());
            }

            protected void Dispose()
            {
                base.Dispose();
                TextChanged = null;
            }
        }
    }
}