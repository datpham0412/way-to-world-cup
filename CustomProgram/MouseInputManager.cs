using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// based on the Observer pattern, pay attention to the click events of all MouseClickedEvent objects.
    /// </summary>
    public class MouseInputManager
    {
        
        private List<MouseClickedEvent> _observers; // a list of observers (objects with mouse action)
        public MouseInputManager() => _observers = new List<MouseClickedEvent>();
        
        public void Add(MouseClickedEvent observer) => _observers.Add(observer); // add observer
        
        public void NotifyObservers() // notify all observers to handle all click events at once
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                for (int i = 0; i < _observers.Count; i++)
                {
                    MouseClickedEvent observer = _observers[i];
                    if (observer.IsAt(SplashKit.MousePosition()))
                        observer.OnClick(EventArgs.Empty);
                }
            }
        }
        
    }
}
