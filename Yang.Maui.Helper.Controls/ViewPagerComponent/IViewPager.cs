using System.Collections.ObjectModel;

namespace Yang.Maui.Helper.Controls.ViewPagerComponent
{
    internal interface IViewPager : IView
    {
        View[] Views { get; }

        Action<int> ViewChanged { get; set; }

        internal Action<int> Goto { get; set; }

        internal void InvokeViewChanged(int index);
    }
}