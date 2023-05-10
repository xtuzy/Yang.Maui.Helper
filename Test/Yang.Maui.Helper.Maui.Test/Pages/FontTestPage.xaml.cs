using Topten.RichTextKit.Editor;
using Yang.Maui.Helper.Graphics;
using Yang.Maui.Helper.Image;
using Font = Microsoft.Maui.Font;

namespace Yang.Maui.Helper.Maui.Test.Pages;

public partial class FontTestPage : ContentPage
{
	public FontTestPage()
	{
		InitializeComponent();
        LoadFontFileAsync();
        var showYouYuan = new JustifyParagraphLabel1()
        {
            
            Paragraph = "落霞与孤鹜齐飞，秋水共长天一色"
        };
        showYouYuan.Font = new MauiFont(showYouYuan, Font.OfSize("YouYuan", 0));
        var showIconFont = new JustifyParagraphLabel1()
        {
            Paragraph = $"{FontAwesomeIcons.ArrowLeft} {FontAwesomeIcons.ArrowRight}"
        };
        showIconFont.Font = new MauiFont(showIconFont, Font.OfSize("Font Awesome 6 Free-Solid-900", 0));
        layout.Add(showYouYuan);
        layout.Add(showIconFont);
    }

    async Task LoadFontFileAsync()
    {
        Stream stream = null;
        try
        {
            stream = await FileSystem.OpenAppPackageFileAsync("YouYuan.ttf");
        }
        catch (FileNotFoundException e)
        {
            
        }
        finally
        {
            if (stream != null)
            {
                var typeface = SkiaSharp.SKTypeface.FromStream(stream);
            }
            Console.WriteLine($"Find YouYuan.ttf:{stream != null}");
        }
    }
}