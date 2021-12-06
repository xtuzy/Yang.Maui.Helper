using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Helper.Tools;

namespace Xamarin.Helper.Adapters
{
    public class BaseSimpleAdapter : BaseAdapter
    {
        WeakReference<Context> context;

        public BaseSimpleAdapter(Context context)
        {
            this.context = new WeakReference<Context>(context);
        }

        /// <summary>
        /// Event for <see cref="GetItem(int)"/>
        /// </summary>
        public event GetItemEventHandler GetItemEvent;
        public override Java.Lang.Object GetItem(int position)
        {
            if(GetItemEvent != null)
                return GetItemEvent(position);
            return position;
        }

        /// <summary>
        /// Event for <see cref="GetItemId(int)"/>
        /// </summary>
        public event GetItemIdEventHandler GetItemIdEvent;
        public override long GetItemId(int position)
        {
            if(GetItemIdEvent != null)
                return GetItemIdEvent(position);
            return position;
        }

        /// <summary>
        /// Event for <see cref="GetView(int, View, ViewGroup)"/>
        /// </summary>
        public event GetViewEventHandler GetViewEvent;
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if(GetViewEvent != null)
                return GetViewEvent(position, convertView, parent);

            var view = convertView;
            BaseAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as BaseAdapterViewHolder;

            if (holder == null)
            {
                holder = new BaseAdapterViewHolder();
                context.TryGetTarget(out var activity);
                var inflater =  activity?.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                //view = inflater.Inflate(Resource.Layout.item, parent, false);
                //holder.Title = view.FindViewById<TextView>(Resource.Id.text);
                view.Tag = holder;
            }

            //fill in your items
            //holder.Title.Text = "new text here";

            return view;
        }

        /// <summary>
        /// Event for Count
        /// </summary>
        public event GetIntEventHandler GetIntEvent;
        //Fill in cound here, currently 0
        public override int Count
        {

            get
            {
                if(GetIntEvent!=null)
                    return GetIntEvent();
                return 0;
            }
        }
    }

    internal class BaseAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}