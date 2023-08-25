using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

using SplashKitSDK;

namespace Custom_Program
{
    public class Program
    {
        public static void Main()
        {
            PlayersGenerator playersGenerator = new();
            
            GameImplementation game = new GameImplementation(playersGenerator.Execute());
            Window w = new Window("Custom Program", 1200, 750);
            do
            {
                SplashKit.ProcessEvents();
                game.HandleInput();
                SplashKit.ClearScreen(Color.Gray);
                game.Draw();
                game.Update();
                SplashKit.RefreshScreen();
            } while (!SplashKit.WindowCloseRequested("Custom Program"));
        }
    }
}


