using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.ViewUtils.PlatformImageSource
{
    public sealed partial class PlatformImageSource : ImageSource
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(object), typeof(PlatformImageSource), default(object));

        public object Image
        {
            get { return (object)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public override Task<bool> Cancel()
        {
            return Task.FromResult(false);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == ImageProperty.PropertyName)
                OnSourceChanged();
            base.OnPropertyChanged(propertyName);
        }
    }
}