using Android.Views;
using System;

namespace Yang.Maui.Helper.Platforms.Android.Actions
{
    public class ClickAction : Java.Lang.Object, View.IOnClickListener
    {
        Action Action { get; set; }
        public ClickAction(Action action)
        {
            Action = action;
        }
        public virtual void OnClick(View v)
        {
            // 默认不实现，让子类实现
            //throw new NotImplementedException();
            Action.Invoke();
        }

        protected override void Dispose(bool disposing)
        {
            Action = null;
            base.Dispose(disposing);
        }
    }
}