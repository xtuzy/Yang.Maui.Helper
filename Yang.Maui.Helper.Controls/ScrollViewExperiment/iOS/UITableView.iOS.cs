﻿using CoreGraphics;
using Foundation;
using System.Runtime.InteropServices;
using UIKit;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS
{
    public enum UITableViewStyle
    {
        Plain, Grouped
    }

    public enum UITableViewScrollPosition
    {
        None, Top, Middle, Bottom
    }

    public enum UITableViewRowAnimation
    {
        Fade, Right, Left, Top, Bottom, None, Middle, Automatic = 100
    }

    public delegate NSIndexPath willXRowAtIndexPathDelegate(UITableView tableView, NSIndexPath indexPath);
    public delegate void didXRowAtIndexPathDelegate(UITableView tableView, NSIndexPath indexPath);
    public delegate float heightForRowAtIndexPathDelegate(UITableView tableView, NSIndexPath indexPath);
    public delegate float heightForXInSectionDelegate(UITableView tableView, int section);
    public delegate UIView viewForHeaderInSectionDelegate(UITableView tableView, int section);
    public delegate void XRowAtIndexPathDelegate(UITableView tableView, NSIndexPath indexPath);
    public delegate string titleForDeleteConfirmationButtonForRowAtIndexPathDelegate(UITableView tableView, NSIndexPath indexPath);
    public interface IUITableViewDelegate : IUIScrollViewDelegate
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

    public delegate int numberOfRowsInSectionDelegate(UITableView tableView, int section);
    public delegate UITableViewCell cellForRowAtIndexPathDelegate(UITableView tableView, NSIndexPath indexPath);
    public delegate int numberOfSectionsInTableViewDelegate(UITableView tableView);
    public delegate string titleForXInSectionDelegate(UITableView tableView, int section);
    public delegate string EditintitleForFooterInSectionDelegate(UITableView tableView, int section);
    public delegate void commitEditingStyleDelegate(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath);
    public delegate bool canEditRowAtIndexPathDelegate(UITableView tableView, NSIndexPath indexPath);

    /// <summary>
    /// 最新的Api见
    /// https://learn.microsoft.com/en-us/dotnet/api/uikit.uitableviewdatasource.caneditrow?view=xamarin-ios-sdk-12
    /// </summary>
    public interface UITableViewDataSource
    {
        public numberOfRowsInSectionDelegate numberOfRowsInSection { get; }
        public cellForRowAtIndexPathDelegate cellForRowAtIndexPath { get; }
        public numberOfSectionsInTableViewDelegate numberOfSectionsInTableView { get; }
        public titleForXInSectionDelegate titleForHeaderInSection { get; }
        public titleForXInSectionDelegate titleForFooterInSection { get; }
        public commitEditingStyleDelegate commitEditingStyle { get; }
        public canEditRowAtIndexPathDelegate canEditRowAtIndexPath { get; }
    }

    public class UITableView : UIScrollView
    {
        // http://stackoverflow.com/questions/235120/whats-the-uitableview-index-magnifying-glass-character
        const string UITableViewIndexSearch = @"{search}";

        static readonly float _UITableViewDefaultRowHeight = 43;

        #region https://github.com/BigZaphod/Chameleon/blob/master/UIKit/Classes/UITableView.h
        UITableViewStyle _style;
        IUITableViewDelegate _delegate;
        UITableViewDataSource _dataSource;
        float _rowHeight;
        UITableViewCellSeparatorStyle _separatorStyle;

        UIColor separatorColor;
        UIView _tableHeaderView;
        UIView _tableFooterView;
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
        Dictionary<NSIndexPath, UITableViewCell> _cachedCells;
        List<UITableViewCell> _reusableCells;
        List<UITableViewSection> _sections;
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

        public UITableView(CoreGraphics.CGRect frame) : this(frame, UITableViewStyle.Plain)
        {

        }

        public UITableView(CoreGraphics.CGRect frame, UITableViewStyle style)
        {
            this._style = style;
            this._cachedCells = new();
            this._sections = new();
            this._reusableCells = new();
            this.separatorColor = new UIColor(red: 0.88f, green: 0.88f, blue: 0.88f, alpha: 1);
            this._separatorStyle = UITableViewCellSeparatorStyle.SingleLine;
            this.ShowsHorizontalScrollIndicator = false;
            this.allowsSelection = true;
            this.allowsSelectionDuringEditing = false;
            this._sectionHeaderHeight = this._sectionFooterHeight = 22;
            this.AlwaysBounceVertical = true;
            if (style == UITableViewStyle.Plain)
            {
                this.BackgroundColor = UIColor.White;
            }
            this._setNeedsReload();
        }

        public UITableViewDataSource DataSource
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

        public new IUITableViewDelegate Delegate
        {
            get { return this._delegate; }
            set
            {
                base.Delegate = value;
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

        public float RowHeight
        {
            get => _rowHeight;
            set
            {
                _rowHeight = value;
                this.SetNeedsLayout();
            }
        }

        void _updateSectionsCache()
        {
            // uses the dataSource to rebuild the cache.
            // if there's no dataSource, this can't do anything else.
            // note that I'm presently caching and hanging on to views and titles for section headers which is something
            // the real UIKit appears to fetch more on-demand than this. so far this has not been a problem.

            // remove all previous section header/footer views
            foreach (UITableViewSection previousSectionRecord in _sections)
            {
                previousSectionRecord.headerView?.RemoveFromSuperview();
                previousSectionRecord.footerView?.RemoveFromSuperview();
            }

            // clear the previous cache
            _sections.Clear();

            if (_dataSource != null)
            {
                // compute the heights/offsets of everything
                float defaultRowHeight = _rowHeight != default ? _rowHeight : _UITableViewDefaultRowHeight;
                int numberOfSections = this.numberOfSections();
                for (int section = 0; section < numberOfSections; section++)
                {
                    int numberOfRowsInSection = (int)this.numberOfRowsInSection(section);

                    UITableViewSection sectionRecord = new UITableViewSection();

                    sectionRecord.headerTitle = this._dataSourceHas.titleForHeaderInSection ? this._dataSource.titleForHeaderInSection(this, section) : null;

                    sectionRecord.footerTitle = this._dataSourceHas.titleForFooterInSection ? this._dataSource.titleForFooterInSection(this, section) : null;

                    sectionRecord.headerHeight = this._delegateHas.heightForHeaderInSection ? this._delegate.heightForHeaderInSection(this, section) : _sectionHeaderHeight;

                    sectionRecord.footerHeight = this._delegateHas.heightForFooterInSection ? this._delegate.heightForFooterInSection(this, section) : _sectionFooterHeight;

                    sectionRecord.headerView = sectionRecord.headerHeight > 0 && this._delegateHas.viewForHeaderInSection ? _delegate.viewForHeaderInSection(this, section) : null;

                    sectionRecord.footerView = sectionRecord.footerHeight > 0 && this._delegateHas.viewForFooterInSection ? _delegate.viewForFooterInSection(this, section) : null;

                    // make a default section header view if there's a title for it and no overriding view
                    if (sectionRecord.headerView != null && sectionRecord.headerHeight > 0 && sectionRecord.headerTitle != default)
                    {
                        sectionRecord.headerView = new UITableViewSectionLabel(sectionRecord.headerTitle);
                    }

                    // make a default section footer view if there's a title for it and no overriding view
                    if (sectionRecord.footerView != null && sectionRecord.footerHeight > 0 && sectionRecord.footerTitle != default)
                    {
                        sectionRecord.footerView = new UITableViewSectionLabel(sectionRecord.footerTitle);
                    }

                    if (sectionRecord.headerView != null)
                    {
                        this.AddSubview(sectionRecord.headerView);
                    }
                    else
                    {
                        sectionRecord.headerHeight = 0;
                    }

                    if (sectionRecord.footerView != null)
                    {
                        this.AddSubview(sectionRecord.footerView);
                    }
                    else
                    {
                        sectionRecord.footerHeight = 0;
                    }

                    float[] rowHeights = new float[numberOfRowsInSection];
                    float totalRowsHeight = 0;

                    for (int row = 0; row < numberOfRowsInSection; row++)
                    {
                        float rowHeight = _delegateHas.heightForRowAtIndexPath ? _delegate.heightForRowAtIndexPath(this, NSIndexPath.FromRowSection(row, section)) : defaultRowHeight;

                        rowHeights[row] = rowHeight;
                        totalRowsHeight += rowHeight;
                    }

                    sectionRecord.rowsHeight = totalRowsHeight;
                    sectionRecord.setNumberOfRows(numberOfRowsInSection, rowHeights);

                    _sections.Add(sectionRecord);
                }
            }
        }

        void _updateSectionsCacheIfNeeded()
        {
            // if there's a cache already in place, this doesn't do anything,
            // otherwise calls _updateSectionsCache.
            // this is called from _setContentSize and other places that require access
            // to the section caches (mostly for size-related information)

            if (_sections.Count == 0)
            {
                this._updateSectionsCache();
            }
        }

        void _setContentSize()
        {
            // first calls _updateSectionsCacheIfNeeded, then sets the scroll view's size
            // taking into account the size of the header, footer, and all rows.
            // should be called by reloadData, setFrame, header/footer setters.

            this._updateSectionsCacheIfNeeded();

            NFloat height = _tableHeaderView != null ? _tableHeaderView.Frame.Size.Height : 0;

            foreach (UITableViewSection section in _sections)
            {
                height += section.sectionHeight();
            }

            if (_tableFooterView != null)
            {
                height += _tableFooterView.Frame.Size.Height;
            }

            this.ContentSize = new CGSize(0, height);
        }

        void _layoutTableView()
        {
            // lays out headers and rows that are visible at the time. this should also do cell
            // dequeuing and keep a list of all existing cells that are visible and those
            // that exist but are not visible and are reusable
            // if there's no section cache, no rows will be laid out but the header/footer will (if any).

            CGSize boundsSize = this.Bounds.Size;
            NFloat contentOffset = this.ContentOffset.Y;
            CGRect visibleBounds = new CGRect(0, contentOffset, boundsSize.Width, boundsSize.Height);
            NFloat tableHeight = 0;

            if (_tableHeaderView != null)
            {
                CGRect tableHeaderFrame = _tableHeaderView.Frame;
                tableHeaderFrame.Location = CGPoint.Empty;
                tableHeaderFrame.Width = boundsSize.Width;
                _tableHeaderView.Frame = tableHeaderFrame;
                tableHeight += tableHeaderFrame.Size.Height;
            }

            // layout sections and rows
            Dictionary<NSIndexPath, UITableViewCell> availableCells = new();
            foreach (var cell in _cachedCells)
                availableCells.Add(cell.Key, cell.Value);
            int numberOfSections = _sections.Count;
            _cachedCells.Clear();

            for (int section = 0; section < numberOfSections; section++)
            {
                CGRect sectionRect = this.RectForSection(section);
                tableHeight += sectionRect.Size.Height;
                if (sectionRect.IntersectsWith(visibleBounds))
                {
                    CGRect headerRect = this.RectForHeaderInSection(section);
                    CGRect footerRect = this.RectForFooterInSection(section);
                    UITableViewSection sectionRecord = _sections[section];
                    int numberOfRows = sectionRecord.numberOfRows;

                    if (sectionRecord.headerView != null)
                    {
                        sectionRecord.headerView.Frame = headerRect;
                    }

                    if (sectionRecord.footerView != null)
                    {
                        sectionRecord.footerView.Frame = footerRect;
                    }

                    for (int row = 0; row < numberOfRows; row++)
                    {
                        NSIndexPath indexPath = NSIndexPath.FromRowSection(row, section);
                        CGRect rowRect = this.RectForRowAtIndexPath(indexPath);
                        if (rowRect.IntersectsWith(visibleBounds) && rowRect.Size.Height > 0)
                        {
                            UITableViewCell cell = availableCells.ContainsKey(indexPath) ? availableCells[indexPath] : this._dataSource.cellForRowAtIndexPath(this, indexPath);
                            if (cell != null)
                            {
                                _cachedCells[indexPath] = cell;
                                if (availableCells.ContainsKey(indexPath)) availableCells.Remove(indexPath);
                                cell.Highlighted = _highlightedRow == null ? false : _highlightedRow.IsEqual(indexPath);
                                cell.Selected = _selectedRow == null ? false : _selectedRow.IsEqual(indexPath);
                                cell.Frame = rowRect;
                                cell.BackgroundColor = this.BackgroundColor;
                                //cell.SeparatorStyle:_separatorStyle color:_separatorColor] ;
                                this.AddSubview(cell);
                            }
                        }
                    }
                }
            }

            // remove old cells, but save off any that might be reusable
            foreach (UITableViewCell cell in availableCells.Values)
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
            foreach (UITableViewCell cell in _reusableCells)
            {
                if (cell.Frame.IntersectsWith(visibleBounds) && !allCachedCells.Contains(cell))
                {
                    cell.RemoveFromSuperview();
                }
            }

            if (_tableFooterView != null)
            {
                CGRect tableFooterFrame = _tableFooterView.Frame;
                tableFooterFrame.Location = new CGPoint(0, tableHeight);
                tableFooterFrame.Width = boundsSize.Width;
                _tableFooterView.Frame = tableFooterFrame;
            }
        }

        CGRect _CGRectFromVerticalOffset(float offset, float height)
        {
            return new CGRect(0, offset, this.Bounds.Size.Width, height);
        }

        float _offsetForSection(int index)
        {
            float offset = (float)(_tableHeaderView != null ? _tableHeaderView.Frame.Size.Height : 0);

            for (int s = 0; s < index; s++)
            {
                offset += _sections[s].sectionHeight();
            }

            return offset;
        }

        public CGRect RectForSection(int section)
        {
            this._updateSectionsCacheIfNeeded();
            return this._CGRectFromVerticalOffset(this._offsetForSection(section), this._sections[section].sectionHeight());
        }

        public CGRect RectForHeaderInSection(int section)
        {
            this._updateSectionsCacheIfNeeded();
            return this._CGRectFromVerticalOffset(this._offsetForSection(section), this._sections[section].headerHeight);
        }

        public CGRect RectForFooterInSection(int section)
        {
            this._updateSectionsCacheIfNeeded();
            UITableViewSection sectionRecord = _sections[section];
            float offset = this._offsetForSection(section);
            offset += sectionRecord.headerHeight;
            offset += sectionRecord.rowsHeight;
            return this._CGRectFromVerticalOffset(offset, sectionRecord.footerHeight);
        }

        public CGRect RectForRowAtIndexPath(NSIndexPath indexPath)
        {
            this._updateSectionsCacheIfNeeded();
            if (indexPath != null && indexPath.Section < _sections.Count)
            {
                UITableViewSection sectionRecord = _sections[indexPath.Section];
                int row = indexPath.Row;
                if (row < sectionRecord.numberOfRows)
                {
                    float[] rowHeights = sectionRecord._rowHeights;
                    float offset = this._offsetForSection(indexPath.Section);
                    offset += sectionRecord.headerHeight;
                    for (int currentRow = 0; currentRow < row; currentRow++)
                    {
                        offset += rowHeights[currentRow];
                    }

                    return this._CGRectFromVerticalOffset(offset, rowHeights[row]);
                }
            }
            return CGRect.Empty;
        }

        void beginUpdates() { }

        void endUpdates() { }

        public UITableViewCell CellForRowAtIndexPath(NSIndexPath indexPath)
        {
            // this is allowed to return nil if the cell isn't visible and is not restricted to only returning visible cells
            // so this simple call should be good enough.
            if (indexPath == null) return null;
            return _cachedCells.ContainsKey(indexPath) ? _cachedCells[indexPath] : null;
        }

        List<NSIndexPath> indexPathsForRowsInRect(CGRect rect)
        {
            // This needs to return the index paths even if the cells don't exist in any caches or are not on screen
            // For now I'm assuming the cells stretch all the way across the view. It's not clear to me if the real
            // implementation gets anal about this or not (haven't tested it).

            this._updateSectionsCacheIfNeeded();

            List<NSIndexPath> results = new();
            int numberOfSections = _sections.Count;
            float offset = (float)(_tableHeaderView != null ? _tableHeaderView.Frame.Size.Height : 0);

            for (int section = 0; section < numberOfSections; section++)
            {
                UITableViewSection sectionRecord = _sections[section];
                float[] rowHeights = sectionRecord._rowHeights;
                int numberOfRows = sectionRecord.numberOfRows;
                offset += sectionRecord.headerHeight;
                if (offset + sectionRecord.rowsHeight >= rect.Location.Y)
                {
                    for (int row = 0; row < numberOfRows; row++)
                    {
                        float height = rowHeights[row];
                        CGRect simpleRowRect = new CGRect(rect.Location.X, offset, rect.Size.Width, height);

                        if (rect.IntersectsWith(simpleRowRect))
                        {
                            results.Add(NSIndexPath.FromRowSection(row, section));
                        }
                        else if (simpleRowRect.Location.Y > rect.Location.Y + rect.Size.Height)
                        {
                            break;  // don't need to find anything else.. we are past the end
                        }
                        offset += height;
                    }
                }
                else
                {
                    offset += sectionRecord.rowsHeight;
                }
                offset += sectionRecord.footerHeight;
            }
            return results;
        }

        public NSIndexPath IndexPathForRowAtPoint(CGPoint point)
        {
            var paths = this.indexPathsForRowsInRect(new CGRect(point.X, point.Y, 1, 1));
            return (paths.Count > 0) ? paths[0] : null;
        }

        public List<NSIndexPath> IndexPathsForVisibleRows
        {
            get
            {
                this._layoutTableView();
                List<NSIndexPath> indexes = new();
                CGRect bounds = this.Bounds;

                // Special note - it's unclear if UIKit returns these in sorted order. Because we're assuming that visibleCells returns them in order (top-bottom)
                // and visibleCells uses this method, I'm going to make the executive decision here and assume that UIKit probably does return them sorted - since
                // there's nothing warning that they aren't. :)
                var indexPaths = _cachedCells.Keys.ToList();
                indexPaths.Sort(delegate (NSIndexPath x, NSIndexPath y) { return (int)(x.Compare(y)); });
                foreach (NSIndexPath indexPath in indexPaths)
                {
                    if (bounds.IntersectsWith(this.RectForRowAtIndexPath(indexPath))) ;
                    {
                        indexes.Add(indexPath);
                    }
                }

                return indexes;
            }
        }

        public List<UITableViewCell> VisibleCells
        {
            get
            {
                List<UITableViewCell> cells = new();
                foreach (NSIndexPath index in this.IndexPathsForVisibleRows)
                {
                    UITableViewCell cell = this.CellForRowAtIndexPath(index);
                    if (cell != null)
                    {
                        cells.Add(cell);
                    }
                }
                return cells;
            }
        }

        public UIView TableHeaderView
        {
            get => _tableHeaderView;
            set
            {
                if (value != _tableHeaderView)
                {
                    _tableHeaderView?.Dispose();
                    _tableHeaderView = value;
                    _setContentSize();
                    this.AddSubview(_tableHeaderView);
                }
            }
        }

        public UIView TableFooterView
        {
            get => _tableFooterView;
            set
            {
                if (value != _tableFooterView)
                {
                    _tableFooterView?.Dispose();
                    _tableFooterView = value;
                    _setContentSize();
                    this.AddSubview(_tableFooterView);
                }
            }
        }

        public void setBackgroundView(UIView backgroundView)
        {
            if (_backgroundView != backgroundView)
            {
                _backgroundView?.Dispose();
                _backgroundView = backgroundView;
                this.InsertSubview(_backgroundView, 0);
            }
        }

        public int numberOfSections()
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

        public int numberOfRowsInSection(int section)
        {
            return _dataSource.numberOfRowsInSection(this, section);
        }

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
            this._updateSectionsCache();
            this._setContentSize();

            this._needsReload = false;
        }

        public void ReloadRowsAtIndexPaths(object[] indexPaths, UITableViewRowAnimation animation)
        {
            this.ReloadData();
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
            this.SetNeedsLayout();
        }

        public override void LayoutSubviews()
        {
            if (_backgroundView != null)
                _backgroundView.Frame = this.Bounds;
            this._reloadDataIfNeeded();
            this._layoutTableView();
            base.LayoutSubviews();
        }

        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                CGRect oldFrame = this.Frame;

                if (!oldFrame.Equals(value))
                {
                    base.Frame = value;

                    if (oldFrame.Size.Width != value.Size.Width)
                    {
                        this._updateSectionsCache();
                    }

                    this._setContentSize();
                }
            }
        }

        public NSIndexPath IndexPathForSelectedRow()
        {
            return _selectedRow;
        }

        public NSIndexPath IndexPathForCell(UITableViewCell cell)
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

        public void SelectRowAtIndexPath(NSIndexPath indexPath, bool animated, UITableViewScrollPosition scrollPosition)
        {
            // unlike the other methods that I've tested, the real UIKit appears to call reload during selection if the table hasn't been reloaded
            // yet. other methods all appear to rebuild the section cache "on-demand" but don't do a "proper" reload. for the sake of attempting
            // to maintain a similar delegate and dataSource access pattern to the real thing, I'll do it this way here. :)
            this._reloadDataIfNeeded();

            if (!(_selectedRow == indexPath))
            {
                this.DeselectRowAtIndexPath(_selectedRow, animated);
                _selectedRow = indexPath;
                this.CellForRowAtIndexPath(_selectedRow).Selected = true;
            }

            // I did not verify if the real UIKit will still scroll the selection into view even if the selection itself doesn't change.
            // this behavior was useful for Ostrich and seems harmless enough, so leaving it like this for now.
            this.ScrollToRowAtIndexPath(_selectedRow, scrollPosition, animated);
        }

        void _setUserSelectedRowAtIndexPath(NSIndexPath rowToSelect)
        {
            var Delegate = (this.Delegate as IUITableViewDelegate);
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

            this.SelectRowAtIndexPath(rowToSelect, false, UITableViewScrollPosition.None);

            if (_delegateHas.didSelectRowAtIndexPath)
            {
                Delegate.didSelectRowAtIndexPath(this, rowToSelect);
            }
        }

        void _scrollRectToVisible(CGRect aRect, UITableViewScrollPosition scrollPosition, bool animated)
        {
            if (!(aRect == CGRect.Empty) && aRect.Size.Height > 0)
            {
                // adjust the rect based on the desired scroll position setting
                switch (scrollPosition)
                {
                    case UITableViewScrollPosition.None:
                        break;

                    case UITableViewScrollPosition.Top:
                        aRect.Height = this.Bounds.Size.Height;
                        break;

                    case UITableViewScrollPosition.Middle:
                        aRect.Y -= (this.Bounds.Size.Height / 2.0f) - aRect.Size.Height;
                        aRect.Height = this.Bounds.Size.Height;
                        break;

                    case UITableViewScrollPosition.Bottom:
                        aRect.Y -= this.Bounds.Size.Height - aRect.Size.Height;
                        aRect.Height = this.Bounds.Size.Height;
                        break;
                }

                this.ScrollRectToVisible(aRect, animated: animated);
            }
        }

        public void ScrollToNearestSelectedRowAtScrollPosition(UITableViewScrollPosition scrollPosition, bool animated)
        {
            this._scrollRectToVisible(this.RectForRowAtIndexPath(this.IndexPathForSelectedRow()), scrollPosition, animated);
        }

        public void ScrollToRowAtIndexPath(NSIndexPath indexPath, UITableViewScrollPosition scrollPosition, bool animated)
        {
            this._scrollRectToVisible(this.RectForRowAtIndexPath(indexPath), scrollPosition, animated);
        }

        public UITableViewCell dequeueReusableCellWithIdentifier(string identifier)
        {
            foreach (UITableViewCell cell in _reusableCells)
            {
                if (cell.ReuseIdentifier == identifier)
                {
                    UITableViewCell strongCell = cell;

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

        public void InsertSections(NSIndexSet sections, UITableViewRowAnimation animation)
        {
            this.ReloadData();
        }

        public void DeleteSections(NSIndexSet sections, UITableViewRowAnimation animation)
        {
            this.ReloadData();
        }

        /// <summary>
        /// See <see cref="UIKit.UITableView.InsertRows(NSIndexPath[], UIKit.UITableViewRowAnimation)"/>
        /// </summary>
        /// <param name="indexPaths"></param>
        /// <param name="animation"></param>
        public void InsertRowsAtIndexPaths(NSIndexPath[] indexPaths, UITableViewRowAnimation animation)
        {
            this.ReloadData();
        }

        public void DeleteRowsAtIndexPaths(NSIndexPath[] indexPaths, UITableViewRowAnimation animation)
        {
            this.ReloadData();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
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
        }

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
                    (this.Delegate as IUITableViewDelegate).willBeginEditingRowAtIndexPath(this, indexPath);
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
                    (this.Delegate as IUITableViewDelegate).didEndEditingRowAtIndexPath(this, indexPath);
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

        void rightClick(NSSet touches, UIEvent evt)
        {
            UITouch touch = touches.AnyObject as UITouch;
            CGPoint location = touch.LocationInView(this);

            NSIndexPath touchedRow = this.IndexPathForRowAtPoint(location);

            // this is meant to emulate UIKit's swipe-to-delete feature on Mac by way of a right-click menu
            if (touchedRow != null && this._canEditRowAtIndexPath(touchedRow))
            {
                this._beginEditingRowAtIndexPath(touchedRow);
            }
        }
    }
}