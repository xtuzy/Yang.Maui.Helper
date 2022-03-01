using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Platform.Helper.Tools.MVVM
{
    public interface IViewModel
    {
        void SaveState(object bundle);
        void ReloadState(object bundle);
        /// <summary>
        /// ViewDidLoad
        /// </summary>
        void ViewCreated();

        void ViewAppearing();

        void ViewAppeared();

        void ViewDisappearing();

        void ViewDisappeared();

        /// <summary>
        /// DidMoveToParentViewController
        /// </summary>
        /// <param name="viewFinishing"></param>
        void ViewDestroy(bool viewFinishing = true);

    }
}
