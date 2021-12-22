using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;

namespace Xamarin.Helper.Views
{
    /// <summary>
    /// 代表一个页面,对接Activity的根View,页面中的View统一由该Page管理,需要对接ViewModel数据的View使用public标记
    /// </summary>
    public class BasePage : BaseView
    {
        
        Space safeAreaOfTop;
        /// <summary>
        /// 顶部导航栏或者状态栏,或者挖孔屏,需要布局在他们以下区域
        /// </summary>
        public Space SafeAreaOfTop
        {
            get
            {
                if (safeAreaOfTop is null) 
                {
                    safeAreaOfTop = new Space(Context) { Id = View.GenerateViewId() };
                    safeAreaOfTop.LayoutParameters = new ConstraintLayout.LayoutParams(0, 0);
                    ((ConstraintLayout.LayoutParams)safeAreaOfTop.LayoutParameters).LeftToLeft = this.Id;
                    ((ConstraintLayout.LayoutParams)safeAreaOfTop.LayoutParameters).RightToRight = this.Id;
                }
                return safeAreaOfTop;
            }
        }
        /// <summary>
        /// 如曲面屏边缘可能视角不好,或者Pencil在边缘触控有影响
        /// </summary>
        Space safeAreaOfLeft;
        
        public Space SafeAreaOfLeft
        {
            get
            {
                if (safeAreaOfLeft is null)
                {
                    safeAreaOfLeft = new Space(Context) { Id = View.GenerateViewId() };
                    safeAreaOfLeft.LayoutParameters = new ConstraintLayout.LayoutParams(0, 0);
                    ((ConstraintLayout.LayoutParams)safeAreaOfLeft.LayoutParameters).TopToTop = this.Id;
                    ((ConstraintLayout.LayoutParams)safeAreaOfLeft.LayoutParameters).BottomToBottom = this.Id;
                }
                return safeAreaOfLeft;
            }
        }
        /// <summary>
        /// 如曲面屏边缘可能视角不好,或者Pencil在边缘触控有影响
        /// </summary>
        Space safeAreaOfRight;
        
        public Space SafeAreaOfRight
        {
            get
            {
                if (safeAreaOfRight is null)
                {
                    safeAreaOfRight = new Space(Context) { Id = View.GenerateViewId() };
                    safeAreaOfRight.LayoutParameters = new ConstraintLayout.LayoutParams(0, 0);
                    ((ConstraintLayout.LayoutParams)safeAreaOfRight.LayoutParameters).TopToTop = this.Id;
                    ((ConstraintLayout.LayoutParams)safeAreaOfRight.LayoutParameters).BottomToBottom = this.Id;
                }
                return safeAreaOfRight;
            }
        }
        /// <summary>
        /// 如底部拥有返回功能键时,需要布局在返回键以上区域
        /// </summary>
        Space safeAreaOfBottom;
        
        public Space SafeAreaOfBottom
        {
            get
            {
                if (safeAreaOfBottom is null)
                {
                    safeAreaOfBottom = new Space(Context) { Id = View.GenerateViewId()};
                    safeAreaOfBottom.LayoutParameters = new ConstraintLayout.LayoutParams(0, 0);
                    ((ConstraintLayout.LayoutParams)safeAreaOfBottom.LayoutParameters).LeftToLeft = this.Id;
                    ((ConstraintLayout.LayoutParams)safeAreaOfBottom.LayoutParameters).RightToRight = this.Id;
                }
                return safeAreaOfBottom;
            }
        }

        public BasePage(Context context) :
            base(context)
        {
            BasePage_Initialize();
        }

        public BasePage(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            BasePage_Initialize();
        }

        public BasePage(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            BasePage_Initialize();
        }

        private void BasePage_Initialize()
        {
            this.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        }
    }
}