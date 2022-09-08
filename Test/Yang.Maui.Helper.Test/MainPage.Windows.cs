
using Microsoft.Maui.Storage;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Yang.Maui.Helper.Files;
using Yang.Maui.Helper.Test.Tests;

namespace Yang.Maui.Helper.Test
{
    public class MainPage : StackPanel
    {
        public MainPage()
        {
            this.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Colors.HotPink);
            Orientation = Orientation.Vertical;
            HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center;
            VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center;
            
            var fileHeperTestButton = new Button()
            {
                Content = "FileHelperTest",
            };
            fileHeperTestButton.Click += (sender, e) =>
            {
                new FileHelperTest();
            };

            this.Children.Add(fileHeperTestButton);
        }
    }
}
