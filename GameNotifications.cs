using System;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// This Classs draw a message box which is utilised in games to convey messages
    /// </summary>
    public class GameNotifications : DrawableObject
    {
        private string _noti;
        private int _textSize;
        protected int _width;
        public string Noti
        {
            get => _noti;
            set => _noti = value;
        }
        public GameNotifications(float x, float y, int width, int textSize) : base(x, y)
        {
            _textSize = textSize;
            _noti = "";
            _width = width;
        }
        public override void Draw()
        {
            int notiWidth, notiHeight;
            string[] notiLines = _noti.Split('\n'); // Create a new line
            for (int i = 0; i < notiLines.Length; i++)
            {
                notiWidth = SplashKit.TextWidth(notiLines[i], "GameFont", _textSize);
                notiHeight = SplashKit.TextHeight(notiLines[i], "GameFont", _textSize);
                SplashKit.DrawText(notiLines[i], Color.Yellow, "GameFont", _textSize, X + (_width - notiWidth) / 2, Y + notiHeight * i);
            }
        }
    }
}
