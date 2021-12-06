using System.Windows;
//using Windows.ApplicationModel.Activation;

namespace Xamarin.Essentials
{
    public static partial class Platform
    {
        public static async void OnLaunched()
            => await AppActions.OnLaunched();
    }
}
