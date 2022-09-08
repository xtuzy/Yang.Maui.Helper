using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yang.Maui.Helper.Applications;
using Yang.Maui.Helper.Layouts;
using Yang.Maui.Helper.Test.Tests;

namespace Yang.Maui.Helper.Test
{
    public class MainPage : LinearLayout
    {
        public MainPage(Context? context) : base(context)
        {

            Id = View.GenerateViewId();
            this.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            this.SetBackgroundColor(Color.HotPink);
            this.Orientation = Orientation.Vertical;
            var fileHeperTestButton = new Button(this.Context)
            {
                Text = "FileHelperTest",
            };
            fileHeperTestButton.Click += (sender, e) =>
            {
                new FileHelperTest();
            };

            this.AddView(fileHeperTestButton);
        }
    }
}
