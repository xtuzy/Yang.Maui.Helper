using Maui.BindableProperty.Generator.Core;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics;
using Yang.Maui.Helper.Skia.SKGpuView;
using Style = Topten.RichTextKit.Style;
using TextBlock = Topten.RichTextKit.TextBlock;

namespace Yang.Maui.Helper.Maui.Test.Pages
{
    public partial class SkiaTextDrawDemo : ContentView
    {
        string paragraph1 = "In Cambodia, the choice of a spouse is a complex one for the young male. It may involve not only his parents and his friends, [1] those of the young women, but also a matchmaker. A young man can [2] a likely spouse on his own and them ask his parents to [3] the marriage negotiations, or the young man's parents may make the choice of a spouse, giving the child little to say in the selection. [4] , a girl may veto the spouse her parents have chosen. [5] a spouse has been selected, each family investigates the other to make sure its child is marrying  [6]  a good family.";
        string paragraph2 = "The traditional wedding is a long and colorful affair. Formerly it lasted three days, [7]  by the 1980s it more commonly lasted a day and a half. Buddhist priests offer a short sermon and [8] prayers of blessing. Parts of the ceremony involve ritual hair cutting,  [9]  cotton threads soaked in holy water around the bride's and groom's wrists , and [10] a candle around a circle of happily married and respected couples to bless the [11] . Newlyweds traditionally move in with the wife's parents and may [12] with them up to a year, [13] they can build a flew house nearby.\r\n";
        public SkiaTextDrawDemo()
        {
            List<string> list = new List<string>()
            {
                paragraph1,
                paragraph2,
            };
            var scrollView = new ScrollView() { Orientation = ScrollOrientation.Vertical, VerticalScrollBarVisibility = ScrollBarVisibility.Always };
            var panel = new VerticalStackLayout() { Margin = new Thickness(20, 20, 20, 20) };
            scrollView.Content = panel;
            //scrollView.BackgroundColor = Colors.White;
            this.Content = scrollView;
            foreach (var l in list)
            {
                var view = new JustifyParagraphLabel()
                {
                    //FontSize = 30,
                    Paragraph = l,
                    Margin = new Thickness(0, 20, 0, 0),
                    //WidthRequest = 250,
                    //HeightRequest = 800
                    //BackgroundColor = Colors.Gray
                    EnableTransparent = true
                };
                panel.Children.Add(view);
            }

            /*Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);
                    this.Dispatcher.Dispatch(() =>
                    {
                        //this.InvalidateVisual();
                    });
                }
            });*/
        }

        public override SizeRequest Measure(double widthConstraint, double heightConstraint, MeasureFlags flags = MeasureFlags.None)
        {
            return base.Measure(widthConstraint, heightConstraint, flags);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return base.OnMeasure(widthConstraint, heightConstraint);
        }
    }

    //[INotifyPropertyChanged]
    partial class JustifyParagraphLabel : SKGpuView
    {
        static float density;
        static float Density
        {
            get
            {
                if (density == default)
                    density = (float)DeviceDisplay.Current.MainDisplayInfo.Density;
                return density;
            }
        }

        public JustifyParagraphLabel()
        {
            this.PaintSurface += View_PaintSurface;
            this.SizeChanged += JustifyParagraphLabel_SizeChanged;
        }

        private void JustifyParagraphLabel_SizeChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("SizeCahnged");
        }

        /// <summary>
        /// 段落开始缩进空格数目
        /// </summary>
        [AutoBindable(DefaultValue = "4")]
        public int begainSpaceCount = 4;

        /// <summary>
        /// 字体大小, dp
        /// </summary>
        [AutoBindable(DefaultValue = "14")]
        int fontSize = 14; //Android默认大小

        [AutoBindable]
        string fontFamily;// = "Impact";//"Microsoft YaHei UI"
        /// <summary>
        /// 行间距
        /// </summary>
        [AutoBindable]
        int lineSpace = 0;

        [AutoBindable(DefaultValue = "SkiaSharp.SKColors.Black")]
        SKColor fontColor;

        //[AutoBindable]
        //SKColor backgroundColor;

        /// <summary>
        /// 段落切分后的全部block. 用于存储字符串数据.
        /// </summary>
        List<TextBlock> allBlock;

        /// <summary>
        /// 段落的文本
        /// </summary>
        [AutoBindable(OnChanged = nameof(OnParagraphChanged))]
        string paragraph;

        void OnParagraphChanged(string value)
        {
            allBlock?.Clear();
            allBlock = null;
            lines?.Clear();
            lines = null;
        }

