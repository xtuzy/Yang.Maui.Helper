using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Yang.Maui.Helper.Tools;
using WeakEventManager = Yang.Maui.Helper.Tools.WeakEventManager;

namespace Yang.Maui.Helper.Platforms.iOS.Views
{
    /// <summary>
    /// 自定义View的基类,提供一致体验和简化代码<br/>
    /// <br/>
    /// 集成功能:<br/>
    /// 1. 提供观察View是否显示到屏幕的方法<br/>
    /// 2. 提供点击事件开关和处理点击事件的方法
    /// </summary>
    [Register("BaseUIView")]
    public class BaseUIView : UIView
    {
        public readonly WeakEventManager _eventManager = new WeakEventManager();

        bool isAttachedToWindow = false;

        UITapGestureRecognizer gesture = new UITapGestureRecognizer();
        bool isClickable = false;
        /// <summary>
        /// 开启和关闭可点击
        /// </summary>
        public bool IsClickable
        {
            set
            {
                if (value != isClickable && value == false)
                {
                    //TODO:gesture.RemoveTarget(gesture.Token);
                    this.RemoveGestureRecognizer(gesture);
                }
                else if (value != isClickable && value == true)
                {
                    //为文本框边框添加Tap手势,用来在点击边框时唤起键盘输入
                    gesture.AddTarget(() => HandleTapGesture(gesture));
                    this.AddGestureRecognizer(gesture);
                }
            }
        }

        public virtual void HandleTapGesture(UITapGestureRecognizer gesture) { }

        public BaseUIView() : base()
        {
            //这句不能用在非RootController的根View上,不然黑屏
            //this.TranslatesAutoresizingMaskIntoConstraints = false;
        }

        /// <summary>
        /// 在从xib创建时需要
        /// </summary>
        /// <param name="handle"></param>
        public BaseUIView(IntPtr handle) : base(handle){}

        public BaseUIView(CGRect frame) : base(frame){}

        /// <summary>
        /// Event for <see cref="AwakeFromNib"/>
        /// </summary>
        public event EventHandler AwakeFromNibEvent
        {
            add => _eventManager.AddEventHandler(value,nameof(AwakeFromNibEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(AwakeFromNibEvent));
        }
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(AwakeFromNibEvent));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void MovedToWindow()
        {
            base.MovedToWindow();
            if (isAttachedToWindow is false)//新添加
            {
                OnAttachedToWindow();
                isAttachedToWindow = true;
            }
            else//移除
            {
                OnDetachedFromWindow();
                isAttachedToWindow = false;
            }
        }

        /// <summary>
        /// Layout in this event, such as set Frame,Constraint<br/>
        /// <see href="https://github.com/lixiang1994/ViewControllerDemo#%E4%BD%BF%E7%94%A8%E6%BC%94%E7%A4%BA"/>
        /// </summary>
        public event EventHandler LayoutSubviewsEvent
        {
            add => _eventManager.AddEventHandler(value, nameof(LayoutSubviewsEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(LayoutSubviewsEvent));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(LayoutSubviewsEvent));
        }

        /// <summary>
        /// Event for <see cref="OnAttachedToWindow"/>
        /// </summary>
        public event EventHandler OnAttachedToWindowEvent
        {
            add => _eventManager.AddEventHandler(value, nameof(OnAttachedToWindowEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(OnAttachedToWindowEvent));
        }
        /// <summary>
        /// 模仿Android的View,添加到Window时调用
        /// </summary>
        public virtual void OnAttachedToWindow()
        {
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(OnAttachedToWindowEvent));
        }

        /// <summary>
        /// Event for <see cref="OnDetachedFromWindow"/>
        /// </summary>
        public event EventHandler OnDetachedFromWindowEvent
        {
            add => _eventManager.AddEventHandler(value, nameof(OnDetachedFromWindowEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(OnDetachedFromWindowEvent));
        }
        /// <summary>
        /// 模仿Android的View,从Window移除时调用
        /// </summary>
        public virtual void OnDetachedFromWindow()
        {
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(OnDetachedFromWindowEvent));
        }
    }
}