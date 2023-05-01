namespace Yang.Maui.Helper.Maui.Test.Pages;

public partial class ViewPagerTestPage : ContentPage
{
	public ViewPagerTestPage()
	{
		InitializeComponent();
        var firstViewPage = new Grid() { BackgroundColor = Colors.Red, Children = { new EnhanceGraphicsViewDrawTextDemo() } };
        var secondViewPage = new Grid() { BackgroundColor = Colors.Green };
        var thirdViewPage = new Grid() { BackgroundColor = Colors.Blue, Children = { new SkiaTextDrawDemo() } };

        List<View> viewPages = new List<View>();
        viewPages.Add(firstViewPage);
        viewPages.Add(secondViewPage);
        viewPages.Add(thirdViewPage);

        viewPager.Views = viewPages.ToArray();

        topTabBar.Tabs.Add(new KeyValuePair<string, View>(nameof(firstViewPage), viewPages[0]));
        topTabBar.Tabs.Add(new KeyValuePair<string, View>(nameof(secondViewPage), viewPages[1]));
        topTabBar.Tabs.Add(new KeyValuePair<string, View>(nameof(thirdViewPage), viewPages[2]));

        topTabBar.TabChanged += (sender) =>
        {
            viewPager.GotoView(sender);
        };
        viewPager.ViewChanged += (sender) =>
        {
            topTabBar.Goto(sender);
        };
    }
}