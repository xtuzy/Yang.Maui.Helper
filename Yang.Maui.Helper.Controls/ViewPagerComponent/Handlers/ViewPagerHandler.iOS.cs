using CoreAudioKit;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using ContentView = Yang.Maui.Helper.Controls.MauiContent.ContentView;

namespace Yang.Maui.Helper.Controls.ViewPagerComponent.Handlers
{
    internal partial class ViewPagerHandler : ViewHandler<IViewPager, UIView>
    {
        private MyPageViewController pageController;
        List<UIViewController> childControllers = new List<UIViewController>();
        protected override UIView CreatePlatformView()
        {
            var view = new UIView();
            return view;
        }

        private static void MapViews(ViewPagerHandler arg1, IViewPager arg2)
        {
            arg1.pageController = new MyPageViewController(UIPageViewControllerTransitionStyle.Scroll,
                                                          UIPageViewControllerNavigationOrientation.Horizontal,
                                                          UIPageViewControllerSpineLocation.None);
            //arg1.pageController.View.ClipsToBounds = true;
            foreach (var view in arg2.Views)
            {
                var contentView = new ContentView()
                {
                    View = view,
                    CrossPlatformMeasure = (w, h) =>
                    {
                        (view as IView).Measure(w, h);
                        return view.DesiredSize;
                    },
                    CrossPlatformArrange = (rect) =>
                    {
                        return (view as IView).Arrange(rect);
                    },
                };
                contentView.Add(view.ToPlatform(arg1.MauiContext));
                var controller =  new UIViewController { View = contentView };
                arg1.childControllers.Add(controller);
            }
            //初始页面
            arg1.pageController.SetViewControllers(new[] { arg1.childControllers[0] },
                UIPageViewControllerNavigationDirection.Forward,
                true,
                (e) =>
                {

                });
            arg1.pageController.DataSource = new MyDataSource(arg1.childControllers);
            arg1.pageController.Delegate = new MyDelegate(arg1.PageChanged);
            arg1.PlatformView.ClearSubviews();
            arg1.PlatformView.Add(arg1.pageController.View);
            arg1.pageController.View.TranslatesAutoresizingMaskIntoConstraints = false;
            arg1.pageController.View.WidthAnchor.ConstraintEqualTo(arg1.PlatformView.WidthAnchor).Active = true;
            arg1.pageController.View.HeightAnchor.ConstraintEqualTo(arg1.PlatformView.HeightAnchor).Active = true;
            arg1.PageChanged(arg1.childControllers[0]);
            //arg1.pageController.DidMoveToParentViewController(arg1.ViewController);
        }
        
        private static void MapGoto(ViewPagerHandler arg1, IViewPager arg2, object arg3)
        {
            UIPageViewControllerNavigationDirection direction = UIPageViewControllerNavigationDirection.Forward;
            if((int)arg3 < arg1.pageController.Index)//需要向左滑动
                direction = UIPageViewControllerNavigationDirection.Reverse;
            arg1.pageController.SetViewControllers( new[] { arg1.childControllers[(int)arg3] },
                direction,
                true,
                (e) =>
                {

                });
            arg1.pageController.Index = (int)arg3;
        }

        void PageChanged(UIViewController controller)
        {
            var index = childControllers.IndexOf(controller);
            pageController.Index = index;
            if (index < 0 || index > childControllers.Count - 1)
                return;
            VirtualView.InvokeViewChanged(index);
        }

        protected override void ConnectHandler(UIView platformView)
        {
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(UIView platformView)
        {
            base.DisconnectHandler(platformView);
            //UIPageViewController的View添加到了PageViewer, 这里切断PageViewer与UIPageViewController联系
            PlatformView.ClearSubviews();
            pageController.View?.RemoveFromSuperview();
            foreach (var controller in childControllers)
            {
                //切断ContentView与UIPageViewController的联系
                controller.View.RemoveFromSuperview();//我UIPageViewController引用了它
                //切断ContentView 与 MauiView联系
                controller.View.ClearSubviews();//MauiView作为子View
                var contentView = controller.View as ContentView;
                if (contentView != null)
                {
                    contentView.CrossPlatformMeasure = null;//移除这个是因为Measure的委托引用了View
                    contentView.CrossPlatformArrange = null;
                }
                //切断ContentView与UIController联系
                controller.View = null;
                //切断UIViewController与UIPageViewController联系
                controller.RemoveFromParentViewController();
            }
            childControllers.Clear();
            pageController.RemoveFromParentViewController();
            pageController.Dispose();
            pageController = null;
        }

        class MyDataSource : UIPageViewControllerDataSource
        {
            public List<UIViewController> Pages { get; private set; }

            public MyDataSource(List<UIViewController> pages)
            {
                this.Pages = pages;
            }

            public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
            {
                var Index = (pageViewController as MyPageViewController).Index;
                if (Index <= 0)//没有前一个了
                {
                    return null;//返回nil代表没有
                }
                else
                {
                    return Pages[Index - 1];
                }
            }

            public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
            {
                var Index = (pageViewController as MyPageViewController).Index;
                if (Index >= Pages.Count - 1)//没有下一个了
                {
                    return null;
                }
                else
                {
                    return Pages[Index + 1];
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                Pages = null;
            }
        }

        class MyDelegate : UIPageViewControllerDelegate
        {
            Action<UIViewController> PageChanged;

            public MyDelegate(Action<UIViewController> action)
            {
                PageChanged = action;
            }

            public override void DidFinishAnimating(UIPageViewController pageViewController, bool finished, UIViewController[] previousViewControllers, bool completed)
            {
                //base.DidFinishAnimating(pageViewController, finished, previousViewControllers, completed);
                if(finished && completed)
                    PageChanged?.Invoke(pageViewController.ViewControllers[0]);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                PageChanged = null;
            }
        }

        class MyPageViewController : UIPageViewController
        {
            public MyPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation): base(style, navigationOrientation, spineLocation)
            {

            }
            public int Index = 0;
        }
    }
}
