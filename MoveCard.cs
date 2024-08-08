using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// This is the card that is related to moving players
    /// </summary>
    public class MoveCard : Card
    {
        public override void Activate(Player player, Board board)
        {
            int step = new Random().Next(-3, 4);
            Cell c;
            if (step < 0)
                Description = "Move " + (-step) + " steps backward";
            else if (step == 0)
                Description = "Nothing happens";
            else
                Description = "Move " + step + " steps forward";
            c = board.FindCell((player.Coordinate + step) % board.CellNumber);
            GamingTools.DisplayDelay(5000); // delay time popup the message when the player step on the cell ( 200 is nearly suddenly ) 
            player.MoveTo(board, c);
        }
    }
}
