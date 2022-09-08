using Microsoft.Maui.Storage;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Test.Tests
{
    internal class FileHelperTest
    {
        public FileHelperTest()
        {
            LoadFontFileAsync();
        }

        async Task LoadFontFileAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("OpenSans-Regular.ttf");
            var typeface = SkiaSharp.SKTypeface.FromStream(stream);
        }
    }
}
