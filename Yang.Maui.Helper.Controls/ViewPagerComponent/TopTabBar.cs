using SharpConstraintLayout.Maui.Helper.Widget;
using SharpConstraintLayout.Maui.Widget;
using System.Diagnostics;
using Maui.BindableProperty.Generator.Core;
namespace Yang.Maui.Helper.Controls.ViewPagerComponent
{
    public partial class TopTabBar : ContentView
    {
        public System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string, View>> Tabs = new System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string, View>>();

        [AutoBindable(DefaultValue = "Colors.Green")]
        Color _selectedTabColor;

        [AutoBindable(DefaultValue = "Colors.Gray")]
        Color _defaultTabColor;

        public List<Button> TabButtons = new List<Button>();

        KeyValuePair<string, View>? currentTab;
        public KeyValuePair<string, View> CurrentTab
        {
            get
            {
                if(currentTab == null)
                    currentTab = Tabs.FirstOrDefault();
                return currentTab.Value;
            }
            set
            {
                currentTab = value;
            }
        }

        public Action<int> TabChanged;
        private ConstraintLayout layout;
        private Flow flexlayout;
        private ScrollView rootLayout;

        public TopTabBar()
        {
            layout = new ConstraintLayout();
            flexlayout = new SharpConstraintLayout.Maui.Helper.Widget.Flow();
            layout.Add(flexlayout);
            Tabs.CollectionChanged += Tabs_CollectionChanged;
            rootLayout = new ScrollView();
            rootLayout.Orientation = ScrollOrientation.Horizontal;
            rootLayout.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
        }

        private void Tabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //Tabs will add or decrease
            if(Tabs.Count > TabButtons.Count)
            {
                for(int i = TabButtons.Count; i < Tabs.Count; i++)
                {
                    var button = new Button();
                    layout.AddElement(button);
                    flexlayout.RefElement(button);
                    TabButtons.Add(button);
                    Debug.WriteLine(i);
                    button.Clicked += Button_Clicked;
                }
            }
            else
            {
                for (int i = TabButtons.Count - 1; i > Tabs.Count; i--)
                {
                    var button = TabButtons[i];
                    layout.RemoveElement(button);
                    flexlayout.RemoveRefElement(button);
                    TabButtons.Remove(button);
                    button.Clicked -= Button_Clicked;
                }
            }

            //The layout of tabs will have two states depending on the number of Tabs. If there are many Tabs, wrap them with ScrollView and arrange them in fixed Tab sizes. If there are few (according to the interface width and number), divide them equally.
            if (Tabs.Count <= 5)
            {
                if (this.Content != layout)
                {
                    this.Content = layout;
                    if (rootLayout.Content == layout)
                    {
                        rootLayout.Content = null;
                    }
                    layout.ConstrainWidth = FluentConstraintSet.MatchParent;
                    layout.ConstrainHeight = FluentConstraintSet.WrapContent;
                    flexlayout.SetHorizontalStyle(Flow.ChainSpread);
                    flexlayout.SetWrapMode(Flow.WrapChain);
                    using (var set = new FluentConstraintSet())
                    {
                        set.Clone(layout);
                        set.Select(flexlayout).Clear().EdgesXTo().Width(SizeBehavier.MatchParent)
                            .Height(SizeBehavier.WrapContent);
                        set.ApplyTo(layout);
                    }
                }
            }
            else
            {
                if(this.Content != rootLayout)
                {
                    this.Content = rootLayout;
                    rootLayout.Content = layout;
                    layout.ConstrainWidth = FluentConstraintSet.WrapContent;
                    layout.ConstrainHeight = FluentConstraintSet.WrapContent;

                    flexlayout.SetWrapMode(Flow.WrapNone);
                    flexlayout.SetHorizontalStyle(Flow.ChainPacked);
                    
                    using(var set = new FluentConstraintSet())
                    {
                        set.Clone(layout);
                        set.Select(flexlayout).Clear()
                            .LeftToLeft().TopToTop()
                            .Width(SizeBehavier.WrapContent)
                            .Height(SizeBehavier.WrapContent);
                        set.ApplyTo(layout);
                    }
                }
            }

            //maybe insert, so we reset all button
            for (var i = 0; i < Tabs.Count; i++)
            {
                TabButtons[i].BindingContext = this;
                if (Tabs[i].Key != CurrentTab.Key)
                {
                    TabButtons[i].SetBinding(Button.TextColorProperty, nameof(DefaultTabColor));
                }
                else
                {
                    TabButtons[i].SetBinding(Button.TextColorProperty, nameof(SelectedTabColor));
                }

                TabButtons[i].Text = Tabs[i].Key;
                TabButtons[i].CornerRadius = 0;
                TabButtons[i].BorderWidth = 0;
                TabButtons[i].SetBinding(Button.BackgroundColorProperty, nameof(BackgroundColor));
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var index = TabButtons.IndexOf(sender as Button);
            if (CurrentTab.Key == Tabs[index].Key && CurrentTab.Value == Tabs[index].Value)
                return;
            else
            {
                //change Tab need change some setting
                foreach (var button in TabButtons)
                {
                    if (button != sender)
                    {
                        button.SetBinding(Button.TextColorProperty, nameof(DefaultTabColor));
                    }
                    else
                    {
                        button.SetBinding(Button.TextColorProperty, nameof(SelectedTabColor));
                    }
                }
                CurrentTab = Tabs[index];
                TabChanged?.Invoke(index);
            }
        }

        public void Goto(int index)
        {
            if (index > Tabs.Count - 1 || index < 0 || index == Tabs.IndexOf(CurrentTab)) return;
            var button = TabButtons[index];
            Button_Clicked(button, null);
        }
    }
}