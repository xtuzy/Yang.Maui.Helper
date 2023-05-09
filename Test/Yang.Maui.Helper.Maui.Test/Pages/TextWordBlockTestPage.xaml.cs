using Yang.Maui.Helper.File;

namespace Yang.Maui.Helper.Maui.Test.Pages;

public partial class TextWordBlockTestPage : ContentPage
{
	public TextWordBlockTestPage()
	{
		InitializeComponent();
		//this.Content = new SkiaTextDrawDemo();
		this.Content = new EnhanceGraphicsViewDrawTextDemo();
    }
}