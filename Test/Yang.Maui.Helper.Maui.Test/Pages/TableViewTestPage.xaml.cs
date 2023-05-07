using Yang.Maui.Helper.Controls.ScrollViewExperiment;

namespace Yang.Maui.Helper.Maui.Test.Pages;

public partial class TableViewTestPage : ContentPage
{
	public TableViewTestPage()
	{
		InitializeComponent();
        var tableView = new Yang.Maui.Helper.Controls.ScrollViewExperiment.TableView();
		Content = tableView;

        tableView.Delegate = new Delegate();
        tableView.DataSource = new Source();

        var headerView = new VerticalStackLayout() { HeightRequest = 50, BackgroundColor = Colors.Green, Spacing = 10 };
        tableView.TableHeaderView = headerView;
        tableView.TableFooterView = new ContentView() { HeightRequest = 50, BackgroundColor = Colors.Green };

    }

    class Delegate : Yang.Maui.Helper.Controls.ScrollViewExperiment.ITableViewDelegate
    {
        public Delegate()
        {
            heightForRowAtIndexPath += heightForRowAtIndexPathMethod;
        }

        public float heightForRowAtIndexPathMethod(Controls.ScrollViewExperiment.TableView tableView, Controls.ScrollViewExperiment.NSIndexPath indexPath)
        {
            return 50;
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

    class Source :  Yang.Maui.Helper.Controls.ScrollViewExperiment.ITableViewDataSource
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
            return 100;
        }
        static int newCellCount = 0;
        //给每个cell设置ID号（重复利用时使用）
        static string cellID = "cellID";
        public TableViewCell cellForRowAtIndexPathMethod(Controls.ScrollViewExperiment.TableView tableView, Controls.ScrollViewExperiment.NSIndexPath indexPath)
        {
            //从tableView的一个队列里获取一个cell
            TableViewCell cell = tableView.dequeueReusableCellWithIdentifier(cellID);

            //判断队列里面是否有这个cell 没有自己创建，有直接使用
            if (cell == null)
            {
                //没有,创建一个
                cell = new Cell(TableViewCellStyle.Default, cellID) { HeightRequest = 100 };
                (cell as Cell).NewCellIndex = ++newCellCount;
                Console.WriteLine($"newCell: {newCellCount}");
            }

            //使用cell
            cell.TextLabel.Text = $"哈哈哈！！！Position={indexPath.Row} newCellIndex={(cell as Cell).NewCellIndex}";
            return cell;
        }
    }

    class Cell : TableViewCell
    {
        public int NewCellIndex;

        public Cell(TableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
        {
            this.SelectedBackgroundView = new Grid() { BackgroundColor = Colors.Green };
        }
    }
}