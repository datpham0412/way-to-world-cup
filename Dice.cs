using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Dice give player value to move 
    /// </summary>
    public class Dice : DrawableObject
    {
        private Animation _animation;
        private Bitmap _image;
        private int _value;
        private bool _deactivated; // dice can be deactivated
        private bool _isRolling; // check to see whether the dice are rolling
        public Dice(float x, float y, Bitmap image) : base(x, y)
        {
            _value = 0;
            _image = image;
            _deactivated = false;
            _isRolling = false;
        }
        public Bitmap Image => _image;
        public Animation Anim
        {
            get => _animation;
            set => _animation = value;
        }
        public int Value => _value;
        public bool Deactivated
        {
            get => _deactivated;
            set => _deactivated = value;
        }
        public bool EndRolling()
        {
            if (_isRolling && Anim.Ended) 
            {
                _isRolling = false;
                return true;
            }
            return false;
        }
        
        public void Roll() // dice rolls 
        {
            _isRolling = true;
            _value = new Random().Next(1, 7);
            _animation.Assign(_value);
        }
        
        public void Reset() // reset the dice
        {
            _value = 0;
            _deactivated = true;
        }
        public override void Draw()
        {
            if (_deactivated)
            {
                return;
            }
            // create Drawing Options with Animation
            DrawingOptions diceOpt = SplashKit.OptionWithAnimation(_animation);
            SplashKit.DrawBitmapOnWindow(SplashKit.CurrentWindow(), _image, X, Y, diceOpt);
            _animation.Update();
        }
    }
}