        /// <summary>
        /// 用于排版的数据
        /// </summary>
        List<(float le, List<TextBlock> li)> lines;
        /// <summary>
        /// 缓存计算的单行高度, px
        /// </summary>
        private float lineHeightCache;
        /// <summary>
        /// 空格宽度, px
        /// </summary>
        private float blankW;
        /// <summary>
        /// 空格
        /// </summary>
        private TextBlock skTextOfBlank;

        /// <summary>
        /// 上一次的宽, 如果宽改变, 需要重新布局, dp
        /// </summary>
        double lastWidth;

        void OnWidthChanged()
        {
            lines?.Clear();
            lines = null;
        }

        public override Size CustomMeasuredSize(double widthConstraint, double heightConstraint)
        {
            if (!double.IsInfinity(widthConstraint))
            {
                if (lastWidth != widthConstraint)
                {
                    OnWidthChanged();
                }
                var r = MeasureParagraph((float)widthConstraint * Density);
                lastWidth = widthConstraint;
                return new Size(widthConstraint, r.h / Density);
            }
            else
                return base.CustomMeasuredSize(widthConstraint, heightConstraint);
        }

        public (float w, float h) MeasureParagraph(float w)
        {
            // Create normal style
            var styleNormal = new Style()
            {
                FontSize = FontSize * Density,
                //LineHeight = 40,
                TextColor = FontColor,
            };
            if (FontFamily != default)
                styleNormal.FontFamily = FontFamily;
            //生成全部block
            if (allBlock == null)
            {
                allBlock = new List<TextBlock>();
                var words = Paragraph.Split(' ');
                foreach (var word in words)
                {
                    var skText = new TextBlock() { };
                    skText.AddText(word, styleNormal);
                    allBlock.Add(skText);
                }
            }

            if (skTextOfBlank == null)
            {
                skTextOfBlank = new TextBlock() { };
                skTextOfBlank.AddText(" ", styleNormal);
                blankW = skTextOfBlank.MeasuredWidth;
                lineHeightCache = skTextOfBlank.MeasuredHeight;
            }

            float wLength = blankW * BegainSpaceCount;//起始位置根据开头空格数
            if (lines == null)
            {
                lines = new List<(float, List<TextBlock>)>();
                var line = new List<TextBlock>();
                //编排行
                foreach (var word in allBlock)
                {
                    var skText = word;
                    var tempW = wLength;
                    //如果该行容不下这个单词, 把先前的存储, 进入下一行
                    if (wLength + skText.MeasuredWidth > w)
                    {
                        //换行
                        lines.Add((wLength, line));
                        line = new List<TextBlock>();
                        wLength = 0;
                    }

                    line.Add(skText);
                    wLength = wLength + skText.MeasuredWidth + blankW;
                }
                lines.Add((wLength, line));
            }
            return (w, lineHeightCache * lines.Count + (lines.Count - 1) * LineSpace);
        }


        public void DrawSentence(SKCanvas canvas, int w)
        {
            //绘制
            for (int i = 0; i < lines.Count; i++)
            {
                float lineSpace = LineSpace;
                float x = 0;//单词位置

                if (i == 0) 
                {
                    lineSpace = 0;
                    x = blankW * BegainSpaceCount;//段落起始位置根据开头空格数
                }

                var lin = lines[i];
                var gap = w - lin.le;
                var c = (int)Math.Round(gap / (lin.li.Count - 1) + 0.5);//不舍

                for (var j = 0; j < lin.li.Count; j++)
                {
                    var t = lin.li[j];
                    //最后一行不拉伸
                    if (i == lines.Count - 1)
                    {
                        t.Paint(canvas, new SKPoint(x, 0 + lineHeightCache * i + lineSpace * i));
                        x = x + t.MeasuredWidth + blankW;
                    }
                    else
                    {
                        //拉伸行的最后一词强制对齐
                        if (j == lin.li.Count - 1)
                        {
                            if (x + t.MeasuredWidth != w)
                            {
                                t.Paint(canvas, new SKPoint(w - t.MeasuredWidth, 0 + lineHeightCache * i + lineSpace * i));
                                continue;
                            }
                        }

                        t.Paint(canvas, new SKPoint(x, 0 + lineHeightCache * i + lineSpace * i));
                        if (x < w)
                        {
                            if (gap >= 1)
                            {
                                x = x + t.MeasuredWidth + blankW + c;
                                gap = gap - c;
                            }
                            else
                            {
                                x = x + t.MeasuredWidth + blankW;
                            }
                        }
                    }
                }
            }
        }

        private void View_PaintSurface(object? sender, SKPaintGpuSurfaceEventArgs e)
        {
            var size = this.CanvasSize;
            Console.WriteLine($"{this.GetHashCode()} Paint");
            e.Surface.Canvas.Clear(SKColors.Red.WithAlpha(200));
            DrawSentence(e.Surface.Canvas, (int)size.Width);
        }
    }
}
