using CoreGraphics;
using Foundation;
using System;
using UIKit;
using Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS;
using UITableViewCell = Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS.UITableViewCell;
using UITableViewCellStyle = Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS.UITableViewCellStyle;

namespace Yang.Maui.Helper.Test
{
    public class MainPage
    {
        public UIView Page;
        public MainPage(CGRect frame)
        {
            var tableView = new Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS.UITableView(frame)
            {
                //Axis = UILayoutConstraintAxis.Vertical,
            };
            Page = tableView;

            tableView.Delegate = new Delegate();
            tableView.DataSource = new Source();

            var headerView = new UIStackView(new CGRect(0, 0, frame.Width / 2, frame.Height / 2)) { Axis = UILayoutConstraintAxis.Vertical, BackgroundColor = UIColor.Green, Spacing = 10 };
            tableView.TableHeaderView = headerView;
            tableView.TableFooterView = new UIView(new CGRect(0, 0, frame.Width / 2, frame.Height / 2)) { BackgroundColor = UIColor.Green };

            var scrollToRowTestButton = CreateTestButton("scrollToRowTest", () =>
            {
                tableView.ScrollToRowAtIndexPath(Controls.ScrollViewExperiment.NSIndexPath.FromRowSection((int)(tableView.DataSource.numberOfRowsInSection(tableView, 0) - 1), 0), Controls.ScrollViewExperiment.iOS.UITableViewScrollPosition.None, false);
            });
            var scrollToSelectedTestButton = CreateTestButton("scrollToSelectedTest", () =>
            {
            });
            headerView.AddArrangedSubview(scrollToRowTestButton);
            headerView.AddArrangedSubview(scrollToSelectedTestButton);
        }

        UIButton CreateTestButton(string title, Action action)
        {
            var button = new UIButton() { BackgroundColor = UIColor.Red };
            button.SetTitle(title, UIControlState.Normal);
            button.TouchUpInside += (sender, e) =>
            {
                action.Invoke();
            };
            return button;
        }

        public UIView GetPage()
        {
            return Page;
        }

        class Delegate : NSObject, Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS.IUITableViewDelegate
        {
            public Delegate()
            {
                heightForRowAtIndexPath += heightForRowAtIndexPathMethod;
            }

            public float heightForRowAtIndexPathMethod(Controls.ScrollViewExperiment.iOS.UITableView tableView, Controls.ScrollViewExperiment.NSIndexPath indexPath)
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

        class Source : NSObject, Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS.UITableViewDataSource
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

            public int numberOfRowsInSectionMethod(Controls.ScrollViewExperiment.iOS.UITableView tableView, int section)
            {
                return 100;
            }
            static int newCellCount = 0;
            //给每个cell设置ID号（重复利用时使用）
            static NSString cellID = new NSString("cellID");
            public UITableViewCell cellForRowAtIndexPathMethod(Controls.ScrollViewExperiment.iOS.UITableView tableView, Controls.ScrollViewExperiment.NSIndexPath indexPath)
            {
                //从tableView的一个队列里获取一个cell
                UITableViewCell cell = tableView.dequeueReusableCellWithIdentifier(cellID);

                //判断队列里面是否有这个cell 没有自己创建，有直接使用
                if (cell == null)
                {
                    //没有,创建一个
                    cell = new Cell(UITableViewCellStyle.Default, cellID);
                    (cell as Cell).NewCellIndex = ++newCellCount;
                    Console.WriteLine($"newCell: {newCellCount}");
                }

                //使用cell
                cell.TextLabel.Text = $"哈哈哈！！！Position={indexPath.Row} newCellIndex={(cell as Cell).NewCellIndex}";
                return cell;
            }
        }

        class Cell : UITableViewCell
        {
            public int NewCellIndex;

            public Cell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
            {
                this.SelectedBackgroundView = new UIView() { BackgroundColor = UIColor.Green };
            }
        }
    }
}
