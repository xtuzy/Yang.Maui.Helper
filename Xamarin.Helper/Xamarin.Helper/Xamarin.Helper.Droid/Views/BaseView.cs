using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.ConstraintLayout.Widget;
using System;
using Xamarin.Helper.Tools;

namespace Xamarin.Helper.Views
{
    /// <summary>
    /// 继承自ConstraintLayout,可以使用约束
    /// </summary>
    public class BaseView : ConstraintLayout
    {
        public readonly WeakEventManager _eventManager = new WeakEventManager();

        public BaseView(Context context) :
            base(context)
        {
            BaseView_Initialize();
        }

        public BaseView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            BaseView_Initialize();
        }

        public BaseView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            BaseView_Initialize();
        }

        private void BaseView_Initialize()
        {
            if (Id == -1)
                Id = View.GenerateViewId();
        }

        /// <summary>
        /// Event for <see cref="OnAttachedToWindow"/>
        /// </summary>
        public event EventHandler OnAttachedToWindowEvent
        {
            add => _eventManager.AddEventHandler(value, nameof(OnAttachedToWindowEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(OnAttachedToWindowEvent));
        }
        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
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

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(OnDetachedFromWindowEvent));
        }
    }
}