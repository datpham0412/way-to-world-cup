using System;
using System.Collections.Generic;
using System.IO;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Represents the cell on the board ( Cell depends on Cell factory)
    /// </summary>
    public abstract class Cell : DrawableObject
    {
        private string _name; // cell's name
        protected Bitmap _image; // cell's image
        private float _angle; // angle to rotate to the left or right
        private bool _isClicked; // check if the cell is clicked by user ( if clicked - show details of the card)
        private int _coordinate; // cell's coordinate
        protected Board _board;
        public Cell(float x, float y, string name, Bitmap image, Board board) : base(x, y)
        {
            _name = name;
            _image = image;
            _angle = 0;
            _isClicked = false;
            _coordinate = 0;
            _board = board;
        }
        public string Name => _name;
        public Bitmap Image => _image;
        public float Angle
        {
            get => _angle;
            set => _angle = value;
        }
        public bool IsClicked
        {
            get => _isClicked;
            set => _isClicked = value;
        }
        // the surrounding quadilateral of the cell
        private Quad EncompassingQuad // rotate for the cell
        {
            get
            {
                float centerX = X + Image.Width / 2;
                float centerY = Y + Image.Height / 2;
                Point2D pt1 = GamingTools.FindRotatePoint(centerX, centerY, X - 1, Y - 1, _angle);
                Point2D pt2 = GamingTools.FindRotatePoint(centerX, centerY, X + Image.Width, Y - 1, _angle);
                Point2D pt3 = GamingTools.FindRotatePoint(centerX, centerY, X - 1, Y + Image.Height, _angle);
                Point2D pt4 = GamingTools.FindRotatePoint(centerX, centerY, X + Image.Width, Y + Image.Height, _angle);
                return new Quad()
                {
                    Points = new Point2D[] { pt1, pt2, pt3, pt4 }
                };
            }
        }
        public int Coordinate
        {
            get => _coordinate;
            set => _coordinate = value;
        }
        // rules when a player enter the cell
        public virtual string OnCellFunction(Player player) // cell funtion's when the player on their cell
        {
            player.IsInTurn = false;
            return "";
        }
        public override void Draw()
        {
            SplashKit.DrawBitmapOnWindow(SplashKit.CurrentWindow(), _image, X, Y, SplashKit.OptionRotateBmp(_angle));
            if (!IsClicked && !IsAt(SplashKit.MousePosition()))
                return;
            DrawOutline(Color.LightGoldenrodYellow);
        }
        public void DrawOutline(Color color) => SplashKit.DrawQuad(color, EncompassingQuad); // draw the outline of the cell
        public bool IsAt(Point2D point) => SplashKit.PointInQuad(point, EncompassingQuad); // check if the cell is in a specific location on the window
        public virtual void Load(QueryResult qr) { } // load cell information from the database
        public virtual string Description => ""; // the short description of the cell

    }
}
