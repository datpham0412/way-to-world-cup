using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// This class draw a button with click events for user
    /// </summary>
    public class Button : DrawableObject, MouseClickedEvent
    {
        private int _width, _height;
        private Color _color, _hoveringColor, _textColor;
        private string _name;
        private int _textSize;
        public Button(float x, float y, int width, int height, string name, int textSize, Color color, Color hoveringColor, Color textColor) : base(x, y)
        {
            _width = width;
            _height = height;
            _color = color;
            _hoveringColor = hoveringColor;
            _textColor = textColor;
            _name = name;
            _textSize = textSize;
        }
        public string Name => _name;
        public override void Draw()
        {
            if (!IsAt(SplashKit.MousePosition()))
                SplashKit.FillRectangle(_color, X, Y, _width, _height);
            else
                SplashKit.FillRectangle(_hoveringColor, X, Y, _width, _height);
            DrawText();
        }
        // Draw the text on the button
        private void DrawText()
        {
            float textX = X + (_width - SplashKit.TextWidth(Name, "GameFont", _textSize)) / 2;
            float textY = Y + (_height - SplashKit.TextHeight(Name, "GameFont", _textSize)) / 2;
            SplashKit.DrawText(Name, _textColor, "GameFont", _textSize, textX, textY);
        }
        public virtual bool IsAt(Point2D point)
        {
            return SplashKit.PointInRectangle(point, new Rectangle() { X = X, Y = Y, Width = _width, Height = _height });
        }
        public event EventHandler ClickEvent;
        public void OnClick(EventArgs myArgs)
        {
            ClickEvent?.Invoke(this, myArgs);
        }
    }
}
