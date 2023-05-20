using UIView = Microsoft.Maui.Controls.Layout;
namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    public enum TableViewStyle
    {
        Plain, Grouped
    }

    public enum TableViewScrollPosition
    {
        None, Top, Middle, Bottom
    }

    public enum TableViewRowAnimation
    {
        Fade, Right, Left, Top, Bottom, None, Middle, Automatic = 100
    }

    public delegate NSIndexPath willXRowAtIndexPathDelegate(TableView tableView, NSIndexPath indexPath);
    public delegate void didXRowAtIndexPathDelegate(TableView tableView, NSIndexPath indexPath);
    public delegate float heightForRowAtIndexPathDelegate(TableView tableView, NSIndexPath indexPath);
    public delegate float heightForXInSectionDelegate(TableView tableView, int section);
    public delegate UIView viewForHeaderInSectionDelegate(TableView tableView, int section);
    public delegate void XRowAtIndexPathDelegate(TableView tableView, NSIndexPath indexPath);
    public delegate string titleForDeleteConfirmationButtonForRowAtIndexPathDelegate(TableView tableView, NSIndexPath indexPath);
    public interface ITableViewDelegate
    {
        willXRowAtIndexPathDelegate willSelectRowAtIndexPath { get; }
        willXRowAtIndexPathDelegate willDeselectRowAtIndexPath { get; }

        didXRowAtIndexPathDelegate didSelectRowAtIndexPath { get; }
        didXRowAtIndexPathDelegate didDeselectRowAtIndexPath { get; }

        heightForRowAtIndexPathDelegate heightForRowAtIndexPath { get; }
        heightForXInSectionDelegate heightForHeaderInSection { get; }
        heightForXInSectionDelegate heightForFooterInSection { get; }

        viewForHeaderInSectionDelegate viewForHeaderInSection { get; }
        viewForHeaderInSectionDelegate viewForFooterInSection { get; }

        XRowAtIndexPathDelegate willBeginEditingRowAtIndexPath { get; }
        XRowAtIndexPathDelegate didEndEditingRowAtIndexPath { get; }

        titleForDeleteConfirmationButtonForRowAtIndexPathDelegate titleForDeleteConfirmationButtonForRowAtIndexPath { get; }
    }

    public delegate int numberOfRowsInSectionDelegate(TableView tableView, int section);
    public delegate TableViewCell cellForRowAtIndexPathDelegate(TableView tableView, NSIndexPath indexPath);
    public delegate int numberOfSectionsInTableViewDelegate(TableView tableView);
    public delegate string titleForXInSectionDelegate(TableView tableView, int section);
    public delegate string EditintitleForFooterInSectionDelegate(TableView tableView, int section);
    public delegate void commitEditingStyleDelegate(TableView tableView, TableViewCellEditingStyle editingStyle, NSIndexPath indexPath);
    public delegate bool canEditRowAtIndexPathDelegate(TableView tableView, NSIndexPath indexPath);

    /// <summary>
    /// 最新的Api见
    /// https://learn.microsoft.com/en-us/dotnet/api/uikit.uitableviewdatasource.caneditrow?view=xamarin-ios-sdk-12
    /// </summary>
    public interface ITableViewDataSource
    {
        public numberOfRowsInSectionDelegate numberOfRowsInSection { get; }
        public cellForRowAtIndexPathDelegate cellForRowAtIndexPath { get; }
        public numberOfSectionsInTableViewDelegate numberOfSectionsInTableView { get; }
        public titleForXInSectionDelegate titleForHeaderInSection { get; }
        public titleForXInSectionDelegate titleForFooterInSection { get; }
        public commitEditingStyleDelegate commitEditingStyle { get; }
        public canEditRowAtIndexPathDelegate canEditRowAtIndexPath { get; }
    }

    public partial class TableView : ScrollView
    {
        // http://stackoverflow.com/questions/235120/whats-the-uitableview-index-magnifying-glass-character
        const string UITableViewIndexSearch = @"{search}";

        static readonly float _UITableViewDefaultRowHeight = 43;

        #region https://github.com/BigZaphod/Chameleon/blob/master/UIKit/Classes/UITableView.h
        TableViewStyle _style;
        ITableViewDelegate _delegate;
        ITableViewDataSource _dataSource;

        TableViewCellSeparatorStyle _separatorStyle;

        Color separatorColor;
        UIView _tableHeaderView;
        View _tableFooterView;
        UIView _backgroundView;
        bool allowsSelection;
        bool allowsSelectionDuringEditing;
        bool editing;

        float _sectionHeaderHeight;
        float _sectionFooterHeight;
        #endregion

        bool _needsReload;
        NSIndexPath _selectedRow;
        NSIndexPath _highlightedRow;
        /// <summary>
        /// 当前显示
        /// </summary>
        Dictionary<NSIndexPath, TableViewCell> _cachedCells;
        List<TableViewCell> _reusableCells;
        List<TableViewSection> _sections;
        delegateHas _delegateHas;
        struct delegateHas
        {
            public bool heightForRowAtIndexPath = true;
            public bool heightForHeaderInSection = true;
            public bool heightForFooterInSection = true;
            public bool viewForHeaderInSection = true;
            public bool viewForFooterInSection = true;
            public bool willSelectRowAtIndexPath = true;
            public bool didSelectRowAtIndexPath = true;
            public bool willDeselectRowAtIndexPath = true;
            public bool didDeselectRowAtIndexPath = true;
            public bool willBeginEditingRowAtIndexPath = true;
            public bool didEndEditingRowAtIndexPath = true;
            public bool titleForDeleteConfirmationButtonForRowAtIndexPath = true;

            public delegateHas()
            {
            }
        }

        dataSourceHas _dataSourceHas;
        struct dataSourceHas
        {
            public bool numberOfSectionsInTableView = true;
            public bool titleForHeaderInSection = true;
            public bool titleForFooterInSection = true;
            public bool commitEditingStyle = true;
            public bool canEditRowAtIndexPath = true;

            public dataSourceHas()
            {
            }
        }

        void Init(TableViewStyle style)
        {
            this._style = style;
            this._cachedCells = new();
            this._sections = new();
            this._reusableCells = new();
            this.separatorColor = new Color(red: 0.88f, green: 0.88f, blue: 0.88f, alpha: 1);
            this._separatorStyle = TableViewCellSeparatorStyle.SingleLine;
            this.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
            this.allowsSelection = true;
            this.allowsSelectionDuringEditing = false;
            this._sectionHeaderHeight = this._sectionFooterHeight = 22;
            //this.AlwaysBounceVertical = true;
            if (style == TableViewStyle.Plain)
            {
                this.BackgroundColor = Colors.White;
            }
            this._setNeedsReload();
        }

        public ITableViewDataSource DataSource
        {
            get { return this._dataSource; }
            set
            {
                _dataSource = value;

                _dataSourceHas.numberOfSectionsInTableView = _dataSource.numberOfSectionsInTableView != null;
                _dataSourceHas.titleForHeaderInSection = _dataSource.titleForHeaderInSection != null;
                _dataSourceHas.titleForFooterInSection = _dataSource.titleForFooterInSection != null;
                _dataSourceHas.commitEditingStyle = _dataSource.commitEditingStyle != null;
                _dataSourceHas.canEditRowAtIndexPath = _dataSource.canEditRowAtIndexPath != null;

                this._setNeedsReload();
            }
        }

        public new ITableViewDelegate Delegate
        {
            get { return this._delegate; }
            set
            {
                //base.Delegate = value;
                _delegate = value;
                _delegateHas.heightForRowAtIndexPath = _delegate.heightForRowAtIndexPath != null;
                _delegateHas.heightForHeaderInSection = _delegate.heightForHeaderInSection != null;
                _delegateHas.heightForFooterInSection = _delegate.heightForFooterInSection != null;
                _delegateHas.viewForHeaderInSection = _delegate.viewForHeaderInSection != null;
                _delegateHas.viewForFooterInSection = _delegate.viewForFooterInSection != null;
                _delegateHas.willSelectRowAtIndexPath = _delegate.willSelectRowAtIndexPath != null;
                _delegateHas.didSelectRowAtIndexPath = _delegate.didSelectRowAtIndexPath != null;
                _delegateHas.willDeselectRowAtIndexPath = _delegate.willDeselectRowAtIndexPath != null;
                _delegateHas.didDeselectRowAtIndexPath = _delegate.didDeselectRowAtIndexPath != null;
                _delegateHas.willBeginEditingRowAtIndexPath = _delegate.willBeginEditingRowAtIndexPath != null;
                _delegateHas.didEndEditingRowAtIndexPath = _delegate.didEndEditingRowAtIndexPath != null;
                _delegateHas.titleForDeleteConfirmationButtonForRowAtIndexPath = _delegate.titleForDeleteConfirmationButtonForRowAtIndexPath != null;
            }
        }

        void _layoutTableView()
        {
            // lays out headers and rows that are visible at the time. this should also do cell
            // dequeuing and keep a list of all existing cells that are visible and those
            // that exist but are not visible and are reusable
            // if there's no section cache, no rows will be laid out but the header/footer will (if any).

            Size boundsSize = this.Bounds.Size;
            var contentOffset = this.ScrollY; //ContentOffset.Y;
            Rect visibleBounds = new Rect(0, contentOffset, boundsSize.Width, boundsSize.Height);
            double tableHeight = 0;

            if (_tableHeaderView != null)
            {
                LayoutChild(_tableHeaderView, new Rect(0, tableHeight, visibleBounds.Width, _tableHeaderView.DesiredSize.Height));
                tableHeight += _tableHeaderView.DesiredSize.Height;
            }

            // layout sections and rows
            int numberOfSections = _sections.Count;
            for (int section = 0; section < numberOfSections; section++)
            {
                TableViewSection sectionRecord = _sections[section];
                int numberOfRows = sectionRecord.numberOfRows;

                for (int row = 0; row < numberOfRows; row++)
                {
                    NSIndexPath indexPath = NSIndexPath.FromRowSection(row, section);
                    //尝试用之前测量的值或者预设值估计底部在哪
                    var rowMaybeTop = tableHeight;
                    var rowMaybeBottom = tableHeight + (sectionRecord._rowHeights[row] != 0 ? sectionRecord._rowHeights[row] : EstimatedRowHeight);
                    //如果在可见区域, 就详细测量
                    if ((rowMaybeTop >= visibleBounds.Top && rowMaybeTop <= visibleBounds.Bottom)
                        || (rowMaybeBottom >= visibleBounds.Top && rowMaybeBottom <= visibleBounds.Bottom)
                        || (rowMaybeTop <= visibleBounds.Top && rowMaybeBottom >= visibleBounds.Bottom))
                    {
                        //获取Cell, 优先获取之前已经被显示的
                        if (!_cachedCells.ContainsKey(indexPath)) continue;
                        TableViewCell cell = _cachedCells[indexPath];
                        if (cell != null)
                        {
                            //测量高度
                            LayoutChild(cell, new Rect(0, tableHeight, visibleBounds.Width, cell.DesiredSize.Height));

                            tableHeight += cell.DesiredSize.Height;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    else//如果不可见
                    {
                        tableHeight = rowMaybeBottom;
                    }
                }
            }

            if (_tableFooterView != null)
            {
                LayoutChild(_tableFooterView, new Rect(0, tableHeight, visibleBounds.Width, _tableFooterView.DesiredSize.Height));
            }

            foreach (TableViewCell cell in _reusableCells)
            {
                LayoutChild(cell, new Rect(0, -3000, cell.DesiredSize.Width, cell.DesiredSize.Height));
            }
        }

        public double EstimatedRowHeight = 20;

        List<NSIndexPath> needRemoveCell = new List<NSIndexPath>();
        /// <summary>
        /// Measure思路:
        /// 总体的高度我们只要一个很大的值, 因为是要保证滑动, 我们只要确定划到最后一个是在最底部就行.
        /// 那么快速滑动时有来不及计算的Row, 我们直接取一个近似固定值, 这样去确定哪一行Row滑动到了可见部分, 然后对之后的Row进行正确的测量
        /// </summary>
        /// <param name="widthConstraint">宽一般为固定值</param>
        /// <param name="heightConstraint">高一般为无限值</param>
        /// <returns></returns>
        Size _measureTableView(double widthConstraint, double heightConstraint)
        {
            //tableView自身的大小
            Size tableViewBoundsSize = new Size(widthConstraint, Bounds.Height > 1 ? Bounds.Height : initTableViewHeightConstraintWhenMeasure);
            //当前可见区域在ContentView中的位置
            Rect visibleBounds = new Rect(0, this.ScrollY, tableViewBoundsSize.Width, tableViewBoundsSize.Height);
            double tableHeight = 0;

            //表头的View是确定的, 我们可以直接测量
            if (_tableHeaderView != null)
            {
                var _tableHeaderViewH = MeasureChild(_tableHeaderView, widthConstraint, heightConstraint).Request.Height;
                tableHeight += _tableHeaderViewH;
            }

            // 需要重新布局后, cell会变动, 先将之前显示的cell放入可供使用的cell字典
            Dictionary<NSIndexPath, TableViewCell> availableCells = new();
            foreach (var cell in _cachedCells)
                availableCells.Add(cell.Key, cell.Value);
            int numberOfSections = _sections.Count;
            _cachedCells.Clear();

            //复用是从_reusableCells获取的, 需要让不可见的先回收
            var tempCells = availableCells.ToList();
            if (scrollOffset > 0)//往上滑, 上面的需要回收
            {
                foreach (var cell in tempCells)
                {
                    if (cell.Value.DesiredSize.Height < scrollOffset)
                    {
                        needRemoveCell.Add(cell.Key);
                        scrollOffset -= cell.Value.DesiredSize.Height;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else//往下滑, 下面的需要回收
            {
                scrollOffset = -scrollOffset;
                for (int i = tempCells.Count - 1; i >= 0; i--)
                {
                    var cell = tempCells[i];
                    if (cell.Value.DesiredSize.Height < scrollOffset)
                    {
                        needRemoveCell.Add(cell.Key);
                        scrollOffset -= cell.Value.DesiredSize.Height;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            foreach (var indexPath in needRemoveCell)
            {
                var cell = availableCells[indexPath];
                _reusableCells.Add(cell);
                availableCells.Remove(indexPath);
            }
            tempCells.Clear();
            needRemoveCell.Clear();
            scrollOffset = 0;//重置为0, 避免只更新数据时也移除cell

            for (int section = 0; section < numberOfSections; section++)
            {
                TableViewSection sectionRecord = _sections[section];
                int numberOfRows = sectionRecord.numberOfRows;

                for (int row = 0; row < numberOfRows; row++)
                {
                    NSIndexPath indexPath = NSIndexPath.FromRowSection(row, section);
                    //尝试用之前测量的值或者预设值估计底部在哪
                    var rowMaybeTop = tableHeight;
                    var rowMaybeBottom = tableHeight + (sectionRecord._rowHeights[row] != 0 ? sectionRecord._rowHeights[row] : EstimatedRowHeight);
                    //如果在可见区域, 就详细测量
                    if ((rowMaybeTop >= visibleBounds.Top && rowMaybeTop <= visibleBounds.Bottom)
                       || (rowMaybeBottom >= visibleBounds.Top && rowMaybeBottom <= visibleBounds.Bottom)
                       || (rowMaybeTop <= visibleBounds.Top && rowMaybeBottom >= visibleBounds.Bottom))
                    {
                        //获取Cell, 优先获取之前已经被显示的, 这里假定已显示的数据没有变化
                        TableViewCell cell = availableCells.ContainsKey(indexPath) ? availableCells[indexPath] : this._dataSource.cellForRowAtIndexPath(this, indexPath);
                        if (cell != null)
                        {
                            //将Cell添加到正在显示的Cell字典
                            _cachedCells[indexPath] = cell;
                            if (availableCells.ContainsKey(indexPath)) availableCells.Remove(indexPath);
                            //Cell是否是正在被选择的
                            cell.Highlighted = _highlightedRow == null ? false : _highlightedRow.IsEqual(indexPath);
                            cell.Selected = _selectedRow == null ? false : _selectedRow.IsEqual(indexPath);

                            //添加到ScrollView, 必须先添加才有测量值
                            if (!this.ContentView.Children.Contains(cell))
                                this.AddSubview(cell);
                            //测量高度
                            sectionRecord._rowHeights[row] = MeasureChild(cell, tableViewBoundsSize.Width, heightConstraint).Request.Height;

                            //cell.SeparatorStyle:_separatorStyle color:_separatorColor] ;

                            tableHeight += sectionRecord._rowHeights[row];
                        }
                    }
                    else//如果不可见
                    {
                        if (availableCells.ContainsKey(indexPath))
                        {
                            var cell = availableCells[indexPath];
                            if (cell.ReuseIdentifier != default)
                            {
                                _reusableCells.Add(cell);
                                availableCells.Remove(indexPath);
                            }
                            cell.PrepareForReuse();
                        }
                        tableHeight = rowMaybeBottom;
                    }
                }
            }

            // 重新测量后, 需要显示的已经存入缓存的字典, 剩余的放入可重用列表
            foreach (TableViewCell cell in availableCells.Values)
            {
                if (cell.ReuseIdentifier != default)
                {
                    _reusableCells.Add(cell);
                }
                else
                {
                    cell.RemoveFromSuperview();
                }
            }

            // non-reusable cells should end up dealloced after at this point, but reusable ones live on in _reusableCells.

            // now make sure that all available (but unused) reusable cells aren't on screen in the visible area.
            // this is done becaue when resizing a table view by shrinking it's height in an animation, it looks better. The reason is that
            // when an animation happens, it sets the frame to the new (shorter) size and thus recalcuates which cells should be visible.
            // If it removed all non-visible cells, then the cells on the bottom of the table view would disappear immediately but before
            // the frame of the table view has actually animated down to the new, shorter size. So the animation is jumpy/ugly because
            // the cells suddenly disappear instead of seemingly animating down and out of view like they should. This tries to leave them
            // on screen as long as possible, but only if they don't get in the way.
            var allCachedCells = _cachedCells.Values;
            foreach (TableViewCell cell in _reusableCells)
            {
                if (cell.Frame.IntersectsWith(visibleBounds) && !allCachedCells.Contains(cell))
                {
                    //cell.RemoveFromSuperview();
                }
            }

            //表尾的View是确定的, 我们可以直接测量
            if (_tableFooterView != null)
            {
                tableHeight += MeasureChild(_tableFooterView, tableViewBoundsSize.Width, heightConstraint).Request.Height;
            }

            return new Size(tableViewBoundsSize.Width, tableHeight);
        }

        Rect _CGRectFromVerticalOffset(float offset, float height)
        {
            return new Rect(0, offset, this.Bounds.Width > 0 ? this.Bounds.Width : initTableViewWidthConstraintWhenMeasure, height);
        }

        void beginUpdates() { }

        void endUpdates() { }

        public TableViewCell CellForRowAtIndexPath(NSIndexPath indexPath)
        {
            // this is allowed to return nil if the cell isn't visible and is not restricted to only returning visible cells
            // so this simple call should be good enough.
            if (indexPath == null) return null;
            return _cachedCells.ContainsKey(indexPath) ? _cachedCells[indexPath] : null;
        }

        public UIView TableHeaderView
        {
            get => _tableHeaderView;
            set
            {
                if (value != _tableHeaderView)
                {
                    _tableHeaderView = null;// _tableHeaderView?.Dispose();
                    _tableHeaderView = value;
                    this.AddSubview(_tableHeaderView);
                }
            }
        }

        public View TableFooterView
        {
            get => _tableFooterView;
            set
            {
                if (value != _tableFooterView)
                {
                    _tableFooterView = null;//_tableFooterView?.Dispose();
                    _tableFooterView = value;
                    this.AddSubview(_tableFooterView);
                }
            }
        }

        public void setBackgroundView(UIView backgroundView)
        {
            if (_backgroundView != backgroundView)
            {
                _backgroundView = null;//_backgroundView?.Dispose();
                _backgroundView = backgroundView;
                this.InsertSubview(_backgroundView, 0);
            }
        }

        public int NumberOfSections()
        {
            if (_dataSourceHas.numberOfSectionsInTableView)
            {
                return _dataSource.numberOfSectionsInTableView(this);
            }
            else
            {
                return 1;
            }
        }

        public int NumberOfRowsInSection(int section)
        {
            return _dataSource.numberOfRowsInSection(this, section);
        }

        /// <summary>
        /// 清空字典里存储的View, 并且从ScrollView里移除, 重新统计高度
        /// </summary>
        public void ReloadData()
        {
            // clear the caches and remove the cells since everything is going to change
            foreach (var cell in _cachedCells.Values)
                cell.RemoveFromSuperview();//.makeObjectsPerformSelector("removeFromSuperview");
            _reusableCells.ForEach((v) => v.RemoveFromSuperview());//.makeObjectsPerformSelector("removeFromSuperview");
            _reusableCells.Clear();//.removeAllObjects();
            _cachedCells.Clear();//.removeAllObjects();

            // clear prior selection
            this._selectedRow = null;
            this._highlightedRow = null;

            // trigger the section cache to be repopulated
            for (var i = 0; i < NumberOfSections(); i++)
            {
                var section = new TableViewSection();
                section.numberOfRows = NumberOfRowsInSection(i);
                section._rowHeights = new double[section.numberOfRows];
                _sections.Add(section);
            }

            this._needsReload = false;
        }

        void _reloadDataIfNeeded()
        {
            if (_needsReload)
            {
                this.ReloadData();
            }
        }

        void _setNeedsReload()
        {
            _needsReload = true;
            this.InvalidateMeasure();
        }

        public partial Size OnMeasure(double widthConstraint, double heightConstraint)
        {
            this._reloadDataIfNeeded();
            return _measureTableView(widthConstraint, heightConstraint);
        }

        public SizeRequest MeasureChild(Element element, double widthConstraint, double heightConstraint)
        {
            return (element as IView).Measure(widthConstraint, heightConstraint);
        }

        public partial void OnLayoutSubviews()
        {
            if (_backgroundView != null)
                LayoutChild(_backgroundView, Bounds);

            this._layoutTableView();
        }

        public void LayoutChild(Element element, Rect rect)
        {
            (element as IView).Arrange(rect);
        }

        public NSIndexPath IndexPathForSelectedRow()
        {
            return _selectedRow;
        }

        public NSIndexPath IndexPathForCell(TableViewCell cell)
        {
            foreach (NSIndexPath index in _cachedCells.Keys)
            {
                if (_cachedCells[index] == cell)
                {
                    return index;
                }
            }

            return null;
        }

        public void DeselectRowAtIndexPath(NSIndexPath indexPath, bool animated)
        {
            if (indexPath != null && indexPath == _selectedRow)
            {
                var cell = this.CellForRowAtIndexPath(_selectedRow);
                if (cell != null) cell.Selected = false;
                _selectedRow = null;
            }
        }

        public void SelectRowAtIndexPath(NSIndexPath indexPath, bool animated, TableViewScrollPosition scrollPosition)
        {
            // unlike the other methods that I've tested, the real UIKit appears to call reload during selection if the table hasn't been reloaded
            // yet. other methods all appear to rebuild the section cache "on-demand" but don't do a "proper" reload. for the sake of attempting
            // to maintain a similar delegate and dataSource access pattern to the real thing, I'll do it this way here. :)
            this._reloadDataIfNeeded();

            if (_selectedRow != indexPath)
            {
                this.DeselectRowAtIndexPath(_selectedRow, animated);
                _selectedRow = indexPath;
                var cell = this.CellForRowAtIndexPath(_selectedRow);
                if (cell != null)//TODO:不知道为什么有时候为空
                    cell.Selected = true;
            }

            // I did not verify if the real UIKit will still scroll the selection into view even if the selection itself doesn't change.
            // this behavior was useful for Ostrich and seems harmless enough, so leaving it like this for now.
            //this.ScrollToRowAtIndexPath(_selectedRow, scrollPosition, animated);
        }

        void _setUserSelectedRowAtIndexPath(NSIndexPath rowToSelect)
        {
            var Delegate = (this.Delegate as ITableViewDelegate);
            if (_delegateHas.willSelectRowAtIndexPath)
            {
                rowToSelect = Delegate.willSelectRowAtIndexPath(this, rowToSelect);
            }

            NSIndexPath selectedRow = this.IndexPathForSelectedRow();

            if (selectedRow != null && !(selectedRow == rowToSelect))
            {
                NSIndexPath rowToDeselect = selectedRow;

                if (_delegateHas.willDeselectRowAtIndexPath)
                {
                    rowToDeselect = Delegate.willDeselectRowAtIndexPath(this, rowToDeselect);
                }

                this.DeselectRowAtIndexPath(rowToDeselect, false);

                if (_delegateHas.didDeselectRowAtIndexPath)
                {
                    Delegate.didDeselectRowAtIndexPath(this, rowToDeselect);
                }
            }

            this.SelectRowAtIndexPath(rowToSelect, false, TableViewScrollPosition.None);

            if (_delegateHas.didSelectRowAtIndexPath)
            {
                Delegate.didSelectRowAtIndexPath(this, rowToSelect);
            }
        }

        void _scrollRectToVisible(Rect aRect, TableViewScrollPosition scrollPosition, bool animated)
        {
            if (!(aRect == Rect.Zero) && aRect.Size.Height > 0)
            {
                // adjust the rect based on the desired scroll position setting
                switch (scrollPosition)
                {
                    case TableViewScrollPosition.None:
                        break;

                    case TableViewScrollPosition.Top:
                        aRect.Height = this.Bounds.Size.Height;
                        break;

                    case TableViewScrollPosition.Middle:
                        aRect.Y -= (this.Bounds.Size.Height / 2.0f) - aRect.Size.Height;
                        aRect.Height = this.Bounds.Size.Height;
                        break;

                    case TableViewScrollPosition.Bottom:
                        aRect.Y -= this.Bounds.Size.Height - aRect.Size.Height;
                        aRect.Height = this.Bounds.Size.Height;
                        break;
                }

                //this.ScrollRectToVisible(aRect, animated: animated);
                this.ScrollToAsync(aRect.X, aRect.Y, true);
            }
        }

        public void ScrollToRowAtIndexPath(NSIndexPath indexPath, TableViewScrollPosition scrollPosition, bool animated)
        {
            throw new NotImplementedException();
        }

        public TableViewCell dequeueReusableCellWithIdentifier(string identifier)
        {
            foreach (TableViewCell cell in _reusableCells)
            {
                if (cell.ReuseIdentifier == identifier)
                {
                    TableViewCell strongCell = cell;

                    // the above strongCell reference seems totally unnecessary, but without it ARC apparently
                    // ends up releasing the cell when it's removed on this line even though we're referencing it
                    // later in this method by way of the cell variable. I do not like this.
                    _reusableCells.Remove(cell);

                    strongCell.PrepareForReuse();
                    return strongCell;
                }
            }

            return null;
        }

        void setEditing(bool editing, bool animated)
        {
            this.editing = editing;
        }

        void setEditing(bool editing)
        {
            this.setEditing(editing, false);
        }

        public void InsertSections(int[] sections, TableViewRowAnimation animation)
        {
            this.ReloadData();
        }

        public void DeleteSections(int[] sections, TableViewRowAnimation animation)
        {
            this.ReloadData();
        }

        /// <summary>
        /// See <see cref="UIKit.UITableView.InsertRows(NSIndexPath[], UIKit.UITableViewRowAnimation)"/>
        /// </summary>
        /// <param name="indexPaths"></param>
        /// <param name="animation"></param>
        public void InsertRowsAtIndexPaths(NSIndexPath[] indexPaths, TableViewRowAnimation animation)
        {
            this.ReloadData();
        }

        public void DeleteRowsAtIndexPaths(NSIndexPath[] indexPaths, TableViewRowAnimation animation)
        {
            this.ReloadData();
        }

        /// <summary>
        /// 可见的区域中的点在哪一行
        /// </summary>
        /// <param name="point">相对于TableView的位置, 可以是在TableView上设置手势获取的位置</param>
        /// <returns></returns>
        public NSIndexPath IndexPathForVisibaleRowAtPointOfTableView(Point point)
        {
            var contentOffset = ScrollY;
            point.Y = point.Y + contentOffset;//相对于content
            return IndexPathForRowAtPointOfContentView(point);
        }

        /// <summary>
        /// 迭代全部内容计算点在哪
        /// </summary>
        /// <param name="point">相对与Content的位置</param>
        /// <returns></returns>
        public NSIndexPath IndexPathForRowAtPointOfContentView(Point point)
        {
            double totalHeight = 0;
            double tempBottom = 0;
            if (_tableHeaderView != null)
            {
                tempBottom = totalHeight + _tableHeaderView.DesiredSize.Height;
                if (totalHeight <= point.Y && tempBottom >= point.Y)
                {
                    return null;
                }
                totalHeight = tempBottom;
            }

            var number = NumberOfSections();
            for (int section = 0; section < number; section++)
            {
                TableViewSection sectionRecord = _sections[section];
                int numberOfRows = sectionRecord.numberOfRows;
                for (int row = 0; row < numberOfRows; row++)
                {
                    tempBottom = totalHeight + sectionRecord._rowHeights[row];
                    if (totalHeight <= point.Y && tempBottom >= point.Y)
                    {
                        return NSIndexPath.FromRowSection(row, section);
                    }
                    else
                    {
                        totalHeight = tempBottom;
                    }
                }
            }

            return null;
        }

        /*public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            if (_highlightedRow == null)
            {
                UITouch touch = touches.AnyObject as UITouch;
                CGPoint location = touch.LocationInView(this);

                _highlightedRow = this.IndexPathForRowAtPoint(location);
                if (_highlightedRow != null)
                    this.CellForRowAtIndexPath(_highlightedRow).Highlighted = true;
            }

            if (_highlightedRow != null)
            {
                this.CellForRowAtIndexPath(_highlightedRow).Highlighted = false;
                this._setUserSelectedRowAtIndexPath(_highlightedRow);
                _highlightedRow = null;
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            if (_highlightedRow != null)
            {
                this.CellForRowAtIndexPath(_highlightedRow).Highlighted = false;
                _highlightedRow = null;
            }
        }*/

        bool _canEditRowAtIndexPath(NSIndexPath indexPath)
        {
            // it's YES by default until the dataSource overrules
            return _dataSourceHas.commitEditingStyle && (!_dataSourceHas.canEditRowAtIndexPath || _dataSource.canEditRowAtIndexPath(this, indexPath));
        }

        void _beginEditingRowAtIndexPath(NSIndexPath indexPath)
        {
            if (this._canEditRowAtIndexPath(indexPath))
            {
                this.editing = true;

                if (_delegateHas.willBeginEditingRowAtIndexPath)
                {
                    (this.Delegate as ITableViewDelegate).willBeginEditingRowAtIndexPath(this, indexPath);
                }

                // deferring this because it presents a modal menu and that's what we do everywhere else in Chameleon
                _showEditMenuForRowAtIndexPath(indexPath);//this.PerformSelector(new ObjCRuntime.Selector(nameof(_showEditMenuForRowAtIndexPath)), indexPath, afterDelay: 0, null);
            }
        }

        void _endEditingRowAtIndexPath(NSIndexPath indexPath)
        {
            if (this.editing)
            {
                this.editing = false;

                if (_delegateHas.didEndEditingRowAtIndexPath)
                {
                    (this.Delegate as ITableViewDelegate).didEndEditingRowAtIndexPath(this, indexPath);
                }
            }
        }

        void _showEditMenuForRowAtIndexPath(NSIndexPath indexPath)
        {
            // re-checking for safety since _showEditMenuForRowAtIndexPath is deferred. this may be overly paranoid.
            if (this._canEditRowAtIndexPath(indexPath))
            {
                /*UITableViewCell cell = this.cellForRowAtIndexPath(indexPath);
                string menuItemTitle = null;

                // fetch the title for the delete menu item
                if (_delegateHas.titleForDeleteConfirmationButtonForRowAtIndexPath)
                {
                    menuItemTitle = (this.Delegate as UITableViewDelegate).titleForDeleteConfirmationButtonForRowAtIndexPath(this, indexPath);
                }
                if(menuItemTitle.Length == 0)
                {
                    menuItemTitle = @"Delete";
                }
                cell.Highlighted = true;
                UIMenuItem theItem = new UIMenuItem(menuItemTitle, null);// [[NSMenuItem alloc] initWithTitle: menuItemTitle action:NULL keyEquivalent:@""];

                UIMenu menu = UIMenu.Create("", null);// [[NSMenu alloc] initWithTitle: @""];
                menu.AutoenablesItems :NO] ;
                menu.setAllowsContextMenuPlugIns:NO] ;
                menu.AddItem:theItem] ;

                // calculate the mouse's current position so we can present the menu from there since that's normal OSX behavior
                NSPoint mouseLocation = [NSEvent mouseLocation];
                CGPoint screenPoint = [self.window.screen convertPoint: NSPointToCGPoint(mouseLocation) fromScreen: nil];

                // modally present a menu with the single delete option on it, if it was selected, then do the delete, otherwise do nothing
                bool didSelectItem = menu.popUpMenuPositioningItem: nil atLocation: NSPointFromCGPoint(screenPoint) inView: self.window.screen.UIKitView];

                UIApplication.InterruptTouchesInView(nil);

                if (didSelectItem)
                {
                    _dataSource.commitEditingStyle(this, UITableViewCellEditingStyle.Delete, indexPath) ;
                }

                cell.Highlighted = false;*/
            }

            // all done
            this._endEditingRowAtIndexPath(indexPath);
        }
    }
}