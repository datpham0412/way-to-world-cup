using SplashKitSDK;
using System;

namespace Custom_Program
{
    /// <summary>
    /// The user interface for objects having mouse click events
    /// </summary>
    public interface MouseClickedEvent
    {
        bool IsAt(Point2D point);
        event EventHandler ClickEvent;
        void OnClick(EventArgs e);
    }
}
