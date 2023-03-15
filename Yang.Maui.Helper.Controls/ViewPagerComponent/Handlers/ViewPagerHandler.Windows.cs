using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using ContentPanel = Yang.Maui.Helper.Controls.MauiContent.ContentPanel;

namespace Yang.Maui.Helper.Controls.ViewPagerComponent.Handlers
{
    internal partial class ViewPagerHandler : ViewHandler<IViewPager, Pivot>
    {
        const string ContentPanelTag = "MAUIViewPagerContentPanel";
        int Index = 0;
        protected override Pivot CreatePlatformView()
        {
            Pivot pivot = new Pivot();
    
            return pivot;
        }

        protected override void ConnectHandler(Pivot platformView)
        {
            base.ConnectHandler(platformView);
            platformView.SelectionChanged += Pivot_SelectionChanged;
        }

        protected override void DisconnectHandler(Pivot platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.SelectionChanged -= Pivot_SelectionChanged;
            foreach(var item in platformView.Items)
            {
                var pivotItem = item as PivotItem;
                if(pivotItem != null)
                {
                    var content = pivotItem.Content as ContentPanel;
                    if(content != null)
                    {
                        content.CrossPlatformMeasure = null;
                        content.CrossPlatformArrange = null;
                        content.Children.Clear();
                    }
                }
            }
            platformView.Items.Clear();
        }

        private void Pivot_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            if(Index != PlatformView.SelectedIndex)
            {
                Index = PlatformView.SelectedIndex;
                VirtualView.InvokeViewChanged(Index);
            }
        }

        private static void MapViews(ViewPagerHandler arg1, IViewPager arg2)
        {
            foreach (var view in arg2.Views)
            {
                var platformView = view.ToPlatform(arg1.MauiContext);
                var paddingShim = new ContentPanel()
                {
                    CrossPlatformMeasure = CrossPlatformMeasure(view),
                    CrossPlatformArrange = CrossPlatformArrange(view),
                    Tag = ContentPanelTag
                };
   
                paddingShim.Children.Add(platformView);
                arg1.PlatformView.Items.Add(new PivotItem() { Content = paddingShim, Header = null, Margin= new Microsoft.UI.Xaml.Thickness(0,-50,0,0)});
            }
        }

        private static Func<Rect, Size> CrossPlatformArrange(View view)
        {
            return (rect) =>
            {
                return (view as IView).Arrange(rect);
            };
        }

        private static Func<double, double, Size> CrossPlatformMeasure(View view)
        {
            return (w, h) =>
            {
                (view as IView).Measure(w, h);
                return view.DesiredSize;
            };
        }

        private static void MapGoto(ViewPagerHandler arg1, IViewPager arg2, object arg3)
        {
            if ((int)arg3 == arg1.Index)
                return;
            else
            {
                arg1.Index = (int)arg3;
                arg1.PlatformView.SelectedIndex = arg1.Index;
            }
        }
    }
}
