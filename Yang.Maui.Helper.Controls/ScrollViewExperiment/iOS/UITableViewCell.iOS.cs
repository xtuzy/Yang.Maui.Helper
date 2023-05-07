using CoreGraphics;
using System.Runtime.InteropServices;
using UIKit;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment.iOS
{
    public enum UITableViewCellAccessoryType
    {
        None, DisclosureIndicator, DetailDisclosureButton, Checkmark
    }

    public enum UITableViewCellSeparatorStyle
    {
        None, SingleLine, SingleLineEtched
    }

    public enum UITableViewCellStyle
    {
        Default, Value1, Value2, Subtitle
    }

    public enum UITableViewCellSelectionStyle
    {
        None, Blue, Gray
    }

    public enum UITableViewCellEditingStyle
    {
        None, Delete, Insert
    }

    public class UITableViewCell : UIView
    {
        public static float _UITableViewDefaultRowHeight;
        #region https://github.com/BigZaphod/Chameleon/blob/master/UIKit/Classes/UITableViewCell.h

        UILabel detailTextLabel;
        UIView _backgroundView;
        UIView _selectedBackgroundView;
        UITableViewCellSelectionStyle _selectionStyle;
        int indentationLevel;
        UITableViewCellAccessoryType accessoryType;
        UIView _accessoryView;
        UITableViewCellAccessoryType editingAccessoryType;
        bool _selected;
        bool _highlighted;
        bool editing; // not yet implemented
        bool showingDeleteConfirmation;  // not yet implemented
        string _reuseIdentifier;  // not yet implemented
        float _indentationWidth; // 10 per default

        public string ReuseIdentifier => _reuseIdentifier;
        #endregion

        UITableViewCellStyle _style;
        UITableViewCellSeparator _seperatorView;
        UIView _contentView;
        UIImageView _imageView;
        UILabel _textLabel;

        public UITableViewCell(CGRect frame) : base(frame)
        {
            _indentationWidth = 10;
            _style = UITableViewCellStyle.Default;
            _selectionStyle = UITableViewCellSelectionStyle.Blue;

            _seperatorView = new UITableViewCellSeparator();
            this.AddSubview(_seperatorView);

            this.accessoryType = UITableViewCellAccessoryType.None;
            this.editingAccessoryType = UITableViewCellAccessoryType.None;
        }

        public UITableViewCell(UITableViewCellStyle style, string reuseIdentifier) : this(new CGRect(0, 0, 320, _UITableViewDefaultRowHeight))
        {
            _style = style;
            _reuseIdentifier = reuseIdentifier;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CGRect bounds = this.Bounds;
            bool showingSeperator = !_seperatorView.Hidden;

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

            if (_style == UITableViewCellStyle.Default)
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
        }

        public UIView ContentView
        {
            get
            {
                if (_contentView == null)
                {
                    _contentView = new UIView();
                    this.AddSubview(_contentView);
                    this.LayoutIfNeeded();
                }

                return _contentView;
            }
        }

        public UIImageView ImageView
        {
            get
            {
                if (_imageView == null)
                {
                    _imageView = new UIImageView();
                    _imageView.ContentMode = UIViewContentMode.Center;
                    this.ContentView.AddSubview(_imageView);
                    this.LayoutIfNeeded();
                }

                return _imageView;
            }
        }

        public UILabel TextLabel
        {
            get
            {
                if (_textLabel == null)
                {
                    _textLabel = new UILabel();
                    _textLabel.BackgroundColor = UIColor.Clear;
                    _textLabel.TextColor = UIColor.Black;
                    _textLabel.HighlightedTextColor = UIColor.White;
                    _textLabel.Font = UIFont.BoldSystemFontOfSize(17);
                    this.ContentView.AddSubview(_textLabel);
                    this.LayoutIfNeeded();
                }

                return _textLabel;
            }
        }

        void _setSeparatorStyle(UITableViewCellSeparatorStyle theStyle, UIColor theColor)
        {
            _seperatorView.setSeparatorStyle(theStyle, theColor);
        }

        void _setHighlighted(bool highlighted, UIView[] subviews)
        {
            foreach (UIView view in subviews)
            {
                if (view is IHighlightable)
                {
                    (view as IHighlightable).setHighlighted(highlighted);
                }
                this._setHighlighted(highlighted, view.Subviews);
            }
        }

        void _updateSelectionState()
        {
            bool shouldHighlight = _highlighted || _selected;
            if (_selectedBackgroundView != null)
                _selectedBackgroundView.Hidden = !shouldHighlight;
            this._setHighlighted(shouldHighlight, this.Subviews);
        }

        public void SetSelected(bool selected, bool animated)
        {
            if (selected != _selected && _selectionStyle != UITableViewCellSelectionStyle.None)
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
            if (_highlighted != highlighted && _selectionStyle != UITableViewCellSelectionStyle.None)
            {
                _highlighted = highlighted;
                this._updateSelectionState();
            }
        }

        public bool Highlighted { set => this.SetHighlighted(value, false); get => _highlighted; }

        public UIView BackgroundView
        {
            get => _backgroundView;
            set
            {
                if (value != _backgroundView)
                {
                    _backgroundView.RemoveFromSuperview();
                    _backgroundView = value;
                    this.AddSubview(_backgroundView);
                    this.BackgroundColor = UIColor.Clear;
                }
            }
        }

        public UIView SelectedBackgroundView
        {
            get => _selectedBackgroundView;
            set
            {
                if (value != _selectedBackgroundView)
                {
                    if (_selectedBackgroundView != null)
                        _selectedBackgroundView.RemoveFromSuperview();
                    _selectedBackgroundView = value;
                    _selectedBackgroundView.Hidden = !_selected;
                    this.AddSubview(_selectedBackgroundView);
                }
            }
        }

        public void PrepareForReuse()
        {

        }
    }

    public interface IHighlightable
    {
        void setHighlighted(bool highlighted);
    }
}
