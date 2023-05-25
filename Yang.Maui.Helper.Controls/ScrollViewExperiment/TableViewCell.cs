using System.Runtime.InteropServices;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    public enum TableViewCellAccessoryType
    {
        None, DisclosureIndicator, DetailDisclosureButton, Checkmark
    }

    public enum TableViewCellSeparatorStyle
    {
        None, SingleLine, SingleLineEtched
    }

    public enum TableViewCellStyle
    {
        Default, Value1, Value2, Subtitle
    }

    public enum TableViewCellSelectionStyle
    {
        None, Blue, Gray
    }

    public enum TableViewCellEditingStyle
    {
        None, Delete, Insert
    }

    public class TableViewCell : Grid
    {
        /// <summary>
        /// 存储Cell的位置, 
        /// </summary>
        internal Point PositionInLayout;

        public static float _UITableViewDefaultRowHeight;
        #region https://github.com/BigZaphod/Chameleon/blob/master/UIKit/Classes/UITableViewCell.h

        Label detailTextLabel;
        View _backgroundView;
        View _selectedBackgroundView;
        TableViewCellSelectionStyle _selectionStyle;
        int indentationLevel;
        TableViewCellAccessoryType accessoryType;
        View _accessoryView;
        TableViewCellAccessoryType editingAccessoryType;
        bool _selected;
        bool _highlighted;
        bool editing; // not yet implemented
        bool showingDeleteConfirmation;  // not yet implemented
        string _reuseIdentifier;  // not yet implemented
        float _indentationWidth; // 10 per default

        public string ReuseIdentifier => _reuseIdentifier;
        #endregion

        TableViewCellStyle _style;
        TableViewCellSeparator _seperatorView;
        Layout _contentView;
        Image _imageView;
        Label _textLabel;

        public TableViewCell(Rect frame)
        {
            _indentationWidth = 10;
            _style = TableViewCellStyle.Default;
            _selectionStyle = TableViewCellSelectionStyle.Blue;

            _seperatorView = new TableViewCellSeparator();
            this.Add(_seperatorView);

            this.accessoryType = TableViewCellAccessoryType.None;
            this.editingAccessoryType = TableViewCellAccessoryType.None;
        }

        public TableViewCell(TableViewCellStyle style, string reuseIdentifier) : this(new Rect(0, 0, 320, _UITableViewDefaultRowHeight))
        {
            _style = style;
            _reuseIdentifier = reuseIdentifier;
        }

        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }

        /*protected override Size ArrangeOverride(Rect bounds)
        {
            return base.ArrangeOverride(bounds);
        
            bool showingSeperator = _seperatorView.IsVisible;

            CGRect contentFrame = new CGRect(0, 0, bounds.Size.Width, bounds.Size.Height - (showingSeperator ? 1 : 0));
            CGRect accessoryRect = new CGRect(bounds.Size.Width, 0, 0, 0);

            if (_accessoryView != null)
            {
                accessoryRect.Size = _accessoryView.SizeThatFits(bounds.Size);
                accessoryRect.X = bounds.Size.Width - accessoryRect.Size.Width;
                accessoryRect.Y = (NFloat)Math.Round(0.5 * (bounds.Size.Height - accessoryRect.Size.Height));
                _accessoryView.Frame = accessoryRect;
                this.AddSubview(_accessoryView);
                contentFrame.Width = accessoryRect.Location.X - 1;
            }

            if (_backgroundView != null)
            {
                _backgroundView.Frame = contentFrame;
                this.SendSubviewToBack(_backgroundView);
            }

            if (_selectedBackgroundView != null)
            {
                _selectedBackgroundView.Frame = contentFrame;
                this.SendSubviewToBack(_selectedBackgroundView);
            }

            if (_contentView != null)
            {
                _contentView.Frame = contentFrame;
                this.BringSubviewToFront(_contentView);
            }

            if (_accessoryView != null)
                this.BringSubviewToFront(_accessoryView);

            if (showingSeperator)
            {
                _seperatorView.Frame = new CGRect(0, bounds.Size.Height - 1, bounds.Size.Width, 1);
                this.BringSubviewToFront(_seperatorView);
            }

            if (_style == TableViewCellStyle.Default)
            {
                float padding = 5;

                bool showImage = _imageView != null && _imageView.Image != null;
                float imageWidth = (showImage ? 30 : 0);

                if (showImage)
                    _imageView.Frame = new CGRect(padding, 0, imageWidth, contentFrame.Size.Height);

                CGRect textRect = new();
                textRect.Location = new CGPoint(padding + imageWidth + padding, 0);
                textRect.Size = new CGSize(Math.Max(0, contentFrame.Size.Width - textRect.Location.X - padding), contentFrame.Size.Height);
                if (_textLabel != null)
                    _textLabel.Frame = textRect;
            }
        }*/

        public Layout ContentView
        {
            get
            {
                if (_contentView == null)
                {
                    _contentView = new Grid();
                    this.Add(_contentView);
                    this.InvalidateMeasure();
                }

                return _contentView;
            }
        }

        public Image ImageView
        {
            get
            {
                if (_imageView == null)
                {
                    _imageView = new Image();
                    _imageView.Aspect = Aspect.Center;
                    this.ContentView.Add(_imageView);
                    this.InvalidateMeasure();
                }

                return _imageView;
            }
        }

        public Label TextLabel
        {
            get
            {
                if (_textLabel == null)
                {
                    _textLabel = new Label() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                    _textLabel.BackgroundColor = Colors.Transparent;
                    _textLabel.TextColor = Colors.Black;
                    //_textLabel.HighlightedTextColor = UIColor.White;
                    _textLabel.FontSize = 17;
                    _textLabel.FontAttributes = FontAttributes.Bold;
                    this.ContentView.Add(_textLabel);
                    this.InvalidateMeasure();
                }

                return _textLabel;
            }
        }

        void _setSeparatorStyle(TableViewCellSeparatorStyle theStyle, Color theColor)
        {
            _seperatorView.setSeparatorStyle(theStyle, theColor);
        }

        void _setHighlighted(bool highlighted, IList<IView> subviews)
        {
            foreach (var view in subviews)
            {
                if (view is IHighlightable)
                {
                    (view as IHighlightable).setHighlighted(highlighted);
                }

                if(view is Layout)
                    this._setHighlighted(highlighted, (view as Layout).Children);
            }
        }

        void _updateSelectionState()
        {
            bool shouldHighlight = _highlighted || _selected;
            if (_selectedBackgroundView != null)
                _selectedBackgroundView.IsVisible = shouldHighlight;
            this._setHighlighted(shouldHighlight, this.Children);
        }

        public void SetSelected(bool selected, bool animated)
        {
            if (selected != _selected && _selectionStyle != TableViewCellSelectionStyle.None)
            {
                _selected = selected;
                this._updateSelectionState();
            }
        }

        public bool Selected
        {
            set => this.SetSelected(value, false);
        }

        public void SetHighlighted(bool highlighted, bool animated)
        {
            if (_highlighted != highlighted && _selectionStyle != TableViewCellSelectionStyle.None)
            {
                _highlighted = highlighted;
                this._updateSelectionState();
            }
        }

        public bool Highlighted { set => this.SetHighlighted(value, false); get => _highlighted; }

        public View BackgroundView
        {
            get => _backgroundView;
            set
            {
                if (value != _backgroundView)
                {
                    _backgroundView.RemoveFromSuperview();
                    _backgroundView = value;
                    this.Add(_backgroundView);
                    this.BackgroundColor = Colors.Transparent;
                }
            }
        }

        public View SelectedBackgroundView
        {
            get => _selectedBackgroundView;
            set
            {
                if (value != _selectedBackgroundView)
                {
                    if (_selectedBackgroundView != null)
                        _selectedBackgroundView.RemoveFromSuperview();
                    _selectedBackgroundView = value;
                    _selectedBackgroundView.IsVisible = _selected;
                    this.Add(_selectedBackgroundView);
                }
            }
        }

        public static Point EmptyPoint = new Point(-1, -1);

        public bool IsEmpty = true;
        public virtual void PrepareForReuse()
        {
            IsEmpty = true;
            PositionInLayout = EmptyPoint;
        }
    }

    public interface IHighlightable
    {
        void setHighlighted(bool highlighted);
    }
}
