using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.CustomControls.Platform
{
    public class PlatformDrawEventArgs : EventArgs
    {
        public object PlatformDrawArgs;
        public PlatformDrawEventArgs(object platformDrawArgs)
        {
            PlatformDrawArgs = platformDrawArgs;
        }
    }
}
