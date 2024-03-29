﻿using UIKit;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS
{
    public class UITableViewSection
    {
        public float rowsHeight;
        public float headerHeight;
        public float footerHeight;
        public int numberOfRows;
        public float[] _rowHeights;
        public UIView headerView;
        public UIView footerView;
        public string headerTitle;
        public string footerTitle;

        public float sectionHeight()
        {
            return this.rowsHeight + this.headerHeight + this.footerHeight;
        }

        public void setNumberOfRows(int rows, float[] newRowHeights)
        {
            _rowHeights = newRowHeights;
            numberOfRows = rows;
        }
    }
}
