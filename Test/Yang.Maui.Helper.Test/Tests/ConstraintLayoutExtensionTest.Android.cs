using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yang.Maui.Helper.Layouts;

namespace Yang.Maui.Helper.Test.Tests
{
    internal class ConstraintLayoutExtensionTest
    {
        private Button MainButton;
        private Button FirstButton;
        public Button SecondButton;
        private View ThirdCanvas;
        private TextView FouthTextBlock;
        private EditText FifthTextBox;
        private TextView SixthRichTextBlock;
        public ConstraintLayout layout;

        public ConstraintLayoutExtensionTest(ConstraintLayout page)
        {
            CreateControls(page.Context);
            ConstraintLayoutInPlatformLayoutTest(page);
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
        }

        void CreateControls(Context context)
        {
            FirstButton = new Button(context)
            {
                Id = View.GenerateViewId(),
                Text = "FirstButton At Center",
            };
            FirstButton.SetBackgroundColor(Color.Red);
            FirstButton.SetTextColor(Color.White);

            SecondButton = new Button(context)
            {
                Id = View.GenerateViewId(),
                Text = "Second Button",
            };
            SecondButton.SetBackgroundColor(Color.Black);
            SecondButton.SetTextColor(Color.White);

            ThirdCanvas = new View(context)
            {
                Id = View.GenerateViewId(),
            };
            ThirdCanvas.SetBackgroundColor(Color.LightGreen);

            FouthTextBlock = new TextView(context)
            {
                Id = View.GenerateViewId(),
                Tag = nameof(FouthTextBlock),
                Text = "TextBlock"
            };

            FifthTextBox = new EditText(context)
            {
                Id = View.GenerateViewId(),
                Tag = nameof(FifthTextBox),
                Text = "TextBox",
            };

            SixthRichTextBlock = new TextView(context)
            {
                Id = View.GenerateViewId(),
                Text = "RichTextBlock",
                TextSize = 18,
            };
        }
    }
}
