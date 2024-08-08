using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Temporary button is a button that can be disabled and enabled
    /// </summary>
    public class TemporaryButton : Button
    {
        private bool _deactivated;
        public TemporaryButton(float x, float y, int width, int height, string name, int textSize, Color color, Color colorOnHover, Color textColor)
             : base(x, y, width, height, name, textSize, color, colorOnHover, textColor)
        {
            _deactivated = true;
        }
        public override void Draw()
        {
            if (!_deactivated)
                base.Draw();
        }
        public override bool IsAt(Point2D point)
        {
            return !_deactivated && base.IsAt(point);
        }
        public bool Deactivated
        {
            get { return _deactivated; }
            set { _deactivated = value; }
        }
    }
}
