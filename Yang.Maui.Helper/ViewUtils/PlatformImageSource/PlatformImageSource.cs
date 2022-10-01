using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.ViewUtils.PlatformImageSource
{
    public sealed partial class PlatformImageSource : ImageSource, IPlatformImageSource
    {
        public static readonly BindableProperty PlatformImageProperty = BindableProperty.Create(nameof(PlatformImage), typeof(object), typeof(PlatformImageSource), default(object));

        public object PlatformImage
        {
            get { return (object)GetValue(PlatformImageProperty); }
            set { SetValue(PlatformImageProperty, value); }
        }

        public override Task<bool> Cancel()
        {
            return Task.FromResult(false);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == PlatformImageProperty.PropertyName)
                OnSourceChanged();
            base.OnPropertyChanged(propertyName);
        }
    }
}