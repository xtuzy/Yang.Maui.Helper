using Maui.BindableProperty.Generator.Core;
using SkiaSharp;
using System.Diagnostics;
using Yang.Maui.Helper.Controls.EnhanceGraphicsViewComponent;
using Yang.Maui.Helper.Graphics;
using Yang.Maui.Helper.Graphics.Hyphen;
using Style = Topten.RichTextKit.Style;

namespace Yang.Maui.Helper.Maui.Test.Pages
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DatabaseViewer"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DatabaseViewer;assembly=DatabaseViewer"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:PaperViewDrawDemo/>
    ///
    /// </summary>
    public partial class EnhanceGraphicsViewDrawTextDemo : ContentView
    {
        public static string paragraph1 = "In Cambodia, the choice of a spouse is a complex one for the young male. It may involve not only his parents and his friends, [1] those of the young women, but also a matchmaker. A young man can [2] a likely spouse on his own and them ask his parents to [3] the marriage negotiations, or the young man's parents may make the choice of a spouse, giving the child little to say in the selection. [4] , a girl may veto the spouse her parents have chosen. [5] a spouse has been selected, each family investigates the other to make sure its child is marrying  [6]  a good family.";
        public static string paragraph2 = "The traditional中文 wedding is a long and colorful affair. Formerly it lasted three days, [7]  by the 1980s it more commonly lasted a day and a half. Buddhist priests offer a short sermon and [8] prayers of blessing. Parts of the ceremony involve ritual hair cutting,  [9]  cotton threads soaked in holy water around the bride's and groom's wrists , and [10] a candle around a circle of happily married and respected couples to bless the [11] . Newlyweds traditionally move in with the wife's parents and may [12] with them up to a year, [13] they can build a flew house nearby.";
        public EnhanceGraphicsViewDrawTextDemo()
        {
            List<string> list = new List<string>()
            {
                "这是一行中文文本, 包含表情符号🤣",
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,

                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,

                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,

                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,

                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
                paragraph1,
                paragraph2,
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
                var view = new JustifyParagraphLabelNormal()
                {
                    //FontSize = 30,
                    Paragraph = l,
                    Margin = new Thickness(0, 20, 0, 0),
                    //WidthRequest = 250,
                    //HeightRequest = 300
                    //BackgroundColor = Colors.Gray
                    LineSpace = 10
                };
                //panel.Children.Add(view);
            }
            panel.Children.Add(new JustifySingleParagraphLabelByHyphen()
            {
                BegainSpaceCount = 0,
                FontSize = 20,
                FontColor = Colors.Green,
                Paragraph = paragraph2 + " " + paragraph1,
                LineSpace = 10
            });
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

        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }
    }

    /// <summary>
    /// 其只创建一个TextWordBlock
    /// </summary>
    public partial class JustifySingleParagraphLabelByHyphen : JustifyParagraphLabelNormal
    {
        private DefaultHyphenator Hyphenator;

        public JustifySingleParagraphLabelByHyphen() : base()
        {
            Hyphenator = Graphics.Hyphen.DefaultHyphenator.getInstance(Graphics.Hyphen.DefaultHyphenator.HyphenPattern.EN_US);
        }

        public override (float w, float h) MeasureParagraph(float w)
        {
            // Create normal style
            styleNormal = new JustifyParagraphLabelNormal.PlatformStyle()
            {
                FontSize = FontSize,
                Font = Font,
                FontColor = FontColor,
            };

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
                //blankW = skTextOfBlank.MeasuredWidth;
                //lineHeightCache = skTextOfBlank.MeasuredHeight;

                var m = new TextBlock() { };
                m.AddText("m", styleNormal);
                var n = new TextBlock() { };
                n.AddText("n", styleNormal);
                var nm = new TextBlock() { };
                nm.AddText("n m", styleNormal);

                blankWCache = nm.MeasuredWidth - n.MeasuredWidth - n.MeasuredWidth;
                lineHeightCache = nm.MeasuredHeight;
#if DEBUG
                Console.WriteLine($"m w={m.MeasuredWidth} h={m.MeasuredHeight}");
                Console.WriteLine($"n w={n.MeasuredWidth} h={n.MeasuredHeight}");
                Console.WriteLine($"space w={skTextOfBlank.MeasuredWidth} h={skTextOfBlank.MeasuredHeight}");
                Console.WriteLine($"n m w={nm.MeasuredWidth} h={nm.MeasuredHeight}");
#endif
            }

            float wLength = blankWCache * BegainSpaceCount;//起始位置根据开头空格数
            if (lines == null)
            {
                lines = new List<(float, List<TextBlock>)>();
                var line = new List<TextBlock>();
                //编排行
                foreach (var word in allBlock)
                {
                    var skText = word;
                    var tempW = wLength;
                    //如果该行容不下这个单词
                    if (wLength + skText.MeasuredWidth > w)
                    {
                        var leftWidth = w - wLength;
                        var maybeLeftCharCount = (int)(leftWidth / blankWCache);
                        //尝试打断这个单词
                        var list = Hyphenator.hyphenate(skText.Text);
                        var count = 0;
                        if (list.Count > 1)
                        {
                            for (var i = 0; i < list.Count - 1; i++)
                            {
                                int lastCount = count;
                                count = lastCount + list[i].Length;
                                int currentCharCount = count;
                                int nextCharCount = count + list[i + 1].Length;
                                var currentBlock = new TextBlock();
                                currentBlock.AddText(skText.Text.Substring(0, currentCharCount) + '-', styleNormal);
                                var nextBlock = new TextBlock();
                                nextBlock.AddText(skText.Text.Substring(0, nextCharCount) + '-', styleNormal);
                                if (wLength + currentBlock.MeasuredWidth <= w && wLength + nextBlock.MeasuredWidth > w)
                                {
                                    line.Add(currentBlock);
                                    currentBlock.HaveFooter = true;
                                    wLength = wLength + currentBlock.MeasuredWidth;
                                    //下一行的
                                    var oldText = skText.Text;
                                    skText = new TextBlock();
                                    skText.AddText(oldText.Substring(currentCharCount, oldText.Length - currentCharCount), styleNormal);
                                    skText.HaveHeader = true;
                                    break;
                                }
                            }
                        }
                        //换行
                        lines.Add((wLength, line));
                        line = new List<TextBlock>();
                        wLength = 0;
                    }

                    line.Add(skText);
                    wLength = wLength + skText.MeasuredWidth + blankWCache;
                }
                lines.Add((wLength, line));
            }
            var h = lineHeightCache * lines.Count + (lines.Count - 1) * LineSpace;
            Console.WriteLine($"[{this.GetType().Name}] Id={this.Id} Method=MeasureParagraph w={w} h={h}");
            return (w, h);
        }

        public override void DrawSentence(ICanvas canvas, int w)
        {
            //绘制
            for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)//每行
            {
                float lineSpace = LineSpace;
                float x = 0;//单词位置

                if (lineIndex == 0)
                {
                    lineSpace = 0;
                    x = blankWCache * BegainSpaceCount;//段落起始位置根据开头空格数
                }

                var lin = lines[lineIndex];
                var gap = w - lin.le;
                var c = (int)Math.Round(gap / (lin.li.Count - 1) + 0.5);//不舍

                for (var wordIndex = 0; wordIndex < lin.li.Count; wordIndex++)//每词
                {
                    var t = lin.li[wordIndex];
                    //最后一行不拉伸
                    if (lineIndex == lines.Count - 1)
                    {
                        t.Paint(canvas, new SKPoint(x, 0 + lineHeightCache * lineIndex + lineSpace * lineIndex), lineHeightCache);
                        x = x + t.MeasuredWidth + blankWCache;
                    }
                    else
                    {
                        //拉伸行的最后一词强制对齐
                        /*if (j == lin.li.Count - 1)
                        {
                            if (x + t.MeasuredWidth != w)
                            {
                                t.Paint(canvas, new SKPoint(w - t.MeasuredWidth, 0 + lineHeightCache * i + lineSpace * i), lineHeightCache);
                                continue;
                            }
                        }*/

                        t.Paint(canvas, new SKPoint(x, 0 + lineHeightCache * lineIndex + lineSpace * lineIndex), lineHeightCache);
                        if (x < w)
                        {
                            if (gap >= 1)
                            {
                                x = x + t.MeasuredWidth + blankWCache + c;
                                gap = gap - c;
                            }
                            else
                            {
                                x = x + t.MeasuredWidth + blankWCache;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 其使用空格切割段落, 然后为每个词创建一个TextWordBlock.
    /// 其目的是看这样使用的性能怎么样, 像Android平台在StaticLayout下还有Spannable, 直接在StaticLayout层就能操作的话, 跨平台实现能简单点.
    /// </summary>
    public partial class JustifyParagraphLabelNormal : EnhanceGraphicsView, IDrawable, IView
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

        public JustifyParagraphLabelNormal()
        {
            //this.PaintSurface += View_PaintSurface;
            this.Drawable = this;
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
        MauiFont font;// = "Impact";//"Microsoft YaHei UI"

        /// <summary>
        /// 行间距
        /// </summary>
        [AutoBindable]
        int lineSpace = 0;

        [AutoBindable(DefaultValue = "Microsoft.Maui.Graphics.Colors.Black")]
        Color fontColor;

        //[AutoBindable]
        //SKColor backgroundColor;

        /// <summary>
        /// 段落切分后的全部block. 用于存储字符串数据.
        /// </summary>
        protected List<TextBlock> allBlock;

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
        protected List<(float le, List<TextBlock> li)> lines;
        /// <summary>
        /// 缓存计算的单行高度, px
        /// </summary>
        protected float lineHeightCache;
        /// <summary>
        /// 空格宽度, px
        /// </summary>
        protected float blankWCache;
        /// <summary>
        /// 空格
        /// </summary>
        protected TextBlock skTextOfBlank;

        /// <summary>
        /// 上一次的宽, 如果宽改变, 需要重新布局, dp
        /// </summary>
        protected double lastWidth;

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
                var r = MeasureParagraph((float)widthConstraint);
                lastWidth = widthConstraint;
                return new Size(widthConstraint, r.h);
            }
            else
                return base.CustomMeasuredSize(widthConstraint, heightConstraint);
        }

        protected PlatformStyle styleNormal;

        public virtual (float w, float h) MeasureParagraph(float w)
        {
            // Create normal style
            styleNormal = new PlatformStyle()
            {
                FontSize = FontSize,
                Font = Font,
                FontColor = FontColor,
            };

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
                //blankW = skTextOfBlank.MeasuredWidth;
                //lineHeightCache = skTextOfBlank.MeasuredHeight;

                var m = new TextBlock() { };
                m.AddText("m", styleNormal);
                var n = new TextBlock() { };
                n.AddText("n", styleNormal);
                var nm = new TextBlock() { };
                nm.AddText("n m", styleNormal);

                blankWCache = nm.MeasuredWidth - n.MeasuredWidth - n.MeasuredWidth;
                lineHeightCache = nm.MeasuredHeight;
#if DEBUG
                Console.WriteLine($"m w={m.MeasuredWidth} h={m.MeasuredHeight}");
                Console.WriteLine($"n w={n.MeasuredWidth} h={n.MeasuredHeight}");
                Console.WriteLine($"space w={skTextOfBlank.MeasuredWidth} h={skTextOfBlank.MeasuredHeight}");
                Console.WriteLine($"n m w={nm.MeasuredWidth} h={nm.MeasuredHeight}");
#endif
            }

            float wLength = blankWCache * BegainSpaceCount;//起始位置根据开头空格数
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
                    wLength = wLength + skText.MeasuredWidth + blankWCache;
                }
                lines.Add((wLength, line));
            }
            var h = lineHeightCache * lines.Count + (lines.Count - 1) * LineSpace;
            Console.WriteLine($"[{this.GetType().Name}] Id={this.Id} Method=MeasureParagraph w={w} h={h}");
            return (w, h);
        }

        public virtual void DrawSentence(ICanvas canvas, int w)
        {
            //绘制
            for (int i = 0; i < lines.Count; i++)
            {
                float lineSpace = LineSpace;
                float x = 0;//单词位置

                if (i == 0)
                {
                    lineSpace = 0;
                    x = blankWCache * BegainSpaceCount;//段落起始位置根据开头空格数
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
                        t.Paint(canvas, new SKPoint(x, 0 + lineHeightCache * i + lineSpace * i), lineHeightCache);
                        x = x + t.MeasuredWidth + blankWCache;
                    }
                    else
                    {
                        //拉伸行的最后一词强制对齐
                        if (j == lin.li.Count - 1)
                        {
                            if (x + t.MeasuredWidth != w)
                            {
                                t.Paint(canvas, new SKPoint(w - t.MeasuredWidth, 0 + lineHeightCache * i + lineSpace * i), lineHeightCache);
                                continue;
                            }
                        }

                        t.Paint(canvas, new SKPoint(x, 0 + lineHeightCache * i + lineSpace * i), lineHeightCache);
                        if (x < w)
                        {
                            if (gap >= 1)
                            {
                                x = x + t.MeasuredWidth + blankWCache + c;
                                gap = gap - c;
                            }
                            else
                            {
                                x = x + t.MeasuredWidth + blankWCache;
                            }
                        }
                    }
                }
            }
        }

        //private void View_PaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Console.WriteLine($"[{this.GetType().Name}] Id={this.Id} Method=Draw dirtyRect={dirtyRect}");
            canvas.FillColor = Colors.Red;
            canvas.DrawRectangle(dirtyRect);
            //MeasureParagraph((int)dirtyRect.Width);
            if (lines != null)
                DrawSentence(canvas, (int)(dirtyRect.Width));
        }

        public class TextBlock : IDisposable
        {
            public static bool DrawDebugBounds = false;
            public bool HaveHeader = false;
            public bool HaveFooter = false;

            TextWordBlock wordBlock;
            public TextBlock()
            {

            }

            public float MeasuredWidth
            {
                get
                {
                    return wordBlock.MeasuredSize.Width;
                }
            }

            public float MeasuredHeight
            {
                get
                {
                    return wordBlock.MeasuredSize.Height;
                }
            }
            public string Text;
            internal void AddText(string word, PlatformStyle styleNormal)
            {
                Text = word;
                this.wordBlock = new TextWordBlock(word, styleNormal.Font, styleNormal.FontSize, styleNormal.FontColor, null);
            }

            internal void Paint(ICanvas canvas, SKPoint sKPoint, float lineHeight)
            {
                //canvas.FillColor = styleNormal.TextColor.ToMauiColor().WithAlpha(0.5f);
                if (DrawDebugBounds)
                {
                    canvas.StrokeColor = Colors.Red;
                    canvas.DrawRectangle(sKPoint.X, sKPoint.Y, MeasuredWidth - 1, MeasuredHeight - 1);
                }
                wordBlock.Paint(canvas, sKPoint.X, sKPoint.Y);
            }

            public void Dispose()
            {

            }
        }

        public class PlatformStyle : Style
        {
            public Color FontColor;

            public MauiFont Font;
        }
    }
}
