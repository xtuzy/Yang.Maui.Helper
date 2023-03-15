using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Controls.ViewPagerComponent
{
    public class ViewPager : View, IViewPager
    {
        private void ViewPager_SizeChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        public View[] Views { get; set; }

        public Action<int> ViewChanged { get; set; }
        
        Action<int> IViewPager.Goto { get; set; }

        public void GotoView(int index)
        {
            Handler?.Invoke(nameof(IViewPager.Goto), index);
        }

        void IViewPager.InvokeViewChanged(int index)
        {
            ViewChanged?.Invoke(index);
        }

        protected override Size ArrangeOverride(Rect bounds)
        {
            return base.ArrangeOverride(bounds);
        }
    }
}
