using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using ScrollView = Android.Widget.ScrollView;
using View = Android.Views.View;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    public class MyScrollView : ScrollView
    {
        /// <summary>
        /// 创建Item和设置数据的适配器
        /// </summary>
        Adapter Adapter { get; set; }
        /// <summary>
        /// 当前显示的
        /// </summary>
        List<ViewHolder> CurrentShowedViewHolder = new();
        /// <summary>
        /// 移除后缓存的
        /// </summary>
        List<ViewHolder> CachedViewHolder = new();

        public void AddChildToLast(ViewHolder viewHolder)
        {

        }

        public MyScrollView(Context context) : base(context)
        {

        }

        protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
        {
            base.OnScrollChanged(l, t, oldl, oldt);
            ScrollViewContentView view = this.GetChildAt(0) as ScrollViewContentView;

            if (this.Height + this.ScrollY == view.Height)// Scrolled to Botttom
            {

            }
            else if (view.Height - (this.Height + this.ScrollY) < 50) //Load more when have a little distance to bottom, let it can still scroll
            {
                var currentLastPosition = CurrentShowedViewHolder.Last().BindingAdapterPosition;
                if (currentLastPosition < Adapter.ItemCount)
                {
                    var newLastViewHolder = GetViewHolder(currentLastPosition+1);
                    CurrentShowedViewHolder.Add(newLastViewHolder);
                    AddChildToLast(newLastViewHolder);
                }
            }
        }

        public ViewHolder GetViewHolder(int position)
        {
            ViewHolder viewHolder;
            if (CachedViewHolder.Count == 0)
            {
                viewHolder = Adapter.OnCreateViewHolder(this, Adapter.GetItemViewType(position)) as ViewHolder;
            }
            else
            {
                viewHolder = CachedViewHolder.First();
            }

            Adapter.BindViewHolder(viewHolder, position);
            return viewHolder;
        }

        public class ScrollViewContentView : LinearLayout
        {
            public ScrollViewContentView(Context context) : base(context)
            {
            }
        }
    }

    public class Adapter : AndroidX.RecyclerView.Widget.RecyclerView.Adapter
    {
        public override int ItemCount => throw new NotImplementedException();

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            throw new NotImplementedException();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            return base.GetItemId(position);
        }

        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
        }
    }

    public class ViewHolder : RecyclerView.ViewHolder
    {
        public ViewHolder(View itemView) : base(itemView)
        {
        }
    }
}
