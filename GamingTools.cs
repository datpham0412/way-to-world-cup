using System;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// This class use tools for creating game
    /// </summary>
    public static class GamingTools
    {
        // Pause for some action
        public static void DisplayDelay(uint time, Action action)
        {
            action(); // Action delegate does not return a value
            DisplayDelay(time);
        }
        // Pause for a specified time
        public static void DisplayDelay(uint time)
        {
            SplashKit.RefreshScreen();
            SplashKit.Delay(time);
        }
        // Find a new position when rotate a point around center point
        public static Point2D FindRotatePoint(float centerX, float centerY, float x, float y, float angle)
        {
            // Formula :
            // newX = (x - centerX)cosa - (y - centerY)sina
            // newY = (x - centerX)sina + (y - centerY)cosa
            // source : https://stackoverflow.com/questions/5180685/get-coordinates-after-rotation 
            float newX = (x - centerX) * SplashKit.Cosine(angle) - (y - centerY) * SplashKit.Sine(angle) + centerX;
            float newY = (x - centerX) * SplashKit.Sine(angle) + (y - centerY) * SplashKit.Cosine(angle) + centerY;
            return SplashKit.PointAt(newX, newY);
        }
    }
}
