
using CoreFoundation;
using Foundation;
using System;
using UIKit;
using Yang.Maui.Helper.Tools;
using WeakEventManager = Yang.Maui.Helper.Tools.WeakEventManager;

namespace Yang.Maui.Helper.Platforms.iOS.Controllers
{
    /// <summary>
    /// <see href="https://github.com/lixiang1994/ViewControllerDemo">参考swift将UIView与UIViewController分离</see><br/>
    /// 所有子View组合成单独的Page,该类负责绑定Page和ViewModel,以及生命周期管理.<br/>
    /// <br/>
    /// 集成功能:<br/>
    /// 1. 提供DayNight主题切换监听<br/>
    /// </summary>
    [Register("BaseUIViewController")]
    public class BaseUIViewController : UIViewController
    {
        public readonly WeakEventManager _eventManager = new WeakEventManager();

        public BaseUIViewController()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        /// <summary>
        /// Event for <see cref="ViewDidLoad"/><br/>
        /// Event Arg is <see cref="EventArgs.Empty"/>
        /// </summary>
        public event EventHandler ViewDidLoadEvent
        {
            add => _eventManager.AddEventHandler(value,nameof(ViewDidLoadEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(ViewDidLoadEvent));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(ViewDidLoadEvent));
        }

        /// <summary>
        /// Event for <see cref="ViewWillAppear(bool)"/><br/>
        /// Event Arg is <see cref="EventArgs.Empty"/>
        /// </summary>
        public event EventHandler ViewWillAppearEvent
        {
            add => _eventManager.AddEventHandler(value,nameof(ViewWillAppearEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(ViewWillAppearEvent));
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(ViewWillAppearEvent));
        }

        /// <summary>
        /// Event for <see cref="ViewWillDisappear(bool)"/><br/>
        /// Event Arg is <see cref="EventArgs.Empty"/>
        /// </summary>
        public event EventHandler ViewWillDisappearEvent
        {
            add => _eventManager.AddEventHandler(value,nameof(ViewWillDisappearEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(ViewWillDisappearEvent));
        }
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(ViewWillDisappearEvent));
        }

        /// <summary>
        /// Event for theme change between Dark and Light.<br/>
        /// Event Arg is <see cref="Theme"/>.
        /// </summary>
        public event EventHandler ThemeChangedEvent
        {
            add => _eventManager.AddEventHandler(value,nameof(ThemeChangedEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(ThemeChangedEvent));
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            //颜色发生了变化,说明切换了DayNight主题
            if (TraitCollection.HasDifferentColorAppearanceComparedTo(previousTraitCollection))
            {
                //模式发生变化会回调这里
                //检测当前主题
                if (this.TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
                    _eventManager.RaiseEvent(this, Theme.Dark, nameof(ThemeChangedEvent));
                else
                    _eventManager.RaiseEvent(this, Theme.Light, nameof(ThemeChangedEvent));
            }
        }
    }
}