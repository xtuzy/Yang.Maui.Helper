using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager2.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using static AndroidX.ViewPager2.Widget.ViewPager2;
using ContentViewGroup = Yang.Maui.Helper.Controls.MauiContent.ContentViewGroup;

namespace Yang.Maui.Helper.Controls.ViewPagerComponent.Handlers
{
    internal partial class ViewPagerHandler : ViewHandler<IViewPager, ViewPager2>
    {
        protected override ViewPager2 CreatePlatformView()
        {
            var view = new ViewPager2(Context);
            return view;
        }

        private static void MapViews(ViewPagerHandler handler, IViewPager arg2)
        {
            var viewPager = handler.PlatformView;
            var adapter = new MyAdapter();
            foreach (var view in arg2.Views)
            {
                var paddingShim = new ContentViewGroup(handler.MauiContext.Context)
                {
                    CrossPlatformMeasure = (w, h) =>
                    {
                        (view as IView).Measure(w, h);
                        return view.DesiredSize;
                    },
                    CrossPlatformArrange = (rect) =>
                    {
                        return (view as IView).Arrange(rect);
                    }
                };
                paddingShim.AddView(view.ToPlatform(handler.MauiContext));
                adapter.Views.Add(paddingShim);
            }
            viewPager.Adapter = adapter;
        }

        private static void MapGoto(ViewPagerHandler handler, IViewPager arg2, object arg3)
        {
            handler.PlatformView.SetCurrentItem((int)arg3, true);
        }

        MyOnPageChangeCallback PageChangeCallback;
        protected override void ConnectHandler(ViewPager2 platformView)
        {
            base.ConnectHandler(platformView);
            if (PageChangeCallback == null)
                PageChangeCallback = new MyOnPageChangeCallback(VirtualView.InvokeViewChanged);
            platformView.RegisterOnPageChangeCallback(PageChangeCallback);
        }

        protected override void DisconnectHandler(ViewPager2 platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.UnregisterOnPageChangeCallback(PageChangeCallback);
            var myAdapter = (PlatformView.Adapter as MyAdapter);
            if (myAdapter != null)
            {
                foreach(var view in myAdapter.Views)
                {
                    var contentView = view as ContentViewGroup;
                    contentView.CrossPlatformMeasure = null;
                    contentView.CrossPlatformArrange = null;
                    contentView.RemoveAllViews();
                }
            }
            myAdapter.Dispose();
            PlatformView.Adapter = null;
        }

        class MyAdapter : RecyclerView.Adapter, IDisposable
        {
            internal List<Android.Views.View> Views = new List<Android.Views.View>();

            public override int ItemCount => Views.Count;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MyHolder vh = holder as MyHolder;
                vh.Layout.RemoveAllViews();
                (Views[position].Parent as ViewGroup)?.RemoveView(Views[position]);//**Java.Lang.IllegalStateException:** 'The specified child already has a parent. You must call removeView() on the child's parent first.'
                vh.Layout.AddView(Views[position], new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent));
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                return new MyHolder(new FrameLayout(parent.Context) { LayoutParameters = new RecyclerView.LayoutParams(RecyclerView.LayoutParams.MatchParent, RecyclerView.LayoutParams.MatchParent) });
            }

            class MyHolder : RecyclerView.ViewHolder, IDisposable
            {
                public FrameLayout Layout;
                public MyHolder(Android.Views.View itemView) : base(itemView)
                {
                    Layout = itemView as FrameLayout;
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                Views = null;
            }
        }

        class MyOnPageChangeCallback: OnPageChangeCallback, IDisposable
        {
            Action<int> PageSelected;
            public MyOnPageChangeCallback(Action<int> pageSelected)
            {
                PageSelected = pageSelected;
            }

            public override void OnPageSelected(int position)
            {
                base.OnPageSelected(position);
                PageSelected?.Invoke(position);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                PageSelected = null;
            }
        }
    }
}
