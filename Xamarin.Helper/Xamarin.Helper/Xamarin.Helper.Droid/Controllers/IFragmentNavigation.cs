namespace Xamarin.Helper.Controllers
{
    public interface IFragmentNavigation
    {
        void PopToRootViewController();
        void PopToViewController(BaseFragment toFragment);
        void PopViewController();
        void PushViewController(BaseFragment toFragment);
    }
}