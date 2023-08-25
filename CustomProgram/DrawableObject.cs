using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Every objects with drawing capability need this class 
    /// </summary>
    public abstract class DrawableObject
    {
        private float _x, _y; // the coordinate on screen
        public DrawableObject(float x, float y)
        {
            _x = x;
            _y = y;
        }
        public float X
        {
            get => _x;
            set => _x = value;
        }
        public float Y
        {
            get => _y;
            set => _y = value;
        }
        public abstract void Draw();
    }
}
