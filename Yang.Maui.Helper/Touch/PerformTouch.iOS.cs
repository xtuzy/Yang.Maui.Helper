using UIKit;

namespace Yang.Maui.Helper.Touch
{
    /// <summary>
    /// 参考https://stackoverflow.com/questions/5625651/programmatically-fire-button-click-event
    /// </summary>
    public class PerformTouch
    {
        public static void Click(UIButton view, UIControlEvent actionEvent)
        {
            view.Highlighted = true;
            view.SendActionForControlEvents(actionEvent);
            view.Highlighted = false;
        }
    }
}
