namespace Yang.Maui.Helper.Platforms.Android.Controllers
{
    public interface IFragmentNavigation
    {
        void PopToRootViewController();
        void PopToViewController(BaseFragment toFragment);
        void PopViewController();
        void PushViewController(BaseFragment toFragment);
    }
}