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

namespace Yang.Maui.Helper.Test
{
    public class MainPage : ConstraintLayout
    {
        private Button MainButton;
        private Button FirstButton;
        public Button SecondButton;
        private View ThirdCanvas;
        private TextView FouthTextBlock;
        private EditText FifthTextBox;
        private TextView SixthRichTextBlock;
        public ConstraintLayout layout;
        public MainPage(Context? context) : base(context)
        {
            /*this.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            MainButton = new Button(context)
            {
                Text = "MainButton321",
                LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            };
            ((RelativeLayout.LayoutParams)(MainButton.LayoutParameters)).SetMargins(50, 50, 0, 0);
            this.AddView(MainButton);
            //this.AddView(new TextView(context) { Text = "Text" });
            SetBackgroundColor(Android.Graphics.Color.Pink);
            MainButton.Click += MainButton_Click;*/

            Id = View.GenerateViewId();
            this.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            createControls();
            this.SetBackgroundColor(Color.HotPink);
            ConstraintLayoutInPlatformLayoutTest(this);
        }

        void ConstraintLayoutInPlatformLayoutTest(ConstraintLayout page)
        {

            var scrollView = new ScrollView(page.Context)
            {
                //ContentSize = new CGSize(300, 600),
                Id = View.GenerateViewId(),
            };
            scrollView.SetBackgroundColor(Color.White);
            page.AddElement(scrollView);

            using (var set = new FluentConstraintSet())
            {
                set.Clone(page);
                set.Select(scrollView)
                    .LeftToLeft().TopToTop()
                    .Width(FluentConstraintSet.SizeBehavier.MatchParent)
                    .Height(FluentConstraintSet.SizeBehavier.MatchParent);
                set.ApplyTo(page);
            }

            var firstConstraintLayoutPage = new ConstraintLayout(page.Context)
            {
                //Frame = new CGRect(0, 0, 200, 100),
                Id = View.GenerateViewId(),
            };
            firstConstraintLayoutPage.SetBackgroundColor(Color.Pink);

            scrollView.AddView(firstConstraintLayoutPage);

            firstConstraintLayoutPage.AddElement(FirstButton);
            firstConstraintLayoutPage.AddElement(SecondButton);
            firstConstraintLayoutPage.AddElement(ThirdCanvas);
            firstConstraintLayoutPage.AddElement(FouthTextBlock);
            firstConstraintLayoutPage.AddElement(FifthTextBox);
            firstConstraintLayoutPage.AddElement(SixthRichTextBlock);

            firstConstraintLayoutPage.LayoutParameters = new ScrollView.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            using (var set = new FluentConstraintSet())
            {
                set.Clone(firstConstraintLayoutPage);
                set.Select(FirstButton)
                    .TopToTop().CenterXTo()
                    .Select(SecondButton).TopToBottom(FirstButton).CenterXTo()
                    .Select(ThirdCanvas).TopToBottom(SecondButton).CenterXTo()
                    .Select(FouthTextBlock).TopToBottom(ThirdCanvas).CenterXTo()
                    .Select(FifthTextBox).TopToBottom(FouthTextBlock).CenterXTo()
                    .Select(SixthRichTextBlock).TopToBottom(FifthTextBox).CenterXTo()
                    .Select(FirstButton, SecondButton, ThirdCanvas, FouthTextBlock, FifthTextBox, SixthRichTextBlock)
                    .Width(FluentConstraintSet.SizeBehavier.WrapContent)
                    .Height(FluentConstraintSet.SizeBehavier.WrapContent);
                set.ApplyTo(firstConstraintLayoutPage);
            }

            Task.Run(async () =>
                  {
                      await Task.Delay(3000);//wait ui show
                      UIThread.Invoke(() =>
                         {
                             //System.Diagnostics.Debug.WriteLine("firstConstraintLayoutPage:" + firstConstraintLayoutPage.GetViewLayoutInfo());
                             //SimpleDebug.WriteLine("FirstButton:" + FirstButton.GetViewLayoutInfo());
                             //SimpleDebug.WriteLine("scrollView:" + scrollView.GetViewLayoutInfo());
                             //SimpleDebug.WriteLine("page:" + page.GetViewLayoutInfo());
                         }, page);
                  });
        }

        private void createControls()
        {
            FirstButton = new Button(this.Context)
            {
                Id = View.GenerateViewId(),
                Text = "FirstButton At Center",
            };
            FirstButton.SetBackgroundColor(Color.Red);
            FirstButton.SetTextColor(Color.White);

            SecondButton = new Button(this.Context)
            {
                Id = View.GenerateViewId(),
                Text = "Second Button",
            };
            SecondButton.SetBackgroundColor(Color.Black);
            SecondButton.SetTextColor(Color.White);

            ThirdCanvas = new View(this.Context)
            {
                Id = View.GenerateViewId(),
            };
            ThirdCanvas.SetBackgroundColor(Color.LightGreen);

            FouthTextBlock = new TextView(this.Context)
            {
                Id = View.GenerateViewId(),
                Tag = nameof(FouthTextBlock),
                Text = "TextBlock"
            };

            FifthTextBox = new EditText(this.Context)
            {
                Id = View.GenerateViewId(),
                Tag = nameof(FifthTextBox),
                Text = "TextBox",
            };

            SixthRichTextBlock = new TextView(this.Context)
            {
                Id = View.GenerateViewId(),
                Text = "RichTextBlock",
                TextSize = 18,
            };
        }

        private void MainButton_Click(object sender, EventArgs e)
        {
            Toast.MakeText((sender as View).Context, "Clicked", ToastLength.Short).Show();
        }

        async Task LoadMauiAsset()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("favorite_black_24dp.svg");
            System.Diagnostics.Debug.WriteLine($"svg size:{stream.Length}");
        }

    }
}
