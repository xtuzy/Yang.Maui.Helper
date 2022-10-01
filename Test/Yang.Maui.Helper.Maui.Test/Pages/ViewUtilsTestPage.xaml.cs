using Yang.Maui.Helper.ViewUtils.PlatformImageSource;

namespace Yang.Maui.Helper.Maui.Test.Pages;

public partial class ViewUtilsTestPage : ContentPage
{
    public ViewUtilsTestPage()
    {
        InitializeComponent();
    }

    private async void GenerateResultImageButton_Clicked(object sender, EventArgs e)
    {
#if WINDOWS
        var image = await ViewUtils.CaptureViewImage.GetImageFormViewAsync(NeedTestLabel.Handler.PlatformView as Microsoft.UI.Xaml.UIElement);

        this.Dispatcher.Dispatch(() =>
        {
            ShowResultImage.Source = new PlatformImageSource() { PlatformImage = image };
        });

#endif
    }
}