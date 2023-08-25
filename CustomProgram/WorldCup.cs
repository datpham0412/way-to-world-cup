using System;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// World Cup is held on random country based on player chosen
    /// </summary>
    public class WorldCup : Cell
    {
        public WorldCup(float x, float y, string name, Bitmap image, Board board) : base(x, y, name, image, board) { }
        public override string OnCellFunction(Player player)
        {
            if (player.CityCount != 0)
            {
                player.IsChoosingWorldCup = true;
                return "Pick a city in your area to hold the World Cup";
            }
            player.IsInTurn = false;
            return player.Name + " does not have any estate to host a World Cup";
        }
        public override string Description => "Player can choose an area to host a World Cup\nThe rent for that city will double\nOnly one city can host a World Cup";
    }
}
