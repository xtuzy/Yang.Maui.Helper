using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yang.Maui.Helper.Logs;

namespace Yang.Maui.Helper.Platforms.Android.Layouts
{
    public static class RelativeLayoutParamsHelper
    {
        public static RelativeLayout.LayoutParams AddRules(this RelativeLayout.LayoutParams lp, LayoutRules rule)
        {
            lp.AddRule(rule);
            return lp;
        }

        public static RelativeLayout.LayoutParams AddRules(this RelativeLayout.LayoutParams lp, LayoutRules rule, int vieId)
        {
            lp.AddRule(rule, vieId);
            return lp;
        }

        public static RelativeLayout.LayoutParams AddRules(this RelativeLayout.LayoutParams lp, LayoutRules rule, View view)
        {
            if (view.Id == View.NoId)
            {
                LogHelper.Debug("{0} {1}", view, "No id,will auto generate.");
                view.Id = View.GenerateViewId();
            }
            lp.AddRule(rule, view.Id);
            return lp;
        }

        #region 尝试和iOS一致
        public static View LeftToLeft(this View view, View secondView, int constant = 0)
        {
            var lp = view.LayoutParameters as RelativeLayout.LayoutParams;
            if (lp == null)
            {
                LogHelper.Debug("{0} {1}", view, "Not RelativeLayout.LayoutParams!");
                throw new ArgumentException();
            }
            else
            {
                if (constant == 0)
                {
                    lp.AddRules(LayoutRules.AlignLeft, secondView);//左边成一排
                }
                else
                {
                    lp.AddRules(LayoutRules.AlignLeft, secondView)
                        .SetLeftMargins(constant);//可以为负数
                }
            }
            return view;
        }

        /// <summary>
        /// 不确定是否依赖右边界,只确定在右边
        /// </summary>
        /// <param name="view"></param>
        /// <param name="secondView"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        public static View LeftToRight(this View view, View secondView, int constant = 0)
        {
            var lp = view.LayoutParameters as RelativeLayout.LayoutParams;
            if (lp == null)
            {
                LogHelper.Debug("{0} {1}", view, "Not RelativeLayout.LayoutParams!");
                throw new ArgumentException();
            }
            else
            {
                if (constant == 0)
                {
                    lp.AddRules(LayoutRules.RightOf, secondView);
                }
                else
                {
                    lp.AddRules(LayoutRules.RightOf, secondView)
                        .SetLeftMargins(constant);//可以为负数
                }
            }
            return view;
        }

        public static View RighToRight(this View view, View secondView, int constant = 0)
        {
            var lp = view.LayoutParameters as RelativeLayout.LayoutParams;
            if (lp == null)
            {
                LogHelper.Debug("{0} {1}", view, "Not RelativeLayout.LayoutParams!");
                throw new ArgumentException();
            }
            else
            {
                if (constant == 0)
                {
                    lp.AddRules(LayoutRules.AlignRight, secondView);
                }
                else
                {
                    lp.AddRules(LayoutRules.AlignRight, secondView)
                        .SetLeftMargins(constant);//可以为负数
                }
            }
            return view;
        }

        /// <summary>
        /// 不确定是否依赖左边界,只确定在左边
        /// </summary>
        /// <param name="view"></param>
        /// <param name="secondView"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        public static View RighToLeft(this View view, View secondView, int constant = 0)
        {
            var lp = view.LayoutParameters as RelativeLayout.LayoutParams;
            if (lp == null)
            {
                LogHelper.Debug("{0} {1}", view, "Not RelativeLayout.LayoutParams!");
                throw new ArgumentException();
            }
            else
            {
                if (constant == 0)
                {
                    lp.AddRules(LayoutRules.LeftOf, secondView);
                }
                else
                {
                    lp.AddRules(LayoutRules.LeftOf, secondView)
                        .SetLeftMargins(constant);//可以为负数
                }
            }
            return view;
        }

        public static View TopToTop(this View view, View secondView, int constant = 0)
        {
            var lp = view.LayoutParameters as RelativeLayout.LayoutParams;
            if (lp == null)
            {
                LogHelper.Debug("{0} {1}", view, "Not RelativeLayout.LayoutParams!");
                throw new ArgumentException();
            }
            else
            {
                if (constant == 0)
                {
                    lp.AddRules(LayoutRules.AlignTop, secondView);
                }
                else
                {
                    lp.AddRules(LayoutRules.AlignTop, secondView)
                        .SetLeftMargins(constant);//可以为负数
                }
            }
            return view;
        }

        public static View TopToBottom(this View view, View secondView, int constant = 0)
        {
            var lp = view.LayoutParameters as RelativeLayout.LayoutParams;
            if (lp == null)
            {
                LogHelper.Debug("{0} {1}", view, "Not RelativeLayout.LayoutParams!");
                throw new ArgumentException();
            }
            else
            {
                if (constant == 0)
                {
                    lp.AddRules(LayoutRules.Below, secondView);
                }
                else
                {
                    lp.AddRules(LayoutRules.Below, secondView)
                        .SetLeftMargins(constant);//可以为负数
                }
            }
            return view;
        }

        public static View BottomToBottom(this View view, View secondView, int constant = 0)
        {
            var lp = view.LayoutParameters as RelativeLayout.LayoutParams;
            if (lp == null)
            {
                LogHelper.Debug("{0} {1}", view, "Not RelativeLayout.LayoutParams!");
                throw new ArgumentException();
            }
            else
            {
                if (constant == 0)
                {
                    lp.AddRules(LayoutRules.AlignBottom, secondView);
                }
                else
                {
                    lp.AddRules(LayoutRules.AlignBottom, secondView)
                        .SetLeftMargins(constant);//可以为负数
                }
            }
            return view;
        }

        public static View BottomToTop(this View view, View secondView, int constant = 0)
        {
            var lp = view.LayoutParameters as RelativeLayout.LayoutParams;
            if (lp == null)
            {
                LogHelper.Debug("{0} {1}", view, "Not RelativeLayout.LayoutParams!");
                throw new ArgumentException();
            }
            else
            {
                if (constant == 0)
                {
                    lp.AddRules(LayoutRules.Above, secondView);
                }
                else
                {
                    lp.AddRules(LayoutRules.Above, secondView)
                        .SetLeftMargins(constant);//可以为负数
                }
            }
            return view;
        }

        #endregion
    }
}