using Microsoft.Maui.Controls;
using Yang.Maui.Helper.Controls.ScrollViewExperiment;

namespace Yang.Maui.Helper.Maui.Test.Pages;

public partial class TableViewTestPage : ContentPage
{
    public TableViewTestPage()
    {
        InitializeComponent();
        var tableView = new Yang.Maui.Helper.Controls.ScrollViewExperiment.TableView();
        Content = tableView;
        tableView.VerticalScrollBarVisibility = ScrollBarVisibility.Always;
        tableView.Delegate = new Delegate();
        tableView.DataSource = new Source();

        var click = new TapGestureRecognizer();
        click.Tapped += (s, e) =>
        {
            var p = e.GetPosition(tableView);
#if IOS
            var indexPath = tableView.IndexPathForRowAtPointOfContentView(p.Value);
#else
            var indexPath = tableView.IndexPathForVisibaleRowAtPointOfTableView(p.Value);
#endif
            if(indexPath != null)
                tableView.SelectRowAtIndexPath(indexPath, false, TableViewScrollPosition.None);
        };
        tableView.Content.GestureRecognizers.Add(click);
        var headerView = new Grid()
        {
            HeightRequest = 50,
            BackgroundColor = Colors.Red,
            Children =
            {
                new Label(){ Text = "Header", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center },
            }
        };
        tableView.TableHeaderView = headerView;
        tableView.TableFooterView = new Grid()
        {
            HeightRequest = 50,
            BackgroundColor = Colors.Red,
            Children =
            {
                new Button(){Text = "Footer", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center } 
            }
        };

    }

    class Delegate : Yang.Maui.Helper.Controls.ScrollViewExperiment.ITableViewDelegate
    {
        public Delegate()
        {
            //heightForRowAtIndexPath += heightForRowAtIndexPathMethod;
        }

        public float heightForRowAtIndexPathMethod(Controls.ScrollViewExperiment.TableView tableView, Controls.ScrollViewExperiment.NSIndexPath indexPath)
        {
            return 100;
        }

        public willXRowAtIndexPathDelegate willSelectRowAtIndexPath { get; }

        public willXRowAtIndexPathDelegate willDeselectRowAtIndexPath { get; }

        public didXRowAtIndexPathDelegate didSelectRowAtIndexPath { get; }

        public didXRowAtIndexPathDelegate didDeselectRowAtIndexPath { get; }

        public heightForRowAtIndexPathDelegate heightForRowAtIndexPath { get; }

        public heightForXInSectionDelegate heightForHeaderInSection { get; }

        public heightForXInSectionDelegate heightForFooterInSection { get; }

        public viewForHeaderInSectionDelegate viewForHeaderInSection { get; }

        public viewForHeaderInSectionDelegate viewForFooterInSection { get; }

        public XRowAtIndexPathDelegate willBeginEditingRowAtIndexPath { get; }

        public XRowAtIndexPathDelegate didEndEditingRowAtIndexPath { get; }

        public titleForDeleteConfirmationButtonForRowAtIndexPathDelegate titleForDeleteConfirmationButtonForRowAtIndexPath { get; }
    }

    class Source : Yang.Maui.Helper.Controls.ScrollViewExperiment.ITableViewDataSource
    {
        public numberOfRowsInSectionDelegate numberOfRowsInSection { get; set; }

        public cellForRowAtIndexPathDelegate cellForRowAtIndexPath { get; }

        public numberOfSectionsInTableViewDelegate numberOfSectionsInTableView { get; }

        public titleForXInSectionDelegate titleForHeaderInSection { get; }

        public titleForXInSectionDelegate titleForFooterInSection { get; }

        public commitEditingStyleDelegate commitEditingStyle { get; }

        public canEditRowAtIndexPathDelegate canEditRowAtIndexPath { get; }

        public Source()
        {
            numberOfRowsInSection += numberOfRowsInSectionMethod;
            cellForRowAtIndexPath += cellForRowAtIndexPathMethod;
        }

        public int numberOfRowsInSectionMethod(Controls.ScrollViewExperiment.TableView tableView, int section)
        {
            return 500;
        }
        static int newCellCount = 0;
        //给每个cell设置ID号（重复利用时使用）
        static string cellID1 = "cellID1";
        static string cellID2 = "cellID2";
        public TableViewCell cellForRowAtIndexPathMethod(Controls.ScrollViewExperiment.TableView tableView, Controls.ScrollViewExperiment.NSIndexPath indexPath)
        {
            //从tableView的一个队列里获取一个cell
            if (indexPath.Row % 2 == 0)
            {
                TableViewCell cell = tableView.dequeueReusableCellWithIdentifier(cellID1);

                //判断队列里面是否有这个cell 没有自己创建，有直接使用
                if (cell == null)
                {
                    //没有,创建一个
                    cell = new Cell1(TableViewCellStyle.Default, cellID1) { };
                    (cell as Cell1).NewCellIndex = ++newCellCount;
                    Console.WriteLine($"newCell: {newCellCount}");
                }

                //使用cell
                cell.TextLabel.Text = $"Position={indexPath.Row} newCellIndex={(cell as Cell).NewCellIndex}";
                (cell as Cell1).Image.Source = "https://ydlunacommon-cdn.nosdn.127.net/cb776e6995f1c703706cf8c4c39a7520.png";
                return cell;
            }
            else
            {
                TableViewCell cell = tableView.dequeueReusableCellWithIdentifier(cellID2);

                //判断队列里面是否有这个cell 没有自己创建，有直接使用
                if (cell == null)
                {
                    //没有,创建一个
                    cell = new Cell(TableViewCellStyle.Default, cellID2) { };
                    cell.IsClippedToBounds = true;
                    (cell as Cell).NewCellIndex = ++newCellCount;
                    Console.WriteLine($"newCell: {newCellCount}");
                }

                //使用cell
                cell.TextLabel.Text = $"哈哈哈！！！Position={indexPath.Row} newCellIndex={(cell as Cell).NewCellIndex}";
                return cell;
            }
        }
    }

    class Cell : TableViewCell
    {
        public int NewCellIndex;

        public Cell(TableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
        {
            this.SelectedBackgroundView = new Grid() { BackgroundColor = Colors.Red};
        }

        public override void PrepareForReuse()
        {
            TextLabel.Text = null;
        }
    }

    class Cell1 : Cell
    {
        public Cell1(TableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
        {
            Image = new Microsoft.Maui.Controls.Image() { };
            var indicator = new ActivityIndicator { Color = new Color(0.5f), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            indicator.BindingContext = Image;
            this.ContentView.Add(Image);
            this.ContentView.Add(indicator);
        }

        public Microsoft.Maui.Controls.Image Image;

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            Image.Source = null;
        }
    }
}