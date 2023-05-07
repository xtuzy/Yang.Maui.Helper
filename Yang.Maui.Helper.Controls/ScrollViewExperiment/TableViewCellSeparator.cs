namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    public class TableViewCellSeparator : GraphicsView, IDrawable
    {
        TableViewCellSeparatorStyle _style;
        Color _color;

        public TableViewCellSeparator()
        {
            init();
        }

        public TableViewCellSeparator(Rect frame)
        {
            init();
        }

        void init()
        {
            Drawable = this;
            _style = TableViewCellSeparatorStyle.None;
            this.IsVisible = !true;
        }

        public void setSeparatorStyle(TableViewCellSeparatorStyle theStyle, Color theColor)
        {
            if (_style != theStyle)
            {
                _style = theStyle;
                this.InvalidateMeasure();
            }

            if (_color != theColor)
            {
                _color = theColor;
                this.InvalidateMeasure();
            }

            this.IsVisible = !(_style == TableViewCellSeparatorStyle.None);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_color != null)
            {
                if (_style == TableViewCellSeparatorStyle.SingleLine)
                {
                    canvas.FillColor = _color;
                    canvas.FillRectangle(new Rect(0, 0, dirtyRect.Size.Width, 1));
                }
            }
        }
    }
}
