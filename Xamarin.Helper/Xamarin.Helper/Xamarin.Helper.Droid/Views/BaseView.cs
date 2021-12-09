using Android.Content;
using Android.Graphics;
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
        #region 使用事件
        public readonly WeakEventManager _eventManager;

        public BaseView(Context context) :
            base(context)
        {
            _eventManager = new WeakEventManager();
            BaseView_Initialize();
        }

        public BaseView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            _eventManager = new WeakEventManager();
            BaseView_Initialize();
        }

        public BaseView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            _eventManager = new WeakEventManager();
            BaseView_Initialize();
        }

        #endregion

        #region 使用接口
        WeakReference<IBaseView> viewManager;
        /// <summary>
        /// 用于间接继承,以在不直接继承native对象时使用native对象函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="baseView"></param>
        public BaseView(Context context, IBaseView baseView) :
            base(context)
        {
            BaseView_Initialize();
            viewManager.SetTarget(baseView);
        }

        #endregion

        private void BaseView_Initialize()
        {
            if (Id == -1)
                Id = View.GenerateViewId();
        }


        #region 周期

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
            _eventManager?.RaiseEvent(this, EventArgs.Empty, nameof(OnAttachedToWindowEvent));

            IBaseView view = null;
            viewManager?.TryGetTarget(out view);
            view?.ViewAppearing();

        }

        protected override void OnDraw(Canvas canvas)
        {
            IBaseView view = null;
            viewManager?.TryGetTarget(out view);
            view?.ViewDraw(canvas);
            base.OnDraw(canvas);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            IBaseView view = null;
            viewManager?.TryGetTarget(out view);
            var result = view?.ViewTouchEvent(e);
            if (result.HasValue)
            {
                if (result.Value)//TODO:这里逻辑可能有问题,我如果处理还要不要调用基类的,调用基类的可以判断双击啥的
                    return true;
            }
            return base.OnTouchEvent(e);
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
            _eventManager?.RaiseEvent(this, EventArgs.Empty, nameof(OnDetachedFromWindowEvent));
            IBaseView view = null;
            viewManager?.TryGetTarget(out view);
            view?.ViewDisAppeared();
        }

        #endregion
    }
}